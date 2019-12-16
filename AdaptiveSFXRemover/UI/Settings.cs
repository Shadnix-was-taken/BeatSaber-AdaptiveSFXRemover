using BeatSaberMarkupLanguage.Attributes;
using BS_Utils.Utilities;

namespace AdaptiveSFXRemover.UI
{
    class Settings : PersistentSingleton<Settings>
    {
        private Config config;

        [UIValue("boolEnable")]
        public bool _isModDisabled
        {
            get => config.GetBool("AdaptiveSFXRemover", nameof(_isModDisabled), false, true);
            set => config.SetBool("AdaptiveSFXRemover", nameof(_isModDisabled), value);
        }

        public void Awake()
        {
            config = new Config("AdaptiveSFXRemover");
        }
    }
}
