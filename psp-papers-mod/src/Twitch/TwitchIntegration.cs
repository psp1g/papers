using psp_papers_mod.Patches;
using psp_papers_mod.MonoBehaviour;

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json;
using play.day.border;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Users.GetUsers;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Events;
using ChatCommand = psp_papers_mod.Twitch.Commands.ChatCommand;

namespace psp_papers_mod.Twitch;

public class TwitchIntegration {

    public const int EXPIRY_TIMEOUT_MINUTES = 5;
    public const int MAX_ACTIVE_CHATTER_HISTORY = 20;
    public const int MAX_CHATTER_FREQ_CHATS = 20;
    public const int MAX_CHATTER_OLDER_CHATS = 30;
    public const float CHATS_WEIGHT_MODIFIER = 1.25f;
    public const float OLDER_CHATS_WEIGHT_MODIFIER = 0.5f;
    public const float DENIED_WEIGHT_MODIFIER = 0.8f;

    public ChatterCollection FrequentChatters { get; }

    public static Chatter ActiveChatter { get; set; }

    public static Queue<Chatter> ForcedActiveQueue { get; set; } = [];
    
    public static Chatter ActiveAttacker { get; set; }
    
    public static Person ActiveAttackerPerson { get; set; }

    public static List<Chatter> RecentActiveChatters { get; private set; } = [];
    
    public static LimitedSizeDictionary<long, Chatter> ChattersPerPerson { get; private set; } = new(20);

    public string BotID { get; private set; } = "";
    public string BroadcasterID { get; private set; } = "";

    internal TwitchAPI api;
    internal TwitchClient client;

    private bool triedReAuth = false;

