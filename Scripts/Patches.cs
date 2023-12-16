using System.Collections;
using DiskCardGame;
using HarmonyLib;

namespace EndlessMode
{
    [HarmonyPatch]
    public static class Patches
    {
        [HarmonyPostfix,
         HarmonyPatch(typeof(AscensionSaveData), nameof(AscensionSaveData.NewRun),
             new Type[] { typeof(List<CardInfo>) })]
        public static void AscensionSaveData_NewRun(RunState __instance)
        {
            Endless.SetupNewRun();
        }

        [HarmonyPrefix, HarmonyPatch(typeof(RunState), nameof(RunState.NextRegion))]
        public static bool RunState_NextRegion(RunState __instance)
        {
            if (RunState.Run.regionTier == 3)
            {
                Endless.SetupNextRun();
                return false;
            }

            Endless.NextRegionStarted();
            return true;
        }

        [HarmonyPrefix, HarmonyPatch(typeof(Part1BossOpponent), nameof(Part1BossOpponent.BossDefeatedSequence))]
        public static bool Part1BossOpponent_BossDefeatedSequence(Part1BossOpponent __instance)
        {
            Endless.BossKilled(__instance);

            return true;
        }
    }
}