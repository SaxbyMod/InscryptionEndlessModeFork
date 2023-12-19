using BepInEx.Configuration;

namespace EndlessMode;

public class Configs {
    public static Configs Instance;

    public enum HardLimitType {
        Infinite,
        RegionHardLimit,
        FinalBossHardLimit,
    }

    /* public static bool HardLimit
	 {
	 	get => m_HardLimit.Value;
	 	set
	 	{
	 		m_HardLimit.Value = value;
	 		Plugin.Instance.Config.Save();
	 	}
	 }
 
	*/
    public static HardLimitType LimitMode {
        get => Instance.m_LimitMode.Value;
        set {
            Instance.m_LimitMode.Value = value;
            Plugin.Instance.Config.Save();
        }
    }
    /*
	 public static int RegionsBeforeFinalBoss
	 {
	 	get => m_RegionsBeforeFinalBoss.Value;
	 	set
	 	{
	 		m_RegionsBeforeFinalBoss.Value = value;
	 		Plugin.Instance.Config.Save();
	 	}
	 }
	
	 public static bool RepeatBoss
	 {
	 	get => m_RepeatBoss.Value;
	 	set
	 	{
	 		m_RepeatBoss.Value = value;
	 		Plugin.Instance.Config.Save();
	 	}
	 }
 
	
	 public static bool RepeatRegion
	 {
	 	get => m_RepeatRegion.Value;
	 	set
	 	{
	 		m_RepeatRegion.Value = value;
	 		Plugin.Instance.Config.Save();
	 	}
	 }
	*/
    public static int RegionHardLimit {
        get => Instance.m_RegionHardLimit.Value;
        set {
            Instance.m_RegionHardLimit.Value = value;
            Plugin.Instance.Config.Save();
        }
    }

    public static int FinalBossHardLimit {
        get => Instance.m_FinalBossHardLimit.Value;
        set {
            Instance.m_FinalBossHardLimit.Value = value;
            Plugin.Instance.Config.Save();
        }
    }
    /*
	 private static ConfigEntry<bool> m_HardLimit = Bind("General", "Hard Limit?", false, "Should there be a Hard Limit on the run?");
  */
    private ConfigEntry < HardLimitType > m_LimitMode = Bind("General", "Limit Mode", HardLimitType.Infinite, "What Type of Limit should be on if HardLimit is enabled? [RegionHardLimit] [FinalBossHardLimit]");
    /*
	 private static ConfigEntry<int> m_RegionsBeforeFinalBoss = Bind("General", "Regions Before Final Boss", 3, "How many regions should you go through before a Final Boss?");
	 private static ConfigEntry<bool> m_RepeatBoss = Bind("General", "Repeat Boss?", false, "Should bosses have the ability to be Repeated? this means it can be Prospecter into Prospecter. (With different Regions utilized if RepeatRegions is disabled.) [Prospecter, Angler, Trapper, etc.]");
	 private static ConfigEntry<bool> m_RepeatRegion = Bind("General", "Repeat Region?", false, "Should Regions be Repeated? this means it can be Woodlands into Woodlands. (With different Bosses at the end if RepeatBosses is disabled.) [Woodlands, Wetlands, Alpine, etc.]");
*/
    private ConfigEntry < int > m_RegionHardLimit = Bind("HardLimit", "Region Hard Limit", 3, "If LimitMode is set to [RegionHardLimit] how many regions should be the limit?");
    private ConfigEntry < int > m_FinalBossHardLimit = Bind("HardLimit", "Final Boss Hard Limit", 1, "If LimitMode is set to [FinalBossHardLimit] how many FinalBosses should be the limit?");

    private static ConfigEntry < T > Bind < T > (string section, string key, T defaultValue, string description) {
        return Plugin.Instance.Config.Bind(section, key, defaultValue, new ConfigDescription(description, null, Array.Empty < object > ()));
    }
}
