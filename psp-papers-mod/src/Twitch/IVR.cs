using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace psp_papers_mod.Twitch;

public class SubbedResult {

    public bool UserSubbed = false;

}

public class IVR {

    internal static readonly string subAgeUrl = "https://api.ivr.fi/v2/twitch/subage";

    public static async Task<SubbedResult> CheckSubscribed(string username, string channel) {
        try {
            using HttpClient client = new();
            HttpResponseMessage resp = await client.GetAsync($"{subAgeUrl}/{username}/{channel}").ConfigureAwait(false);

            if (!resp.IsSuccessStatusCode) {
                Console.Error.WriteLine($"IVR API ERROR: {resp.StatusCode.ToString()}");
                return new SubbedResult { UserSubbed = false };
            }

            string result = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

            JObject jsonO = JObject.Parse(result);
            JToken meta = jsonO["meta"];

            Console.Out.WriteLine(result);
            Console.Out.WriteLine(meta != null ? "META NOT NULL" : "META NULL");

            return new SubbedResult { UserSubbed = meta != null };
        }
        catch (Exception e) {
            Console.Error.WriteLine($"IVR Error: {e.Message}\n{e.StackTrace}");
        }

        return new SubbedResult { UserSubbed = false };
    }

}