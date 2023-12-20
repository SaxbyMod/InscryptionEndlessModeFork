using HarmonyLib;

namespace EndlessMode
{
    [HarmonyPatch]
    public static class Encounter_Patches
    {
        // [HarmonyPostfix, HarmonyPatch(typeof(MapGenerator), nameof(MapGenerator.CreateNode))]
        // public static void MapGenerator_CreateNode_Postfix(ref NodeData __result)
        // {
        //     if (__result is CardBattleNodeData cardBattleNodeData)
        //     {
        //         Plugin.Log.LogInfo($"cardBattleNodeData.difficulty {cardBattleNodeData.difficulty}");
        //         cardBattleNodeData.difficulty = Mathf.Clamp(cardBattleNodeData.difficulty, 0, 15);
        //     }
        // }
        //
        // [HarmonyPostfix,
        //  HarmonyPatch(typeof(EncounterBlueprintData.CardBlueprint), nameof(EncounterBlueprintData.CardBlueprint.DifficultyRange), MethodType.Getter)]
        // public static void AscensionSaveData_NewRun(ref Vector2 __result)
        // {
        //     __result.y = Mathf.Clamp(__result.y, 0, 15);
        // }
    }
}