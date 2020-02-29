using System.Reflection;
using BeatSaberMarkupLanguage.Settings;
using HarmonyLib;
using IPA;
using UnityEngine.SceneManagement;
using IPALogger = IPA.Logging.Logger;

namespace AdaptiveSFXRemover
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static string Name => "AdaptiveSFXRemover";

        internal static bool harmonyPatchesLoaded = false;
        internal static Harmony harmonyInstance = new Harmony("com.shadnix.BeatSaber.AdaptiveSFXRemover");

        internal static bool gameCoreJustLoaded = false;

        [Init]
        public void Init(IPALogger logger)
        {
            Logger.log = logger;
        }

        [OnStart]
        public void OnStart()
        {
            AddEvents();
        }

        [OnExit]
        public void OnExit()
        {
            RemoveEvents();
        }

        /// <summary>
        /// Called when the active scene is changed.
        /// </summary>
        /// <param name="prevScene">The scene you are transitioning from.</param>
        /// <param name="nextScene">The scene you are transitioning to.</param>
        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            if (nextScene.name == "MenuViewControllers" && prevScene.name == "EmptyTransition")
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

            if (scene.name == "GameCore")
            {
                gameCoreJustLoaded = true;
            }
        }

        private void AddEvents()
        {
            RemoveEvents();
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void RemoveEvents()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            SceneManager.sceneLoaded -= OnSceneLoaded;
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
