using play.day.booth;
using psp_papers_mod.MonoBehaviour;
using System;
using System.Linq;
using System.Collections.Generic;
using psp_papers_mod.Patches;
using System.Threading.Tasks;
using TwitchLib.Api.Helix.Models.Moderation.BanUser;
using TwitchLib.Client.Models;

namespace psp_papers_mod.Twitch {

    public class Chatter {

        public string UserID { get; }

        public string Username { get; }
        public string First { get; }
        public string Last { get; }

        public bool VIP { get; }
        public bool Moderator { get; }
        public bool Subscriber { get; }
        public bool TwitchStaff { get; }
        public bool Streamer { get; }
        public bool Juicer { get; private set; }

        private bool JuicerChecked = false;

        public int RecentChats { get; set; } = 0;
        public int SemiRecentChats { get; set; } = 0;

        public bool IsActiveChatter => TwitchIntegration.ActiveChatter == this;
        public bool IsActiveAttacker => TwitchIntegration.ActiveAttacker == this;
        public bool WasDenied { get; set; }
        public bool WasApproved { get; set; }
        public bool HasBeenActiveChatter { get; set; }
        
        public bool HasBeenAttacker { get; set; }
        public bool Died { get; set; }

        public bool WasRecentlyActiveChatter =>
            TwitchIntegration.RecentActiveChatters.Exists(c => c.Username == this.Username);

        public int RecentlyActiveChatterPosition =>
            TwitchIntegration.RecentActiveChatters.FindIndex(c => c.Username == this.Username);

        private bool JustDenied { get; set; }
        private bool BannedWhileTalking { get; set; }

        private List<double> RecentChatExpires = [];
        private List<double> SemiRecentChatExpires = [];

        public Chatter(ChatMessage chatMessage) {
            this.UserID = chatMessage.UserId;
            this.Username = chatMessage.Username;

            this.Moderator = chatMessage.IsModerator;
            this.Subscriber = chatMessage.IsSubscriber;
            this.Streamer = chatMessage.IsBroadcaster;
            this.TwitchStaff = chatMessage.IsStaff;
            this.VIP = chatMessage.IsVip;

            // todo; maybe improve?
            // Find last uppercase letter in username
            int lastCapital = System.Array.FindLastIndex(this.Username.ToArray(), char.IsUpper);
            int splitIndex = lastCapital > 0
                ? lastCapital
                // Or split username in half as first and last name
                : (int) System.Math.Floor((float) this.Username.Length / 2);

            this.First = this.Username[..splitIndex];
            this.Last = this.Username[splitIndex..];
        }

        public void Chatted() {
            // Not allowed to be an active chatter if: They were banned while talking or; They're the streamer
            if (this.BannedWhileTalking || this.Streamer) return;
            if (this.HasBeenActiveChatter && Cfg.AlwaysPreventSameActiveChatter.Value) return;

            this.RecentChats = System.Math.Min(this.RecentChats + 1, TwitchIntegration.MAX_CHATTER_FREQ_CHATS);

            double now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            this.RecentChatExpires.Add(now + (60 * 5)); // 5 minutes
            this.SemiRecentChatExpires.Add(now + (60 * 15)); // 5 + 10 minutes
        }

        // todo; make non-blocking?
        // This could be heavy with a lot of chatters
        public void FlushExpired(double now) {
            bool hasRecent = this.RecentChats > 0;
            bool hasSemiRecent = this.SemiRecentChats > 0;

            if (!hasRecent && !hasSemiRecent) return;

            List<double> newExpires = hasRecent
                ? this.RecentChatExpires
                    .Where(expiry => now < expiry)
                    .ToList()
                : null;
            List<double> newExpiresSemi = hasSemiRecent
                ? this.SemiRecentChatExpires
                    .Where(expiry => now < expiry)
                    .ToList()
                : null;

            int recentExpired = this.RecentChatExpires.Count - (newExpires?.Count ?? 0);
            int semiRecentExpired = this.SemiRecentChatExpires.Count - (newExpiresSemi?.Count ?? 0);

            this.RecentChats =
                System.Math.Max(0,
                    System.Math.Min(this.RecentChats - recentExpired, TwitchIntegration.MAX_CHATTER_FREQ_CHATS)
                );
            this.SemiRecentChats =
                System.Math.Max(0,
                    System.Math.Min(this.SemiRecentChats + recentExpired - semiRecentExpired,
                        TwitchIntegration.MAX_CHATTER_OLDER_CHATS)
                );

            if (newExpires != null) this.RecentChatExpires = newExpires;
            if (newExpiresSemi != null) this.SemiRecentChatExpires = newExpiresSemi;
        }

