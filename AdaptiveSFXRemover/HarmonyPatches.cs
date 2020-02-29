using IPA.Utilities;
using HarmonyLib;

namespace AdaptiveSFXRemover.HarmonyPatches
{
    [HarmonyPatch(typeof(AutomaticSFXVolume))]
    [HarmonyPatch("Update")]
    class AutomaticSFXVolumeUpdatePatch
    {
        static bool Prefix(AutomaticSFXVolume __instance)
        {
            if (Plugin.gameCoreJustLoaded)
            {
                Plugin.gameCoreJustLoaded = false;
                //AudioManagerSO audioManager = ReflectionUtil.GetPrivateField<AudioManagerSO>(__instance, "_audioManager");
                AudioManagerSO audioManager = __instance.GetField<AudioManagerSO, AutomaticSFXVolume>("_audioManager");
                //AutomaticSFXVolume.InitData initData = ReflectionUtil.GetPrivateField<AutomaticSFXVolume.InitData>(__instance, "_initData");
                AutomaticSFXVolume.InitData initData = __instance.GetField<AutomaticSFXVolume.InitData, AutomaticSFXVolume>("_initData");
                audioManager.sfxVolume = 1f + initData.volumeOffset;
            }
            return false;
        }
    }
}
