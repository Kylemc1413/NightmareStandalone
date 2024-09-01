using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.Attributes;
namespace NightmareStandalone.UI
{
    public class ModifierUI : NotifiableSingleton<ModifierUI>
    {

        [UIValue("enabled")]
        public bool modEnabled
        {
            get => Config.enabled;
            set
            {
                NightmareBehaviour.EnableNightmare(value);
                Config.enabled = value;
                Config.Write();
            }
        }
        [UIAction("setEnabled")]
        void SetEnabled(bool value)
        {
            modEnabled = value;
        }

    }
}
