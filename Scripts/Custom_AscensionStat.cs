using DiskCardGame;
using GBC;
using HarmonyLib;
using InscryptionAPI.Guid;
using UnityEngine;

namespace EndlessMode
{
    [HarmonyPatch]
    public static class Custom_AscensionStat
    {
        private class CustomStat
        {
            public string GUID;
            public string Name;
            public AscensionStat.Type Type;
            public Func<int> GetValue;
        }

        public static List<AscensionStat.Type> BaseStatTypes = GetBaseStats();
        public static List<AscensionStat.Type> CustomStatTypes = new List<AscensionStat.Type>();
        public static List<AscensionStat.Type> AllStatTypes = GetBaseStats();
        private static List<CustomStat> NewCustomStat = new List<CustomStat>();

        private static List<AscensionStat.Type> GetBaseStats()
        {
            var list = new List<AscensionStat.Type>();
            foreach (AscensionStat.Type type in Enum.GetValues(typeof(AscensionStat.Type)))
            {
                list.Add(type);
            }
            
            return list;
        }

        public static AscensionStat.Type AddCustomStat(string guid, string name, Func<int> GetValue)
        {
            CustomStat stat = new CustomStat();
            stat.Type = GuidManager.GetEnumValue<AscensionStat.Type>(guid, name);
            stat.GUID = guid;
            stat.Name = name;
            stat.GetValue = GetValue;

            NewCustomStat.Add(stat);
            CustomStatTypes.Add(stat.Type);
            return stat.Type;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AscensionStat), nameof(AscensionStat.GetStringForType), new Type[] { })]
        public static bool AscensionStat_GetStringForType(AscensionStat __instance, ref string __result)
        {
            if (CustomStatTypes.Contains(__instance.type))
            {
                foreach (CustomStat stat in NewCustomStat)
                {
                    if (stat.Type == __instance.type)
                    {
                        __result = stat.Name;
                        return false;
                    }
                }
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AscensionStatsScreen), nameof(AscensionStatsScreen.FillStatsText), new Type[] { })]
        public static bool AscensionStatsScreen_FillStatsText(AscensionStatsScreen __instance)
        {
            Plugin.Log.LogInfo($"AscensionStatsScreen_FillStatsText " + NewCustomStat.Count);
            AscensionMenuScreenTransition transition = __instance.GetComponent<AscensionMenuScreenTransition>();
            
            List<AscensionStat> list = new List<AscensionStat>(__instance.allTimeStats ? AscensionSaveData.Data.stats.allTimeStats : AscensionSaveData.Data.stats.currentRunStats);
            foreach (CustomStat customStat in NewCustomStat)
            {
                list.Add(new AscensionStat(customStat.Type, customStat.GetValue()));
                
                // Add displayedStatTypes
                __instance.displayedStatTypes.Add(customStat.Type);

                // Add StatsText
                PixelText template = __instance.statsText[__instance.statsText.Count - 1];
                Vector3 diff = __instance.statsText[1].transform.position - __instance.statsText[0].transform.position;
                
                PixelText clone = GameObject.Instantiate(template, template.transform.parent);
                __instance.statsText.Add(clone);
                clone.transform.position = template.transform.position + diff;
                
                transition.screenInteractables.Insert(transition.screenInteractables.Count - 2, clone.GetComponentInParent<AscensionMenuInteractable>());
                transition.onEnableRevealedObjects.Insert(1, clone.transform.parent.gameObject);
            }
            
            for (int i = 0; i < __instance.displayedStatTypes.Count && i < __instance.statsText.Count; i++)
            {
                AscensionStat ascensionStat = list.Find((AscensionStat x) => x.type == __instance.displayedStatTypes[i]);
                if (ascensionStat == null)
                {
                    ascensionStat = new AscensionStat(__instance.displayedStatTypes[i], 0);
                }
                int num = ascensionStat.value;
                if (__instance.allTimeStats)
                {
                    AscensionStat ascensionStat2 = AscensionSaveData.Data.stats.currentRunStats.Find((AscensionStat x) => x.type == __instance.displayedStatTypes[i]);
                    if (ascensionStat2 != null)
                    {
                        num += ascensionStat2.value;
                    }
                }
                string text = Localization.ToUpper(Localization.Translate(ascensionStat.GetStringForType())) + ":  " + num;
                __instance.statsText[i].SetText(text);
            }

            return false;
        }
    }
}