    public TwitchIntegration() {
        this.FrequentChatters = new ChatterCollection();

        Task<LocalAuthResponse> authTask = LocalAuthResponse.Fetch();
        authTask.Wait();

        LocalAuthResponse authResponse = authTask.Result;

        PapersPSP.Log.LogInfo(authResponse.ClientId);
        
        ChatCommand.FindAll();

        this.api = new TwitchAPI {
            Settings = {
                ClientId = authResponse.ClientId,
                AccessToken = authResponse.Token,
            }
        };

        PapersPSP.Log.LogInfo($"Connecting to {Cfg.Channel.Value} with bot {Cfg.BotName.Value}...");

        ConnectionCredentials credentials = new(Cfg.BotName.Value, authResponse.Token);

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
        this.client.OnNoPermissionError += this.OnAuthError;

        this.client.OnNewSubscriber += this.OnNewSub;
        this.client.OnReSubscriber += this.OnReSub;
        this.client.OnGiftedSubscription += this.OnGiftSub;
        this.client.OnPrimePaidSubscriber += this.OnPrimeSub;
        this.client.OnCommunitySubscription += this.OnCommunitySub;
        this.client.OnContinuedGiftedSubscription += this.OnContinueGiftedSub;
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

    private async Task ReAuthenticate() {
        LocalAuthResponse authResponse = await LocalAuthResponse.Fetch();
        ConnectionCredentials credentials = new(Cfg.BotName.Value, authResponse.Token);

        this.api.Settings.AccessToken = authResponse.Token;

        this.client.SetConnectionCredentials(credentials);
        this.client.Reconnect();
    }
    
    private void OnAuthError(object sender, EventArgs e) {
        if (this.triedReAuth) {
            PapersPSP.Log.LogError($"Re-Authentication with twitch has failed!");
            return;
        }

        PapersPSP.Log.LogWarning($"Authentication error with twitch. Attempting to re-authenticate");
        this.ReAuthenticate();
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
        //this.client.SendMessage(Cfg.Channel.Value, $"+gofish pspTWEAK {new Random().Next()}");
    }

    private void OnMessage(object sender, OnMessageReceivedArgs e) {
        string username = e.ChatMessage.Username.ToLower();

        if (!this.FrequentChatters.ContainsKey(username))
            this.FrequentChatters.Add(username, new Chatter(e.ChatMessage));

        Chatter chatter = this.FrequentChatters[username];
        chatter.Chatted();

        // todo; check in a game
        // User is the "active chatter" and their messages should appear as the traveler's
        if (chatter.IsActiveChatter) {
            BoothEnginePatch.Speak(e.ChatMessage.Message);

            if (e.ChatMessage.Bits > 0)
                this.OnBribe(chatter.Username, e.ChatMessage.BitsInDollars);
        }

        // User is the "active attacker" and their messages should pop up as the attacker's
        if (chatter.IsActiveAttacker) {
            if (ActiveAttackerPerson != null) {
                BorderPatch.Border.bomberSpeechBubble.pos = ActiveAttackerPerson.pos;
                BorderPatch.Border.bomberSpeechBubble.dottedLine.visible = false;
            }
            BorderPatch.Border.bomberSpeechBubble.showText(e.ChatMessage.Message, 0);
        }
    }

    /**
     * Set chatter weight to 0 on bans or timeouts
     */
    private void HandleUserBan(string username) {
        this.FrequentChatters
            .TryGetValue(username.ToLower(), out Chatter chatter);

        chatter?.OnTimedOut();
    }
    private void OnUserBanned(object sender, OnUserBannedArgs e) { this.HandleUserBan(e.UserBan.Username); }
    private void OnUserTimedOut(object sender, OnUserTimedoutArgs e) { this.HandleUserBan(e.UserTimeout.Username); }

    // Any twitch donation event thats >$3, give money in the game
    private void OnBribe(string username, double dollars = 5d) {
        if (ActiveChatter.Username != username && dollars >= 3) return;

        int roundedDollars = (int)System.Math.Round(Math.max(dollars, 5));
        int twenties = (int)System.Math.Floor(roundedDollars / 20d);
        int dollarsLeft = roundedDollars - (twenties * 20);

        int tens = (int)System.Math.Floor(dollarsLeft / 10d);
        dollarsLeft -= tens * 10;

        int fives = (int)System.Math.Floor(dollarsLeft / 5d);

        UnityThreadInvoker.Invoke(() => {
            BoothEnginePatch.GivePaperNow("Money5", fives);
            BoothEnginePatch.GivePaperNow("Money10", tens);
            BoothEnginePatch.GivePaperNow("Money20", twenties);
        });
    }
    private void OnNewSub(object sender, OnNewSubscriberArgs e) { this.OnBribe(e.Subscriber.Login); }
    private void OnReSub(object sender, OnReSubscriberArgs e) { this.OnBribe(e.ReSubscriber.Login); }
    private void OnGiftSub(object sender, OnGiftedSubscriptionArgs e) { this.OnBribe(e.GiftedSubscription.Login); }
    private void OnPrimeSub(object sender, OnPrimePaidSubscriberArgs e) { this.OnBribe(e.PrimePaidSubscriber.Login); }
    private void OnCommunitySub(object sender, OnCommunitySubscriptionArgs e) { this.OnBribe(e.GiftedSubscription.Login, e.GiftedSubscription.MsgParamMassGiftCount * 5); }
    private void OnContinueGiftedSub(object sender, OnContinuedGiftedSubscriptionArgs e) { this.OnBribe(e.ContinuedGiftedSubscription.Login); }

    public static bool IsConnected() {
        return PapersPSP.Twitch.client.IsInitialized && PapersPSP.Twitch.client.IsConnected;
    }

    public static void Message(string message) {
        if (!IsConnected()) return;
        PapersPSP.Twitch.client.SendMessage(Cfg.Channel.Value, message);
    }

    public static void SetActiveChatter(Chatter chatter) {
        ActiveChatter = chatter;
        RecentActiveChatters.Insert(0, chatter);

        chatter.HasBeenActiveChatter = true;

        Message($"@{chatter.Username}, You have been selected for entry into Sususterja flagSususterja . Please proceed to the booth.");

        // Cap history to max
        if (RecentActiveChatters.Count > MAX_ACTIVE_CHATTER_HISTORY)
            RecentActiveChatters = RecentActiveChatters
                .Take(MAX_ACTIVE_CHATTER_HISTORY)
                .ToList();
    }

    public static void SetActiveAttacker(Chatter chatter) {
        ActiveAttacker = chatter;
        ActiveAttackerPerson = null;
        chatter.HasBeenAttacker = true;
    }

}

internal class LocalAuthResponse {

    [JsonProperty("clientId")]
    public string ClientId { get; set; }
    
    [JsonProperty("token")]
    public string Token { get; set; }
    
    [JsonProperty("expires")]
    public DateTime? Expires { get; set; }

    private static LocalAuthResponse DefaultAuthResponse() =>
        new() {
            Expires = null,
            Token = Cfg.AccessToken.Value,
            ClientId = Cfg.ClientID.Value,
        };

    internal static async Task<LocalAuthResponse> Fetch() {
        if (!Cfg.UseLocalAuthServer.Value) return DefaultAuthResponse();

        using HttpClient client = new();

        HttpResponseMessage response =
            await client.PostAsJsonAsync(
                "http://localhost:56709/api/token", 
                new { botUsername = Cfg.BotName.Value, forceRefresh = true, }
            );
        Stream stream = await response.Content.ReadAsStreamAsync();

        using StreamReader streamReader = new(stream);
        await using JsonTextReader jsonReader = new(streamReader);

        JsonSerializer jsonSerializer = new();

        try {
            return jsonSerializer.Deserialize<LocalAuthResponse>(jsonReader);
        } catch (JsonReaderException) {
            PapersPSP.Log.LogWarning($"There was an error with local auth JSON response!");
        }

        return DefaultAuthResponse();
    }

}