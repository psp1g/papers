using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using psp_papers_mod.Patches;
using TwitchLib.Api;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;
using TwitchLib.Api.Helix.Models.Users.GetUsers;

namespace psp_papers_mod.Twitch {

    public class TwitchIntegration {

        public const int EXPIRY_TIMEOUT_MINUTES = 5;
        public const int MAX_ACTIVE_CHATTER_HISTORY = 20;
        public const int MAX_CHATTER_FREQ_CHATS = 20;
        public const int MAX_CHATTER_OLDER_CHATS = 30;
        public const int ACTIVE_CHATTER_DENY_TIMEOUT_SECONDS = 120;
        public const float CHATS_WEIGHT_MODIFIER = 1.25f;
        public const float OLDER_CHATS_WEIGHT_MODIFIER = 0.5f;
        public const float DENIED_WEIGHT_MODIFIER = 0.8f;

        public ChatterCollection FrequentChatters { get; }

        public static Chatter ActiveChatter { get; set; }

        public static List<Chatter> RecentActiveChatters { get; private set; } = new();

        public string BotID { get; private set; } = "";
        public string BroadcasterID { get; private set; } = "";

        internal TwitchAPI api;
        internal TwitchClient client;

        public TwitchIntegration() {
            this.FrequentChatters = new ChatterCollection();

            this.api = new TwitchAPI {
                Settings = {
                    ClientId = Cfg.ClientID.Value,
                    AccessToken = Cfg.AccessToken.Value
                }
            };

            PapersPSP.Log.LogInfo($"Connecting to {Cfg.Channel.Value} with bot {Cfg.BotName.Value}...");

            ConnectionCredentials credentials = new(Cfg.BotName.Value, Cfg.AccessToken.Value);

            this.client = new TwitchClient(
                protocol: TwitchLib.Client.Enums.ClientProtocol.TCP
                //Logs everything from the twitch client to the console
                ,logger: new TwitchLogger<TwitchClient>()
            );
            this.client.Initialize(credentials, Cfg.Channel.Value);

            this.client.OnError += this.OnError;
            this.client.OnConnectionError += this.OnConnectionError;
            this.client.OnConnected += this.OnConnected;
            this.client.OnJoinedChannel += this.OnJoined;
            this.client.OnMessageReceived += this.OnMessage;
            this.client.OnUserBanned += this.OnUserBanned;
            this.client.OnUserTimedout += this.OnUserTimedOut;
        }
        ~TwitchIntegration() {
            this.client.Disconnect();
        }

        public async Task Connect() {
            // Fetch the Bot and Broadcaster ID
            GetUsersResponse userResp =
                await this.api.Helix.Users.GetUsersAsync(null, [Cfg.BotName.Value, Cfg.Channel.Value]);

            User bot = userResp.Users[0];
            User broadcaster = userResp.Users[1];

            if (broadcaster == null || bot == null) {
                PapersPSP.Log.LogError($"Twitch API request for broadcaster/bot information failed! The Channel and/or Bot username is not valid");
                return;
            }

            this.BotID = bot.Id;
            this.BroadcasterID = broadcaster.Id;

            // Connect to Twitch Chat IRC
            this.client.Connect();
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
            PapersPSP.Log.LogInfo($"Connected to IRC Channel {Cfg.Channel.Value}");
            //this.client.SendMessage(PapersPSP.Channel.Value, $"+gofish pspTWEAK {new Random().Next()}");
        }

        private void OnMessage(object sender, OnMessageReceivedArgs e) {
            string username = e.ChatMessage.Username.ToLower();

            if (!this.FrequentChatters.ContainsKey(username))
                this.FrequentChatters.Add(username, new Chatter(e.ChatMessage));

            Chatter chatter = this.FrequentChatters[username];
            chatter.Chatted();

            // todo; check in a game
            // User is the "active chatter" and their messages should appear as the traveler's
            if (chatter.IsActiveChatter)
                BoothEnginePatch.Speak(e.ChatMessage.Message);
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

        public static bool Connected() {
            return PapersPSP.Twitch.client.IsInitialized && PapersPSP.Twitch.client.IsConnected;
        }

        public static void Message(string message) {
            if (!TwitchIntegration.Connected()) return;
            PapersPSP.Twitch.client.SendMessage(Cfg.Channel.Value, message);
        }

        public static void SetActiveChatter(Chatter chatter) {
            ActiveChatter = chatter;
            RecentActiveChatters.Insert(0, chatter);

            chatter.HasBeenActiveChatter = true;

            TwitchIntegration.Message($"@{chatter.Username}, You're currently the active chatter! Show me your papers! sus RightHand");

            // Cap history to max
            if (RecentActiveChatters.Count > MAX_ACTIVE_CHATTER_HISTORY)
                RecentActiveChatters = RecentActiveChatters
                    .Take(MAX_ACTIVE_CHATTER_HISTORY)
                    .ToList();
        }

    }

}