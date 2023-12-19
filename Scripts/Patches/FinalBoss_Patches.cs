using System.Collections;
using System.Diagnostics;
using System.Reflection;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EndlessMode
{
    [HarmonyPatch]
    public static class FinalBoss_Patches
    {
        [HarmonyPatch]
        public static class FinalBoss_LifeLostSequence
        {
            public static IEnumerable<MethodBase> TargetMethods()
            {
                // Find all subclasses of Part1BossOpponent that override LifeLostSequence and patch it so it doesn't end the game.
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                int count = 0;
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (type.IsSubclassOf(typeof(Part1BossOpponent)))
                        {
                            //Plugin.Log.LogInfo($"Found method boss opponent type {type}");

                            // Get LifeLostSequence method
                            MethodInfo method = type.GetMethod("LifeLostSequence",
                                BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public |
                                BindingFlags.NonPublic);
                            if (method != null)
                            {
                                //Plugin.Log.LogInfo($" - Has {method} method!");
                                yield return method;
                                count++;
                            }
                        }
                    }
                }

                stopwatch.Stop();
                Plugin.Log.LogInfo($"Found {count} methods in {stopwatch.ElapsedMilliseconds}ms");
            }

            public static IEnumerator Postfix(IEnumerator sequenceEvent, Part1BossOpponent __instance)
            {
                yield return sequenceEvent;
                if (__instance.NumLives == 0)
                {
                    Singleton<UIManager>.Instance.Effects.GetEffect<ScreenColorEffect>().SetColor(Color.black);
                    Singleton<UIManager>.Instance.Effects.GetEffect<ScreenColorEffect>().SetIntensity(0f, 3f);
                    yield return new WaitForSeconds(0.8f);
                    Singleton<InteractionCursor>.Instance.SetHidden(hidden: false);

                    CustomCoroutine.WaitThenExecute(1.25f, delegate { AudioController.Instance.StopAllLoops(); });

                    yield return Endless.FinalBossKilled(__instance);
                }
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Part1BossOpponent), nameof(Part1BossOpponent.TransitionFromFinalBoss), new Type[] { })]
        public static bool Part1BossOpponent_TransitionFromFinalBoss(Part1BossOpponent __instance)
        {
            // Only end the run if we actually finished the endless run
            Plugin.Log.LogInfo($"Part1BossOpponent_TransitionFromFinalBoss");
            return Endless.EndlessRunFinished();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(SceneManager), nameof(SceneManager.LoadScene), new Type[] { typeof(string) })]
        public static bool SceneManager_LoadScene(string sceneName)
        {
            if (sceneName == "Ascension_Credits")
            {
                // Don't show the credits scene... just continue the game as normal
                if (Environment.StackTrace.Contains("LifeLostSequence"))
                {
                    Plugin.Log.LogInfo($"Credits called from LifeLostSequence, skipping.");
                    return Endless.EndlessRunFinished();
                }

                Plugin.Log.LogInfo($"Credits NOT called from LifeLostSequence, skipping.");
            }

            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Part1BossOpponent), nameof(Part1BossOpponent.EndAscensionRun), new Type[] { })]
        public static bool Part1BossOpponent_EndAscensionRun(Part1BossOpponent __instance)
        {
            // Only end the run if we actually finished the endless run
            return Endless.EndlessRunFinished();
        }
    }
}