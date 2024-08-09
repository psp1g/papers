using Newtonsoft.Json.Linq;
using play.day.booth;
using psp_papers_mod.MonoBehaviour;
using psp_papers_mod.Patches;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;
using haxe.io;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;
using TwitchLib.Client.Models;
using System;

namespace psp_papers_mod;
internal class EmotePapers {

    public static string desiredEmote = "buh";

    // todo: set these ids according to channel
    private static string id = "104391402"; 
    private static string emoteSetUrl_7TV = "https://7tv.io/v3/users/twitch/" + id;
    private static string emoteSetUrl_BTTV = "https://api.betterttv.net/3/cached/users/twitch/" + id;
    private static string emoteSetUrl_FFZ = "https://api.frankerfacez.com/v1/room/psp1g";

    private static string emoteUrl_7TV = "https://cdn.7tv.app/emote/{0}/4x.webp";
    private static string emoteUrl_Twitch = "https://static-cdn.jtvnw.net/emoticons/v2/{0}/default/dark/3.0";
    private static string emoteUrl_BTTV = "https://cdn.betterttv.net/emote/{0}/3x.webp";

    private static JArray emotes_7TV;
    private static JArray emotes_FFZ;
    private static JArray emotes_BTTV;

    private static string emoteErrorUrl = "https://cdn.7tv.app/emote/6151c1de43b2d9da0d32adf9/4x.webp";

    public static void Initialize() {
        Console.WriteLine(emoteSetUrl_FFZ + " " + emoteSetUrl_BTTV);

        GetEmoteSet_7TV().ResultWithUnityThread((res) => {
            emotes_7TV = res;
            PapersPSP.Log.LogWarning("7TV emotes succesfully loaded!");
        });
        
        GetEmoteSet_FFZ().ResultWithUnityThread((res) => {
            emotes_FFZ = res;

            PapersPSP.Log.LogWarning("FZZ emotes succesfully loaded!");
        });
        
        GetEmoteSet_BTTV().ResultWithUnityThread((res) => {
            emotes_BTTV = res;
            PapersPSP.Log.LogWarning("BTTV emotes succesfully loaded!");
        });

    }

    static async Task<JArray> GetEmoteSet_7TV() {
        using (HttpClient client = new HttpClient()) {
            HttpResponseMessage response = await client.GetAsync(emoteSetUrl_7TV);

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            JObject jsonResponse = JObject.Parse(responseBody);
            JToken emoteSet = jsonResponse["emote_set"];
            return  (JArray)emoteSet["emotes"];
        }
    }

    static async Task<JArray> GetEmoteSet_BTTV() {
        using (HttpClient client = new HttpClient()) {
            HttpResponseMessage response = await client.GetAsync(emoteSetUrl_BTTV);

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            JObject jsonResponse = JObject.Parse(responseBody);
            JArray shared = (JArray) jsonResponse["sharedEmotes"];
            JArray owned = (JArray)jsonResponse["channelEmotes"];
            foreach (JToken emote in owned) {
                shared.Add(emote);
            }
            return shared;
        }
    }
    
    static async Task<JArray> GetEmoteSet_FFZ() {
            using (HttpClient client = new HttpClient()) {
                HttpResponseMessage response = await client.GetAsync(emoteSetUrl_FFZ);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                JObject jsonResponse = JObject.Parse(responseBody);
                JToken emoteSet = jsonResponse["sets"]["434949"]["emoticons"];

                return (JArray)emoteSet;
            }
    }
    
    public static string GetEmoteUrl_7TV(string emoteName) {
        if (emotes_7TV == null) return null;

        foreach (var emote in emotes_7TV) {
            if (emote["name"].ToString() == emoteName) {
                string url = String.Format(emoteUrl_7TV, emote["id"].ToString());
                return url;
            }
        }

        PapersPSP.Log.LogWarning("7TV Emote not found!");
        return null;
    }

    public static string GetEmoteUrl_FFZ(string emoteName) {
        if (emotes_FFZ == null) return null;

        foreach (var emote in emotes_FFZ) {
            if (emote["name"].ToString() == emoteName) {
                string url = emote["urls"]["4"].ToString();
                PapersPSP.Log.LogWarning(url);
                return url;
            }
        }

        PapersPSP.Log.LogWarning("FFZ Emote not found!");
        return null;
    }

