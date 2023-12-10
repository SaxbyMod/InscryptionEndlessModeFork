using System.Collections;
using EndlessMode;
using DiskCardGame;

public static class Endless
{
    public static int FinalBossesKilled = 0;
    public static Dictionary<Opponent.Type, int> BossesKilled = new Dictionary<Opponent.Type, int>();
    
    public static IEnumerator FinalBossKilled(Part1BossOpponent opponent)
    {
        yield return opponent.BossDefeatedSequence(); // Give back flames and record boss kill
    }

    public static void CreateNewRun()
    {
        FinalBossesKilled++;
        Plugin.Log.LogInfo($"Final boss killed " + FinalBossesKilled + " times.");
        foreach (KeyValuePair<Opponent.Type,int> pair in BossesKilled)
        {
            Plugin.Log.LogInfo($"Killed {pair.Key} {pair.Value} times.");
        }
        
        RunState.Run.regionTier = 0;
        AscensionSaveData.Data.RollCurrentRunRegionOrder();        
        RunState.Run.GenerateMapDataForCurrentRegion();
    }

    public static void BossKilled(Opponent.Type instanceOpponentType)
    {
        if (!BossesKilled.ContainsKey(instanceOpponentType))
        {
            BossesKilled.Add(instanceOpponentType, 0);
        }
        BossesKilled[instanceOpponentType]++;
        Plugin.Log.LogInfo($"Killed {instanceOpponentType} {BossesKilled[instanceOpponentType]} times.");
    }
}