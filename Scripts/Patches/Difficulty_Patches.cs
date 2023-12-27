using System.Reflection;
using System.Reflection.Emit;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Pelts;

namespace EndlessMode
{
    [HarmonyPatch]
    public static class Difficulty_Patches
    {
        [HarmonyPostfix, HarmonyPatch(typeof(Opponent), nameof(Opponent.CreateCard))]
        public static void Opponent_CreateCard(CardInfo cardInfo, Opponent __instance, ref PlayableCard __result)
        {
            Endless.AdjustDifficulty(__result, __instance);
        }
    }
}