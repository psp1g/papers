using System;
using System.Linq;
using System.Collections.Generic;
using psp_papers_mod.Patches;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Events;
using TwitchLib.Communication.Models;

namespace psp_papers_mod.Twitch {

    public class TwitchIntegration {

        public const int EXPIRY_TIMEOUT_MINUTES = 5;
        public const int MAX_ACTIVE_CHATTER_HISTORY = 5;
        public const int MAX_CHATTER_FREQ_CHATS = 20;
        public const int MAX_CHATTER_OLDER_CHATS = 30;
        public const int ACTIVE_CHATTER_DENY_TIMEOUT_SECONDS = 120;
        public const float CHATS_WEIGHT_MODIFIER = 1.25f;
        public const float OLDER_CHATS_WEIGHT_MODIFIER = 0.5f;
        public const float DENIED_WEIGHT_MODIFIER = 0.8f;

        public ChatterCollection FrequentChatters { get; }

        public static Chatter ActiveChatter { get; set; }

        public static List<Chatter> RecentActiveChatters { get; private set; } = new();

        private TwitchClient client;

        public TwitchIntegration() {
            this.FrequentChatters = new ChatterCollection();

            PapersPSP.Log.LogInfo($"Connecting to {PapersPSP.Channel.Value} with bot {PapersPSP.BotName.Value}...");

            ConnectionCredentials credentials = new(PapersPSP.BotName.Value, PapersPSP.BotPass.Value);

            this.client = new TwitchClient(
                protocol: TwitchLib.Client.Enums.ClientProtocol.TCP
                //Logs everything from the twitch client to the console
                ,logger: new TwitchLogger<TwitchClient>()
            );
            this.client.Initialize(credentials, PapersPSP.Channel.Value);

            this.client.OnError += this.OnError;
            this.client.OnConnectionError += this.OnConnectionError;
            this.client.OnConnected += this.OnConnected;
            this.client.OnJoinedChannel += this.OnJoined;
            this.client.OnMessageReceived += this.OnMessage;
            this.client.OnUserBanned += this.OnUserBanned;
            this.client.OnUserTimedout += this.OnUserTimedOut;

            this.client.Connect();
        }
        ~TwitchIntegration() {
            this.client.Disconnect();
        }

        private void OnError(object sender, OnErrorEventArgs e) {
            PapersPSP.Log.LogError($"There was an error with twitch chat! Err: {e.Exception.Message}");
        }

        private void OnConnectionError(object sender, OnConnectionErrorArgs e) {
            PapersPSP.Log.LogError($"There was an error connecting to twitch chat! Err: {e.Error.Message}");
        }

        private void OnConnected(object sender, OnConnectedArgs e) {
            PapersPSP.Log.LogInfo("Connected to Twitch IRC.");
        }

        private void OnJoined(object sender, OnJoinedChannelArgs e) {
            PapersPSP.Log.LogInfo($"Connected to IRC Channel {PapersPSP.Channel.Value}");
            //this.client.SendMessage(PapersPSP.Channel.Value, $"+gofish pspTWEAK {new Random().Next()}");
        }

        private void OnMessage(object sender, OnMessageReceivedArgs e) {
            string username = e.ChatMessage.Username.ToLower();

            if (!this.FrequentChatters.ContainsKey(username))
                this.FrequentChatters.Add(username, new Chatter(e.ChatMessage.Username));

            Chatter chatter = this.FrequentChatters[username];
            chatter.Chatted();

            if (chatter.IsActiveChatter) {
                // User is the "active chatter" and their messages should appear as the traveler's
                BoothEnginePatch.Speak(e.ChatMessage.Message);
            }
        }

        /**
         * Set chatter weight to 0 on bans or time outs
         */
        private void HandleUserBan(string username) {
            this.FrequentChatters
                .TryGetValue(username.ToLower(), out Chatter chatter);

            chatter?.OnTimedOut();
        }
        private void OnUserBanned(object sender, OnUserBannedArgs e) { this.HandleUserBan(e.UserBan.Username); }
        private void OnUserTimedOut(object sender, OnUserTimedoutArgs e) { this.HandleUserBan(e.UserTimeout.Username); }

        public void TimeoutUser(string username, int seconds, string reason = "") {
            if (!this.client.IsInitialized || !this.client.IsConnected) return;
            // todo; Maybe replace with TwitchLib function to time out
            this.client.SendMessage(PapersPSP.Channel.Value, $"/timeout {username} {seconds} {reason}");
        }

        public static void SetActiveChatter(Chatter chatter) {
            ActiveChatter = chatter;
            RecentActiveChatters.Insert(0, chatter);

            // Cap history to max
            if (RecentActiveChatters.Count > MAX_ACTIVE_CHATTER_HISTORY)
                RecentActiveChatters = RecentActiveChatters
                    .Take(MAX_ACTIVE_CHATTER_HISTORY)
                    .ToList();
        }

    }

}