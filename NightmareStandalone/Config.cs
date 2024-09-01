using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace NightmareStandalone
{

    public static class Config
    {
        public static BS_Utils.Utilities.Config ModPrefs = new BS_Utils.Utilities.Config("NightmareStandalone");
        public static bool enabled = false;
        public static Color MonoChromeColor = new Color(0.7f, 0.7f, 0.7f);
        public static void Read()
        {
       //     enabled = ModPrefs.GetBool("NightmareStandalone", "Enabled", false, true);
        }
        public static void Write()
        {

          //  ModPrefs.SetBool("NightmareStandalone", "Enabled", enabled);
        }
    }
}
