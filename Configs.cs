using BepInEx.Configuration;

namespace EndlessMode;

public class Configs
{
	public static Configs Instance;
	
	// public bool ResetDifficultyAfterFinalBosses
	// {
	// 	get => m_ResetDifficultyAfterFinalBosses.Value;
	// 	set
	// 	{
	// 		m_ResetDifficultyAfterFinalBosses.Value = value;
	// 		Plugin.Instance.Config.Save();
	// 	}
	// }
	
	public bool ResetPeltPricesAfterFinalBosses
	{
		get => m_ResetPeltPricesAfterFinalBosses.Value;
		set
		{
			m_ResetPeltPricesAfterFinalBosses.Value = value;
			Plugin.Instance.Config.Save();
		}
	}
	
	/*
	 public static bool HardLimit
	 {
	 	get => m_HardLimit.Value;
	 	set
	 	{
	 		m_HardLimit.Value = value;
	 		Plugin.Instance.Config.Save();
	 	}
	 }
 
	
	 public static string LimitMode
	 {
	 	get => m_LimitMode.Value;
	 	set
	 	{
	 		m_LimitMode.Value = value;
	 		Plugin.Instance.Config.Save();
	 	}
	 }
	
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
	
	 public static int RegionHardLimit
	 {
	 	get => m_RegionHardLimit.Value;
	 	set
	 	{
	 		m_RegionHardLimit.Value = value;
	 		Plugin.Instance.Config.Save();
	 	}
	 }
 
	 public static int FinalBossHardLimit
	 {
	 	get => m_FinalBossHardLimit.Value;
	 	set
	 	{
	 		m_FinalBossHardLimit.Value = value;
	 		Plugin.Instance.Config.Save();
	 	}
	 }
   
	 private static ConfigEntry<bool> m_HardLimit = Bind("General", "Hard Limit?", false, "Should there be a Hard Limit on the run?");
	 private static ConfigEntry<string> m_LimitMode = Bind("General", "Limit Mode", "", "What Type of Limit should be on if HardLimit is enabled? [RegionHardLimit] [FinalBossHardLimit]");
	 private static ConfigEntry<int> m_RegionsBeforeFinalBoss = Bind("General", "Regions Before Final Boss", 3, "How many regions should you go through before a Final Boss?");
	 private static ConfigEntry<bool> m_RepeatBoss = Bind("General", "Repeat Boss?", false, "Should bosses have the ability to be Repeated? this means it can be Prospecter into Prospecter. (With different Regions utilized if RepeatRegions is disabled.) [Prospecter, Angler, Trapper, etc.]");
	 private static ConfigEntry<bool> m_RepeatRegion = Bind("General", "Repeat Region?", false, "Should Regions be Repeated? this means it can be Woodlands into Woodlands. (With different Bosses at the end if RepeatBosses is disabled.) [Woodlands, Wetlands, Alpine, etc.]");
	 private static ConfigEntry<int> m_RegionHardLimit = Bind("HardLimit", "Region Hard Limit", 99999999, "If LimitMode is set to [RegionHardLimit] how many regions should be the limit?");
	 private static ConfigEntry<int> m_FinalBossHardLimit = Bind("HardLimit", "Final Boss Hard Limit", 99999999, "If LimitMode is set to [FinalBossHardLimit] how many FinalBosses should be the limit?");

*/	
	//private ConfigEntry<bool> m_ResetDifficultyAfterFinalBosses = Bind("Difficulty", "Reset after Final Bosses", false, "If disabled then the difficulty continues getting harder after killing a final boss");
	private ConfigEntry<bool> m_ResetPeltPricesAfterFinalBosses = Bind("Pelts", "Reset Prices after Final Bosses", false, "If disabled then the prices for pelts resets back to cheapest after killing a final boss");
	
	private static ConfigEntry<T> Bind<T>(string section, string key, T defaultValue, string description)
	{
		return Plugin.Instance.Config.Bind(section, key, defaultValue, new ConfigDescription(description, null, Array.Empty<object>()));
	}
}
