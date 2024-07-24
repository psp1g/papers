using psp_papers_mod.Patches;

namespace psp_papers_mod;

public static class AttackHandler {

    internal static int ConsecutiveDenials;
    internal static int TravelerCtSinceLast;

    internal static int ChatterWantAttackCt = 0;

    internal static bool ShouldAttackNow() {
        if (
            TravelerCtSinceLast <= 1 ||
            TravelerNamePatch.TotalTravellers < Cfg.MinTravelersBeforeAttack.Value
        )
            return false;

        double chance = Cfg.AttackChance.Value;

        if (TravelerCtSinceLast > 0 && Cfg.IncreaseChancePerTravelerSinceLastAttack.Value > 0) 
            chance += Cfg.IncreaseChancePerTravelerSinceLastAttack.Value * TravelerCtSinceLast;
        if (ConsecutiveDenials > 0 && Cfg.AttackChanceModifierConsecutiveDenial.Value > 1)
            chance *= Math.pow(Cfg.AttackChanceModifierConsecutiveDenial.Value, ConsecutiveDenials);
        if (ChatterWantAttackCt > 0 && Cfg.WantAttackChanceModifier.Value > 1)
            chance *= Math.pow(Cfg.WantAttackChanceModifier.Value, ChatterWantAttackCt);

        double rand = PapersPSP.Random.Next(100);
        return rand < chance;
    }

    internal static async void Attack() {
        ConsecutiveDenials = 0;
        TravelerCtSinceLast = 0;
        ChatterWantAttackCt = 0;

        // todo; Fix delays triggering end of game (?)
        /*if (Cfg.MaxMsDelayBeforeAttack.Value > 0) {
            int randDelay = PapersPSP.Random.Next(Cfg.MaxMsDelayBeforeAttack.Value - Cfg.MinMsDelayBeforeAttack.Value);
            randDelay += Cfg.MinMsDelayBeforeAttack.Value;
            await Task.Delay(randDelay);
        }*/

        BorderPatch.SendChatterRunner();
    }

    internal static bool AttackIfPossible() {
        bool should = ShouldAttackNow();
        if (should) Attack();
        return should;
    }
    
}