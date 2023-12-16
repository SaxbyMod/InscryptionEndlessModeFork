using System.Collections;
using DiskCardGame;
using HarmonyLib;

[HarmonyPatch]
public static class Patches
{
    [HarmonyPrefix, HarmonyPatch(typeof(RunState), nameof(RunState.NextRegion))]
    public static bool RunState_NextRegion(RunState __instance)
    {
        if (RunState.Run.regionTier == 3)
        {
            Endless.CreateNewRun();
            return false;
        }
        
        return true;
    }
    
    [HarmonyPrefix, HarmonyPatch(typeof(Part1BossOpponent), nameof(Part1BossOpponent.BossDefeatedSequence))]
    public static bool RunState_NextRegion(Part1BossOpponent __instance)
    {
        Endless.BossKilled(__instance.OpponentType);
        
        return true;
    }
}