        public int GetWeight(bool attacker = false) {
            bool disableSelectingApproved =
                attacker ? Cfg.AttacksDisableSelectingApproved.Value : Cfg.DisableSelectingApproved.Value;

            if (
                // If configured, users have no chance of becoming the active chatter again:
                // - Chatters who were banned while they are an active chatter
                // - Chatters who were attackers/shot aren't selected because they were shot/exploded
                // - Chatters who were approved and in the country already
                (this.BannedWhileTalking && Cfg.AlwaysPreventBannedWhileTalking.Value) ||
                ((this.HasBeenAttacker || this.Died) && Cfg.DeathsNeverSelectedAfter.Value) ||
                (this.WasApproved && disableSelectingApproved) ||

                // The active chatter can't be the attacker and vice versa
                (attacker && TwitchIntegration.ActiveChatter == this) ||
                (!attacker && TwitchIntegration.ActiveAttacker == this)
            )
                return 0;

            double weight =
                (this.RecentChats * TwitchIntegration.CHATS_WEIGHT_MODIFIER) +
                (this.SemiRecentChats * TwitchIntegration.OLDER_CHATS_WEIGHT_MODIFIER);

            // Weight multiplier if the user was previously already denied
            if (this.WasDenied) {
                if (attacker) weight *= Cfg.AttacksPreviouslyDeniedMultiplier.Value;
                else weight *= TwitchIntegration.DENIED_WEIGHT_MODIFIER;
            }

            // Weight penalty for being a recent active chatter
            if (!attacker && this.WasRecentlyActiveChatter && this.RecentlyActiveChatterPosition >= 0)
                weight *= (float)this.RecentlyActiveChatterPosition / TwitchIntegration.MAX_ACTIVE_CHATTER_HISTORY;

            // Chat role modifiers
            if (this.VIP) weight *= Cfg.VIPWeightMultiplier.Value;
            if (this.Moderator) weight *= Cfg.ModeratorWeightMultiplier.Value;
            if (this.Subscriber) weight *= Cfg.SubscriberWeightMultiplier.Value;
            if (this.TwitchStaff) weight *= Cfg.TwitchStaffWeightMultiplier.Value;

            return (int) System.Math.Round(weight);
        }

        public async Task JuicerCheck() {
            if (this.JuicerChecked) return;

            bool isJuicer = await IVR.CheckSubscribed(this.Username, "xqc");
            this.Juicer = isJuicer;
            this.JuicerChecked = true;
        }

        public void Approve() {
            this.WasApproved = true;
            AttackHandler.ConsecutiveDenials = 0;
        }

        public void Deny() {
            AttackHandler.ConsecutiveDenials++;
            TwitchIntegration.ActiveChatter = null;

            if (Cfg.DenyTimeoutSeconds.Value > 0)
                this.Timeout(Cfg.DenyTimeoutSeconds.Value, "Denied Entry");

            this.WasDenied = true;
            this.Reset();
        }

        public void Detain() {
            TwitchIntegration.ActiveChatter = null;
            if (Cfg.DetainedTimeoutSeconds.Value > 0)
                this.Timeout(Cfg.DetainedTimeoutSeconds.Value, "Detained");
        }

        public void Shot() {
            this.Died = true;

            if (TwitchIntegration.ActiveAttacker == this) {
                TwitchIntegration.ActiveAttacker = null;
                TwitchIntegration.ActiveAttackerPerson = null;
            }

            if (Cfg.ShotTimeoutSeconds.Value > 0)
                this.Timeout(Cfg.ShotTimeoutSeconds.Value, "Shot");
        }

        public async void Timeout(int seconds, string reason = "") {
            PapersPSP.Log.LogInfo($"Timing out user {this.Username}");

            if (Cfg.TimeoutDelayMs.Value > 0)
                await Task.Delay(Cfg.TimeoutDelayMs.Value);

            BanUserRequest banRequest = new() {
                Reason = reason,
                Duration = seconds,
                UserId = this.UserID,
            };

            this.JustDenied = true;

            try {
                await PapersPSP.Twitch.api.Helix.Moderation.BanUserAsync(
                    PapersPSP.Twitch.BroadcasterID,
                    PapersPSP.Twitch.BotID,
                    banRequest
                );
            } catch (Exception e) {
                PapersPSP.Log.LogError("Failed to send twitch ban! " + e.Message);
            }
        }

        public void OnTimedOut() {
            // Timed out because user was just denied
            if (this.JustDenied) {
                this.JustDenied = false;
                return;
            }

            // User was banned while talking, probably said something naughty on stream
            if (this.IsActiveChatter) {
                this.BannedWhileTalking = true;
                UnityThreadInvoker.Invoke(() => BoothEnginePatch.BoothEngine.day.addDetain());
            }

            // Reset recent chat stats so the chatter isn't selected as the active chatter while banned/timed out
            this.Reset();
        }

        public void Reset(bool gameEnd = false) {
            if (gameEnd) {
                this.Died = false;
                this.WasDenied = false;
                this.WasApproved = false;
                this.HasBeenAttacker = false;
            }

            this.RecentChats = 0;
            this.SemiRecentChats = 0;
            this.RecentChatExpires.Clear();
            this.SemiRecentChatExpires.Clear();
        }

    }

}