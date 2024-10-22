using TwitchLib.Client.Models;
using psp_papers_mod.MonoBehaviour;
using psp_papers_mod.Patches;
using System.Linq;
using psp_papers_mod.src.Twitch;
using System.Globalization;

namespace psp_papers_mod.Twitch.Commands;

public static class Commands {

    private static bool LeaveEnabled = true;
    
    [ChatCommand("leave")]
    public static void LeaveCommand(Chatter sender, ChatMessage chatMessage, string[] _) {
        if (!sender.Moderator && !sender.Streamer && (!sender.IsActiveChatter || !LeaveEnabled)) return;
        // Force the active booth chatter to leave
        UnityThreadInvoker.Invoke(BoothEnginePatch.ForceTravelerLeave);
    }
    
    [ChatCommand("disableleave")]
    public static void DisableLeaveCommand(Chatter sender, ChatMessage chatMessage, string[] _) {
        if (!sender.Moderator && !sender.Streamer) return;
        LeaveEnabled = false;
    }
    
    
    [ChatCommand("enableleave")]
    public static void EnableLeaveCommand(Chatter sender, ChatMessage chatMessage, string[] _) {
        if (!sender.Moderator && !sender.Streamer) return;
        LeaveEnabled = true;
    }

    [ChatCommand("force")]
    public static async void ForceCommand(Chatter sender, ChatMessage chatMessage, string[] args) {
        if (!sender.Moderator && !sender.Streamer) return;

        Chatter forceChatter = sender;

        if (args.Length > 0) {
            string chatterName = args[0].ToLower();
            forceChatter = await PapersPSP.Twitch.FrequentChatters.FromUsername(chatterName);

            if (forceChatter == null) {
                chatMessage.Reply("I can't find a chatter by that username!");
                return;
            }
        }

        TwitchIntegration.ForcedActiveQueue.Enqueue(forceChatter);
    }

    [ChatCommand("attack")]
    public static void Attack(Chatter sender, ChatMessage chatMessage, string[] args) {
        sender.WantsAttack();
    }

    // todo; PAP-23
    [ChatCommand("bomb")]
    public static void Bomb(Chatter sender, ChatMessage chatMessage, string[] args) {
        sender.WantsBomb = true;
    }

    [ChatCommand("nobomb")]
    public static void NoBomb(Chatter sender, ChatMessage chatMessage, string[] args) {
        sender.WantsBomb = false;
    }
    
    [ChatCommand("passport")]
    public static void SetCountry(Chatter sender, ChatMessage chatMessage, string[] args) {

        if (args.Length == 1) {

            string country = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(args[0]);
            bool canSususterja = (sender.Moderator || sender.VIP || sender.TwitchStaff);
            
            if (country == "Sususterja" && !canSususterja) {
                chatMessage.Reply("You're not eligible for a Sususterjan passport GAGAGA");
                return;
            }

            if (ChatterLocalization.ValidCountry(country, canSususterja)) {
                ChatterLocalization.AddOrUpdateChatter(sender.Username, country);
            } else {

                string sususterjaText = canSususterja ? ", Sususterja." : ".";
                chatMessage.Reply("Invalid country! "
                                  + "Choose from: " + string.Join(", ", ChatterLocalization.countries) 
                                  + sususterjaText
                );
            }

        } else if (args.Length == 2) {

            if (!sender.Moderator && !sender.Streamer)
                return;

            string username = args[0];
            string country = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(args[1]);

            if (ChatterLocalization.ValidCountry(country, true)) {
                ChatterLocalization.AddOrUpdateChatter(username, country);
            } else {
                chatMessage.Reply("Invalid country! Choose from: " + string.Join(", ", ChatterLocalization.countriesWithSususterja) + ".");
            }
        }


    }

    [ChatCommand("give")]
    public static void GiveEmote(Chatter sender, ChatMessage chatMessage, string[] args) {
        if (!sender.IsActiveChatter) return;
        if (sender.EmotesUsed > Cfg.EmotesPerChatter.Value) {
            chatMessage.Reply("You can only give " + Cfg.EmotesPerChatter.Value + " emotes!");
            return;
        }

        sender.EmotesUsed++;

        UnityThreadInvoker.Invoke(() =>
            EmotePapers.GiveEmotePaper(args[0], chatMessage)

        );
    }
    
    public static string Capitalize(this string str) {
        return str[0].ToString().ToUpper() + str[1..].ToLower();
    }
    
}