    public static string GetEmoteUrl_BTTV(string emoteName) {
        if (emotes_BTTV == null) return null;

        foreach (var emote in emotes_BTTV) {
            if (emote["code"].ToString() == emoteName) {
                string url = String.Format(emoteUrl_BTTV, emote["id"].ToString());
                return url;
            }
        }

        PapersPSP.Log.LogWarning("BTTV Emote not found!");
        return null;
    }

    public static void GiveEmotePaper(string text, ChatMessage chatMessage) {
        string emoteUrl = null;
        if (emoteUrl == null) emoteUrl = GetEmoteUrl_7TV(text);
        if (emoteUrl == null) emoteUrl = GetEmoteUrl_FFZ(text);
        if (emoteUrl == null) emoteUrl = GetEmoteUrl_BTTV(text);

        //Twitch Emotes
        if (chatMessage.EmoteSet.Emotes.Count > 0) {
            emoteUrl = String.Format(emoteUrl_Twitch, chatMessage.EmoteSet.Emotes[0].Id);
            PapersPSP.Log.LogWarning(text + " " + chatMessage.EmoteSet.Emotes[0].ImageUrl);
        }

        if (emoteUrl == null) {
            PapersPSP.Log.LogInfo("Emote " + text + " not found!");
            return;
        }

        //PapersPSP.Log.LogInfo("Trying to give emote " + text + ": " + emoteUrl);

        BoothEnginePatch.GivePaperNow("emoteBlank");
        Paper paper = BorderPatch.Border.booth.autoFindPaper("emoteBlank");
        play.day.EnginePaper enginePaper = BorderPatch.Border.booth.engine.findEnginePaper("emoteBlank");

        DownloadImage(emoteUrl).ResultWithUnityThread((image) => {
            var (inner, outer) = CreateOuterAndInner(ref image);

            paper.deskItem.setInnerImage(inner, false);
            paper.deskItem.setMountImage(inner);
            paper.deskItem.setOuterImage(outer);

            string newId = "emote-" + text;
            paper.idWithIndex = newId;
            paper.deskItem.id = newId;
            paper.deskItem.idWithIndex = newId;
            enginePaper.idWithIndex = newId;

            var point = new app.vis.PointData(0, 0);
            paper.deskItem.startRevealAnim(point);
        });

    }
    
    static Bytes SharpImageToBytes(Image<Rgba32> image) {
        using (var ms = new MemoryStream()) {
            image.Save(ms, new PngEncoder { ColorType = PngColorType.RgbWithAlpha });
            byte[] data = ms.ToArray();
            Bytes result = new Bytes(data.Length, data);
            return result;
        }
    }

    public static (app.vis.Image, app.vis.Image) CreateOuterAndInner(ref Image img) {

        Image<Rgba32> inner = img.CloneAs<Rgba32>();
        Image<Rgba32> outer = img.CloneAs<Rgba32>();

        inner.Mutate(x => x.Resize(0, Cfg.BigEmoteHeight.Value, KnownResamplers.NearestNeighbor) );
        outer.Mutate(x => x.Resize(0, Cfg.SmallEmoteHeight.Value, KnownResamplers.Bicubic) );

        int sign = PapersPSP.Random.Next(2) == 0 ? -1 : 1;
        int degrees = PapersPSP.Random.Next(5, 30) * sign;

        outer.Mutate(x => x.Rotate(degrees, KnownResamplers.NearestNeighbor));

        var outerResult = app.vis.Image.fromPng( SharpImageToBytes(outer));
        var innerResult = app.vis.Image.fromPng( SharpImageToBytes(inner));
        inner.Dispose();
        outer.Dispose();
        
        return (innerResult, outerResult);
    }

    public static async Task<Image> DownloadImage(string url) {
        // todo: cache images - look for image in folder first or image.Save(path) after downloading

        using (HttpClient client = new HttpClient()) {
            HttpResponseMessage response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            byte[] webpBytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
            WebpDecoderOptions options = new() { BackgroundColorHandling = BackgroundColorHandling.Ignore };
            var img = WebpDecoder.Instance.Decode(options, new MemoryStream(webpBytes));
            
            //animated emotes get a weird white background for some reason
            if (img.Frames.Count > 1) {
                //RemoveWhiteBackground(img);
            }

            //Excessive frames cause stutter down the line while processing
            var result = img.Frames.CloneFrame(0);

            return result;
        }

    }

}
