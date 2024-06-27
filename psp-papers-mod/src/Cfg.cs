using BepInEx.Configuration;

namespace psp_papers_mod;

public static class Cfg {

    internal static ConfigEntry<string> Channel { get; private set; }

    internal static ConfigEntry<string> BotName { get; private set; }
    internal static ConfigEntry<string> AccessToken { get; private set; }
    internal static ConfigEntry<string> ClientID { get; private set; }
    internal static ConfigEntry<bool> UseLocalAuthServer { get; private set; }

    internal static ConfigEntry<bool> DisableSelectingApproved { get; private set; }
    internal static ConfigEntry<bool> AlwaysPreventSameActiveChatter { get; private set; }
    internal static ConfigEntry<bool> AlwaysPreventBannedWhileTalking { get; private set; }

    internal static ConfigEntry<double> VIPWeightMultiplier { get; private set; }
    internal static ConfigEntry<double> ModeratorWeightMultiplier { get; private set; }
    internal static ConfigEntry<double> SubscriberWeightMultiplier { get; private set; }
    internal static ConfigEntry<double> TwitchStaffWeightMultiplier { get; private set; }

    internal static ConfigEntry<double> AttacksPreviouslyDeniedMultiplier { get; private set; }
    internal static ConfigEntry<bool> AttacksDisableSelectingApproved { get; private set; }
    internal static ConfigEntry<bool> DeathsNeverSelectedAfter { get; private set; }

    internal static ConfigEntry<int> MinMsDelayBeforeAttack { get; private set; }
    internal static ConfigEntry<int> MaxMsDelayBeforeAttack { get; private set; }
    internal static ConfigEntry<int> MinTravelersBeforeAttack { get; private set; }
    internal static ConfigEntry<double> AttackChance { get; private set; }
    internal static ConfigEntry<double> IncreaseChancePerTravelerSinceLastAttack { get; private set; }
    internal static ConfigEntry<double> AttackChanceModifierConsecutiveDenial { get; private set; }
    
    internal static ConfigEntry<int> DenyTimeoutSeconds { get; private set; }
    internal static ConfigEntry<int> ShotTimeoutSeconds { get; private set; }
    internal static ConfigEntry<int> DetainedTimeoutSeconds { get; private set; }
    internal static ConfigEntry<int> TimeoutDelayMs { get; private set; }
    
