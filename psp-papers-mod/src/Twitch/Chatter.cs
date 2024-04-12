using System;
using System.Linq;
using System.Collections.Generic;
using psp_papers_mod.Patches;
using System.Threading.Tasks;
using TwitchLib.Api.Helix.Models.Moderation.BanUser;
using TwitchLib.Client.Models;
using StampApprovalKind = data.StampApprovalKind;

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

        public int RecentChats { get; set; } = 0;
        public int SemiRecentChats { get; set; } = 0;

        public bool IsActiveChatter => TwitchIntegration.ActiveChatter?.Username == this.Username;
        public bool WasDenied { get; set; }
        public bool HasBeenActiveChatter { get; set; }

        public bool WasRecentlyActiveChatter =>
            TwitchIntegration.RecentActiveChatters.Exists(c => c.Username == this.Username);

        public int RecentlyActiveChatterPosition =>
            TwitchIntegration.RecentActiveChatters.FindIndex(c => c.Username == this.Username);

        private bool JustDenied { get; set; }
        private bool BannedWhileTalking { get; set; }

        private List<double> RecentChatExpires = new List<double>();
        private List<double> SemiRecentChatExpires = new List<double>();

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

        public int GetWeight() {
            // Users who were banned while they are an active chatter have no chance of becoming the active chatter again
            if (this.BannedWhileTalking) return 0;

            double weight =
                (this.RecentChats * TwitchIntegration.CHATS_WEIGHT_MODIFIER) +
                (this.SemiRecentChats * TwitchIntegration.OLDER_CHATS_WEIGHT_MODIFIER);

            // Weight penalty if the user was previously already denied
            if (this.WasDenied) weight *= TwitchIntegration.DENIED_WEIGHT_MODIFIER;

            // Weight penalty for being a recent active chatter
            if (this.WasRecentlyActiveChatter && this.RecentlyActiveChatterPosition >= 0)
                weight *= (float)this.RecentlyActiveChatterPosition / TwitchIntegration.MAX_ACTIVE_CHATTER_HISTORY;

            // Chat role modifiers
            if (this.VIP) weight *= Cfg.VIPWeightMultiplier.Value;
            if (this.Moderator) weight *= Cfg.ModeratorWeightMultiplier.Value;
            if (this.Subscriber) weight *= Cfg.SubscriberWeightMultiplier.Value;
            if (this.TwitchStaff) weight *= Cfg.TwitchStaffWeightMultiplier.Value;

            return (int) System.Math.Round(weight);
        }

        public async Task JuicerCheck() {
            bool isJuicer = await IVR.CheckSubscribed(this.Username, "xqc");
            this.Juicer = isJuicer;
        }

        public void Deny(int seconds = 60) {
            TwitchIntegration.ActiveChatter = null;
            this.Timeout(seconds, "Denied Entry");

            this.WasDenied = true;
            this.JustDenied = true;
            this.Reset();
        }

        public async void Timeout(int seconds, string reason = "") {
            PapersPSP.Log.LogInfo($"Timing out user {this.Username}");
            BanUserRequest banRequest = new() {
                Reason = reason,
                Duration = seconds,
                UserId = this.UserID,
            };

            await PapersPSP.Twitch.api.Helix.Moderation.BanUserAsync(
                PapersPSP.Twitch.BroadcasterID,
                PapersPSP.Twitch.BotID,
                banRequest
            );
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
                BoothEnginePatch.Stamp(StampApprovalKind.DENIED);
            }

            // Reset recent chat stats so the chatter isn't selected as the active chatter while banned/timed out
            this.Reset();
        }

        public void Reset() {
            this.WasDenied = false;
            this.RecentChats = 0;
            this.SemiRecentChats = 0;
            this.RecentChatExpires.Clear();
            this.SemiRecentChatExpires.Clear();
        }

    }

}