using data;
using System;
using System.Linq;
using System.Collections.Generic;
using psp_papers_mod.Patches;
using StampApprovalKind = data.StampApprovalKind;

namespace psp_papers_mod.Twitch {

    public class Chatter {

        public string Username { get; }
        public string First { get; }
        public string Last { get; }
        public int RecentChats { get; set; } = 0;
        public int SemiRecentChats { get; set; } = 0;

        public bool IsActiveChatter => TwitchIntegration.ActiveChatter?.Username == this.Username;
        public bool WasDenied { get; set; }

        public bool WasRecentlyActiveChatter =>
            TwitchIntegration.RecentActiveChatters.Exists(c => c.Username == this.Username);

        public int RecentlyActiveChatterPosition =>
            TwitchIntegration.RecentActiveChatters.FindIndex(c => c.Username == this.Username);

        private bool JustDenied { get; set; }
        private bool BannedWhileTalking { get; set; }

        private List<double> RecentChatExpires = new List<double>();
        private List<double> SemiRecentChatExpires = new List<double>();

        public Chatter(string username) {
            this.Username = username;

            // Find last uppercase letter in username
            int lastCapital = System.Array.FindLastIndex(username.ToArray(), char.IsUpper);
            int splitIndex = lastCapital > 0
                ? lastCapital
                // Or split username in half as first and last name
                : (int) System.Math.Floor((float) username.Length / 2);

            this.First = username[..splitIndex];
            this.Last = username[splitIndex..];
        }

        public void Chatted() {
            if (this.BannedWhileTalking) return;

            this.RecentChats = System.Math.Min(this.RecentChats + 1, TwitchIntegration.MAX_CHATTER_FREQ_CHATS);

            double now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            this.RecentChatExpires.Add(now + (60 * 5)); // 5 minutes
            this.SemiRecentChatExpires.Add(now + (60 * 15)); // 5 + 10 minutes
        }

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
            if (this.WasRecentlyActiveChatter && this.RecentlyActiveChatterPosition > 0)
                weight *= (float)this.RecentlyActiveChatterPosition / TwitchIntegration.MAX_ACTIVE_CHATTER_HISTORY;

            return (int) System.Math.Round(weight);
        }

        public void Deny(int seconds) {
            TwitchIntegration.ActiveChatter = null;
            PapersPSP.Twitch.TimeoutUser(this.Username, seconds, "Denied Entry");

            this.WasDenied = true;
            this.JustDenied = true;
            this.Reset();
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