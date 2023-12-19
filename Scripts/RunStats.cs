using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Saves;
using UnityEngine;

namespace EndlessMode
{
    [HarmonyPatch]
    public static class RunStats
    {
        public static IReadOnlyDictionary<Opponent.Type, int> BossKillLookup => m_bossKillLookup;
        public static int TotalFinalBossesKilled => m_totalFinalBossesKilled;
        public static int CurrentFloor => m_currentFloor;
        public static int TotalResets => m_totalResets;

        private static int m_currentFloor = 0;
        private static int m_totalResets = 0;
        private static int m_totalFinalBossesKilled = 0;
        private static Dictionary<Opponent.Type, int> m_bossKillLookup = new Dictionary<Opponent.Type, int>();

        public static void SetupNewRun()
        {
            m_totalResets = 0;
            m_currentFloor = 1;
            m_totalFinalBossesKilled = 0;
            m_bossKillLookup = new Dictionary<Opponent.Type, int>();

            Save();
        }

        public static void FinalBossKilled(Part1BossOpponent opponent)
        {
            m_totalFinalBossesKilled++;
            if (!m_bossKillLookup.ContainsKey(opponent.OpponentType))
            {
                m_bossKillLookup.Add(opponent.OpponentType, 0);
            }

            m_bossKillLookup[opponent.OpponentType]++;

            Save();
        }

        public static void BossKilled(Part1BossOpponent opponent)
        {
            if (!m_bossKillLookup.ContainsKey(opponent.OpponentType))
            {
                m_bossKillLookup.Add(opponent.OpponentType, 0);
            }

            m_bossKillLookup[opponent.OpponentType]++;

            Save();
        }

        public static void SetupNextRun()
        {
            m_totalResets++;
            m_currentFloor++;

            Save();
        }

        public static void NewFloorStarted()
        {
            m_currentFloor++;

            Save();
        }

        private static void Save()
        {
            ModdedSaveManager.RunState.SetValue(Plugin.PluginGuid, "TotalFinalBossesKilled", m_totalFinalBossesKilled);
            ModdedSaveManager.RunState.SetValue(Plugin.PluginGuid, "TotalResets", m_totalResets);
            ModdedSaveManager.RunState.SetValue(Plugin.PluginGuid, "CurrentFloor", m_currentFloor);

            string json = JsonUtility.ToJson(new SerializedDictionary(m_bossKillLookup));
            ModdedSaveManager.RunState.SetValue(Plugin.PluginGuid, "BossKillLookup", json);
        }

        [HarmonyPostfix, HarmonyPatch(typeof(ModdedSaveManager), "ReadDataFromFile", new Type[] { })]
        public static void ModdedSaveManager_ReadDataFromFile()
        {
            m_totalFinalBossesKilled =
                ModdedSaveManager.RunState.GetValueAsInt(Plugin.PluginGuid, "TotalFinalBossesKilled");
            m_totalResets = ModdedSaveManager.RunState.GetValueAsInt(Plugin.PluginGuid, "TotalResets");
            m_currentFloor = ModdedSaveManager.RunState.GetValueAsInt(Plugin.PluginGuid, "CurrentFloor");
            m_bossKillLookup = new Dictionary<Opponent.Type, int>();

            string json = ModdedSaveManager.RunState.GetValue(Plugin.PluginGuid, "BossKillLookup");
            if (!string.IsNullOrEmpty(json))
            {
                SerializedDictionary serializedDictionary = JsonUtility.FromJson<SerializedDictionary>(json);
                serializedDictionary.LoadIntoDictionary(m_bossKillLookup);
            }
        }
    }
}