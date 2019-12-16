using System;
using System.Reflection;
using BeatSaberMarkupLanguage.Settings;
using Harmony;
using IPA;
using IPA.Config;
using IPA.Utilities;
using UnityEngine.SceneManagement;
using UnityEngine;
using IPALogger = IPA.Logging.Logger;

namespace AdaptiveSFXRemover
{
    public class Plugin : IBeatSaberPlugin
    {
        internal static string Name => "AdaptiveSFXRemover";

        internal static bool harmonyPatchesLoaded = false;
        internal static HarmonyInstance harmonyInstance = HarmonyInstance.Create("com.shadnix.BeatSaber.AdaptiveSFXRemover");

        public void Init(IPALogger logger)
        {
            Logger.log = logger;
        }

        public void OnApplicationStart()
        {

        }

        public void OnApplicationQuit()
        {

        }

        /// <summary>
        /// Runs at a fixed intervalue, generally used for physics calculations. 
        /// </summary>
        public void OnFixedUpdate()
        {

        }

        /// <summary>
        /// This is called every frame.
        /// </summary>
        public void OnUpdate()
        {

        }

        /// <summary>
        /// Called when the active scene is changed.
        /// </summary>
        /// <param name="prevScene">The scene you are transitioning from.</param>
        /// <param name="nextScene">The scene you are transitioning to.</param>
        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == "MenuViewControllers")
            {
                BSMLSettings.instance.AddSettingsMenu("Adaptive SFX", "AdaptiveSFXRemover.UI.settings.bsml", UI.Settings.instance);
            }
        }

        /// <summary>
        /// Called when the a scene's assets are loaded.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="sceneMode"></param>
        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            // Check for scene MenuCore and GameCore, MenuCore for initializing on start, GameCore for changes to config
            if (scene.name == "MenuCore" || scene.name == "GameCore")
            {
                if (!harmonyPatchesLoaded && !UI.Settings.instance._isModDisabled)
                {
                    Logger.log.Info("Loading Harmony patches...");
                    LoadHarmonyPatch();
                }
                if (harmonyPatchesLoaded && UI.Settings.instance._isModDisabled)
                {
                    Logger.log.Info("Unloading Harmony patches...");
                    UnloadHarmonyPatch();
                }
            }
        }

        public void OnSceneUnloaded(Scene scene)
        {

        }

        internal void LoadHarmonyPatch()
        {
            if (!harmonyPatchesLoaded)
            {
                harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
                Logger.log.Info("Loaded Harmony patches.");
            }
            harmonyPatchesLoaded = true;
        }

        internal void UnloadHarmonyPatch()
        {
            if (harmonyPatchesLoaded)
            {
                harmonyInstance.UnpatchAll("com.shadnix.BeatSaber.AdaptiveSFXRemover");
                Logger.log.Info("Unloaded Harmony patches.");
            }
            harmonyPatchesLoaded = false;
        }
    }
}
