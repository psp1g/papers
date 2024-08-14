using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using psp_papers_mod.MonoBehaviour;
using System.Globalization;
using System.Linq;

namespace psp_papers_mod.src.Twitch;
internal class ChatterLocalization {


    public static string[] countries = ["Random", "Belarus", "Estonia", "Finland", "Latvia", "Lithuania", "Poland", "Sweden"];
    public static string[] countriesWithSususterja = countries.Append("Sususterja").ToArray();

    private static Dictionary<string, string> ChatterCountries = new ();

    private static string filePath = "ChatterCountries.json";

    // Add or update a chatter's country
    public static void AddOrUpdateChatter(string username, string country) {

        ChatterCountries[username] = country;
        if (country == "Random") ChatterCountries.Remove(username);

        SaveToFile().SuccessWithUnityThread(() => {});
    }

    // i think this causes the game to stutter
    // Get a chatter's country by their username
    public static string GetChatterCountry(string username) {
        if (ChatterCountries.TryGetValue(username, out var country)) {
            return country;
        }
        return null; 
    }

    public static bool ValidCountry(string country, bool sususterjaIncluded) {
        country = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(country);

        if (sususterjaIncluded) {
            return countriesWithSususterja.Contains(country);
        } else {
            return countries.Contains(country);
        }

    }

    // Save chatters to a JSON file
    public static async Task SaveToFile() {
        var json = JsonConvert.SerializeObject(ChatterCountries, Formatting.Indented);
        await File.WriteAllTextAsync(filePath, json);
    }
    
    // Load chatters from a JSON file
    public static void LoadFromFile() {
        if (File.Exists(filePath)) {
            var json = File.ReadAllText(filePath);
            ChatterCountries = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        } else {
            ChatterCountries = new Dictionary<string, string>();
        }
    }

}
