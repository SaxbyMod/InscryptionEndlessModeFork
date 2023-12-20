using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace EndlessMode
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	public class Plugin : BaseUnityPlugin
	{
		public const string PluginGuid = "jamesgames.inscryption.endlessmode";
		public const string PluginName = "Endless Mode";
		public const string PluginVersion = "0.1.0";

		public static Plugin Instance;
		public static ManualLogSource Log;
		public static string PluginDirectory;

		private void Awake()
		{
			Instance = this;
			Log = Logger;
			Configs.Instance = new Configs();

			PluginDirectory = this.Info.Location.Replace("EndlessMode.dll", "");

			new Harmony(PluginGuid).PatchAll();

			Custom_AscensionStat.AddCustomStat(PluginGuid, "Final Bosses Killed", ()=>RunStats.TotalFinalBossesKilled);
			Custom_AscensionStat.AddCustomStat(PluginGuid, "Highest Floor", ()=>RunStats.CurrentFloor);

			Logger.LogInfo($"Loaded {PluginName}!");
		}
	}
}
