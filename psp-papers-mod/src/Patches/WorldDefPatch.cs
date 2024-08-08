using app;
using data;
using HarmonyLib;
using haxe.iterators;
using haxe.xml._Access;
using psp_papers_mod.Utils;
using System.Collections.Generic;

namespace psp_papers_mod.Patches;

[HarmonyPatch(typeof(WorldDef))]
public class WorldDefPatch {
    
    [HarmonyPrefix]
    [HarmonyPatch(nameof(WorldDef.getRandomAccessNations))]
    public static bool OverrideRandomAccessNations(Rand rand, bool includeArstotzka, int count, ref WorldDef __instance, ref Array __result) {
        __result = new Array();
        List<string> available = [];
        ArrayIterator nationIter = __instance.nationNodes.iterator();
        while (nationIter.hasNext()) {
            Xml node = nationIter.next().Cast<Xml>();
            available.Add(AttribAccess_Impl_.resolve(node, "name"));
        }
        available.Remove("Sususterja");
        for (int i = 0; i < count - 1; i++) {
            string nation = available[System.Math.Abs(rand.nextInt(available.Count))];
            available.Remove(nation);
            __result.push(nation.ToIl2Cpp());
        }

        if (includeArstotzka) {
            __result.insert(rand.nextInt(__result.length + 1), "Sususterja".ToIl2Cpp());
        } else {
            __result.push(available[System.Math.Abs(rand.nextInt(available.Count))].ToIl2Cpp());
        }

        return false;
    }
    
    
}