    public static void StartBindings(PapersPSP mod) {
        Channel = mod.Config.Bind(
            "Twitch",
            "Channel",
            "psp1g",
            "The chat channel to integrate with"
        );

        BotName = mod.Config.Bind(
            "Twitch.Bot",
            "Username",
            "",
            "Username of the bot to sign into"
        );

        AccessToken = mod.Config.Bind(
            "Twitch.Bot",
            "AccessToken",
            "",
            "Twitch bot authentication token"
        );

        ClientID = mod.Config.Bind(
            "Twitch.Bot",
            "ClientID",
            "",
            "Twitch API client ID"
        );
        
        UseLocalAuthServer = mod.Config.Bind(
            "Twitch.Bot",
            "UseLocalAuthServer",
            false,
            "Only use this if you know what you are doing. Requests a twitch auth token from a local auth server. Requires botUsername"
        );

        AlwaysPreventSameActiveChatter = mod.Config.Bind(
            "Twitch.ChatterSelection",
            "AlwaysPreventSameActiveChatter",
            false,
            "A recent active chatter has less chance of being an active chatter again, and has no chance to be the active chatter twice in a row. Set this to true if you never want the same active chatter twice"
        );

        AlwaysPreventBannedWhileTalking = mod.Config.Bind(
            "Twitch.ChatterSelection",
            "AlwaysPreventBannedWhileTalking",
            true,
            "If a chatter gets banned (other than by this mod, ie for saying something bad) setting to `true` will ensure they will never be selected"
        );

        DisableSelectingApproved = mod.Config.Bind(
            "Twitch.ChatterSelection",
            "DisableSelectingApproved",
            true,
            "Disable selecting chatters who have already been approved to go through the border to be the active chatter again"
        );

        VIPWeightMultiplier = mod.Config.Bind(
            "Twitch.ChatterSelection",
            "VIPWeightMultiplier",
            1.0d,
            "A VIP's weight (chance) to be selected as an active chatter is multiplied by this number (0.5 = half of normal chance, 2 = double chance)"
        );

        ModeratorWeightMultiplier = mod.Config.Bind(
            "Twitch.ChatterSelection",
            "ModeratorWeightMultiplier",
            1.0d,
            "A moderator's weight (chance) to be selected as an active chatter is multiplied by this number (0.5 = half of normal chance, 2 = double chance)"
        );

        SubscriberWeightMultiplier = mod.Config.Bind(
            "Twitch.ChatterSelection",
            "SubscriberWeightMultiplier",
            1.0d,
            "A subscriber's weight (chance) to be selected as an active chatter is multiplied by this number (0.5 = half of normal chance, 2 = double chance)"
        );

        TwitchStaffWeightMultiplier = mod.Config.Bind(
            "Twitch.ChatterSelection",
            "StaffWeightMultiplier",
            1.0d,
            "A Twitch Staff's weight (chance) to be selected as an active chatter is multiplied by this number (0.5 = half of normal chance, 2 = double chance)"
        );

        AttacksPreviouslyDeniedMultiplier = mod.Config.Bind(
            "Twitch.ChatterSelection.Attacks",
            "PreviouslyDeniedMultiplier",
            2.0d,
            "A person's weight (chance), if they were previously denied at the border, to be selected as an attacker is multiplied by this number (0.5 = half of normal chance, 2 = double chance)"
        );

        AttacksDisableSelectingApproved = mod.Config.Bind(
            "Twitch.ChatterSelection.Attacks",
            "DisableSelectingApproved",
            true,
            "Disable selecting chatters who have already been approved to go through the border for attacks"
        );

        DeathsNeverSelectedAfter = mod.Config.Bind(
            "Twitch.ChatterSelection.Attacks",
            "DeathsNeverSelectedAfter",
            true,
            "Disable selecting chatters for ANYTHING (active chatting and attacks) if they have provoked an attack OR shot for any reason until a new game starts."
        );

        MinMsDelayBeforeAttack = mod.Config.Bind(
            "Twitch.ChatterSelection.Attacks",
            "MinDelayBeforeAttack",
            1500,
            "Minimum delay in milliseconds to send an attacker"
        );

        MaxMsDelayBeforeAttack = mod.Config.Bind(
            "Twitch.ChatterSelection.Attacks",
            "MaxDelayBeforeAttack",
            8000,
            "Maximum delay in milliseconds to send an attacker"
        );

        MinTravelersBeforeAttack = mod.Config.Bind(
            "Twitch.ChatterSelection.Attacks",
            "MinTravelersBeforeAttack",
            3,
            "The amount of travelers that must pass in the booth before attacks can start happening"
        );
        
        AttackChance = mod.Config.Bind(
            "Twitch.ChatterSelection.Attacks.Chance",
            "AttackChance",
            5d,
            "Each time a new traveler appears, this is the % chance that there will be an attack"
        );
        
        IncreaseChancePerTravelerSinceLastAttack = mod.Config.Bind(
            "Twitch.ChatterSelection.Attacks.Chance",
            "IncreaseChancePerTravelerSinceLastAttack",
            1d,
            "Every time a traveler passes since last attack, the AttackChance is increased by this amount. total% = AttackChance + this * travelersSinceLastAttack"
        );

        AttackChanceModifierConsecutiveDenial = mod.Config.Bind(
            "Twitch.ChatterSelection.Attacks.Chance",
            "ModifierConsecutiveDenial",
            1.2d,
            "Every time a traveler is denied, the AttackChance is multiplied by this amount. An attack resets consecutive denials. total% = IncreasedAttackChance * this ^ consecutiveDenials"
        );
        
        DenyTimeoutSeconds = mod.Config.Bind(
            "Twitch.ChatterSelection.Timeouts",
            "DenyTimeoutSeconds",
            120,
            "Time in seconds that a chatter should be timed out for being denied"
        );
        
        DetainedTimeoutSeconds = mod.Config.Bind(
            "Twitch.ChatterSelection.Timeouts",
            "DetainedTimeoutSeconds",
            300,
            "Time in seconds that a chatter should be timed out for being detained"
        );
        
        ShotTimeoutSeconds = mod.Config.Bind(
            "Twitch.ChatterSelection.Timeouts",
            "ShotTimeoutSeconds",
            600,
            "Time in seconds that a chatter should be timed out for being shot"
        );
        
        TimeoutDelayMs = mod.Config.Bind(
            "Twitch.ChatterSelection.Timeouts",
            "TimeoutDelayMilliseconds",
            1000,
            "Time in milliseconds that the bot should wait before timing out a user for any reason (compensate for stream delay)"
        );
    }

}