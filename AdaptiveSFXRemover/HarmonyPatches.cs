using Harmony;

namespace AdaptiveSFXRemover.HarmonyPatches
{
    [HarmonyPatch(typeof(AutomaticSFXVolume))]
    [HarmonyPatch("Update")]
    class AutomaticSFXVolumeUpdatePatch
    {
        static bool Prefix()
        {
            return false;
        }
    }
}
