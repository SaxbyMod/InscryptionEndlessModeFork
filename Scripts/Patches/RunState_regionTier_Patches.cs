using System.Reflection;
using System.Reflection.Emit;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Pelts;

namespace EndlessMode
{
    [HarmonyPatch]
    public static class RunState_regionTier_Patches
    {
        public static IEnumerable<MethodInfo> TargetMethods()
        {
            BindingFlags allFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;
            
            // Fixes
            yield return typeof(AscensionSaveData).GetMethod("EndRun", allFlags);
            yield return typeof(MapGenerator).GetMethod("ForceFirstNodeTraderForAscension", allFlags);
            
            // Seeds
            yield return typeof(SaveFile).GetMethod("GetCurrentRandomSeed", allFlags);
            yield return typeof(RunState).GetProperty("RandomSeed", allFlags).GetMethod;
            
            // Difficulty
            // if (!Configs.Instance.ResetDifficultyAfterFinalBosses)
            // {
            //     yield return typeof(MapGenerator).GetMethod("CreateNode", allFlags);
            // }
            
            // Pelt Prices
            if (!Configs.Instance.ResetDifficultyAfterFinalBosses)
            {
                yield return typeof(PeltManager).GetProperty("BasePeltPrices", allFlags).GetMethod;
            }
        }
        
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            // === We want to turn this

            // RunState.Run.regionTier (ldfld int32 DiskCardGame.RunState::regionTier)
            // RunState.CurrentRegionTier (call instance int32 DiskCardGame.RunState::get_CurrentRegionTier())

            // === Into this

            // GetRegionTier(RunState.Run)
            // GetRegionTier2()

            // ===
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static;
            
            // RunState.CurrentRegionTier => GetRegionTier2()
            MethodInfo CurrentRegionTier = typeof(RunState).GetProperty("CurrentRegionTier", flags).GetMethod;
            MethodInfo GetRegionTier2 = typeof(RunState_regionTier_Patches).GetMethod("GetRegionTier2", flags);
            
            // RunState.Run.regionTier => GetRegionTier(RunState.Run)
            FieldInfo regionTier = typeof(RunState).GetField("regionTier", flags);
            MethodInfo GetRegionTier = typeof(RunState_regionTier_Patches).GetMethod("GetRegionTier", flags);
        
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                CodeInstruction codeInstruction = codes[i];
                if (codeInstruction.opcode == OpCodes.Ldfld && codeInstruction.operand == regionTier)
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, GetRegionTier);
                }
                else if (codeInstruction.opcode == OpCodes.Call && codeInstruction.operand == CurrentRegionTier)
                {
                    codes[i] = new CodeInstruction(OpCodes.Call, GetRegionTier2);
                }
            }
        
            return codes;
        }

        public static int GetRegionTier(RunState runState)
        {
            return RunStats.CurrentFloor;
        }

        public static int GetRegionTier2()
        {
            return RunStats.CurrentFloor;
        }
    }
}