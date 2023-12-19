using System.Collections;
using EndlessMode;
using DiskCardGame;

public static class Endless
{
    public static IEnumerator FinalBossKilled(Part1BossOpponent opponent)
    {
        Plugin.Log.LogInfo($"Killed Final Boss: {opponent.OpponentType}");
        RunStats.FinalBossKilled(opponent);
        yield return opponent.BossDefeatedSequence(); // Give back flames and record boss kill
    }

    public static void SetupNewRun()
    {
        RunStats.SetupNewRun();
    }
    
    public static void SetupNextRun()
    {
        RunStats.SetupNextRun();
        
        RunState.Run.regionTier = 0;
        AscensionSaveData.Data.RollCurrentRunRegionOrder();        
        RunState.Run.GenerateMapDataForCurrentRegion();
    }

    public static void NextRegionStarted()
    {
        RunStats.NewFloorStarted();
    }

    public static void BossKilled(Part1BossOpponent opponent)
    {
        RunStats.BossKilled(opponent);
    }

    public static bool EndlessRunFinished()
    {
        switch (Configs.LimitMode)
        {
            case Configs.HardLimitType.RegionHardLimit:
                Plugin.Log.LogInfo($"EndlessRunFinished " + Configs.LimitMode + " " + RunStats.CurrentFloor + " >= " + Configs.RegionHardLimit);
                if (RunStats.CurrentFloor >= Configs.RegionHardLimit)
                {
                    return true;
                }
                break;
            case Configs.HardLimitType.FinalBossHardLimit:
                Plugin.Log.LogInfo($"EndlessRunFinished " + Configs.LimitMode + " " + RunStats.TotalFinalBossesKilled + " >= " + Configs.FinalBossHardLimit);
                if (RunStats.TotalFinalBossesKilled >= Configs.FinalBossHardLimit)
                {
                    return true;
                }
                break;
            default:
                Plugin.Log.LogWarning($"EndlessRunFinished unhandled limit mode: " + Configs.LimitMode);
                break;
        }

        return false;
    }
}