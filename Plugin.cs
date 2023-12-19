using System.Diagnostics;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
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

		public static List<Type> AllFinalBossTypes = null;
		public static List<Type> AllBossTypes = null;

		

		public static Plugin Instance;
		public static ManualLogSource Log;
		public static string PluginDirectory;

		private void Awake()
		{
			Instance = this;
			Log = Logger;
			Configs.Instance = new Configs();

			PluginDirectory = this.Info.Location.Replace("EndlessMode.dll", "");

			GetAllBossTypes();
			
			new Harmony(PluginGuid).PatchAll();

			Custom_AscensionStat.AddCustomStat(PluginGuid, "Final Bosses Killed", ()=>RunStats.TotalFinalBossesKilled);
			Custom_AscensionStat.AddCustomStat(PluginGuid, "Highest Floor", ()=>RunStats.CurrentFloor);

			Logger.LogInfo($"Loaded {PluginName}!");
		}
		
		private static void GetAllBossTypes()
		{
			AllBossTypes = new List<Type>();
			AllBossTypes.Add(typeof(Part1BossOpponent));
			
			AllFinalBossTypes = new List<Type>();
			
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (Type type in assembly.GetTypes())
				{
					if (!type.IsSubclassOf(typeof(Part1BossOpponent))) 
						continue;
					
					//Plugin.Log.LogInfo($"Found method boss opponent type {type}");
					AllBossTypes.Add(type);

					// Get LifeLostSequence method
					MethodInfo method = type.GetMethod("LifeLostSequence",
						BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public |
						BindingFlags.NonPublic);
					if (method != null)
					{
						//Plugin.Log.LogInfo($" - Has {method} method!");
						AllFinalBossTypes.Add(type);
					}
				}
			}

			stopwatch.Stop();
			Plugin.Log.LogInfo($"Found {AllFinalBossTypes.Count}/{AllBossTypes.Count} final bosses in {stopwatch.ElapsedMilliseconds}ms");
		}
	}
}
