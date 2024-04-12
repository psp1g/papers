using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace psp_papers_mod.Twitch;

public class IVR {

    internal static readonly string subAgeUrl = "https://api.ivr.fi/v2/twitch/subage";

    public static async Task<bool> CheckSubscribed(string username, string channel) {
        try {
            using HttpClient client = new();
            HttpResponseMessage resp = await client.GetAsync($"{subAgeUrl}/{username}/{channel}").ConfigureAwait(false);

            if (!resp.IsSuccessStatusCode) {
                Console.Error.WriteLine($"IVR API ERROR: {resp.StatusCode.ToString()}");
                return false;
            }

            string result = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

            JObject jsonO = JObject.Parse(result);
            JToken meta = jsonO["meta"];

            return meta?.HasValues == true;
        } catch (Exception e) {
            Console.Error.WriteLine($"IVR Error: {e.Message}\n{e.StackTrace}");
        }

        return false;
    }

}