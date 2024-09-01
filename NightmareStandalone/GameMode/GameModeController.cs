using NightmareStandalone.GameMode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightmareStandalone.GameMode
{

    public static class GameModeController
    {

        private static GameMode selectedGameMode = null;

        public static GameMode GetSelectedGameMode()
        {
            return selectedGameMode;
        }


        public static void SetGameMode(GameMode gameMode)
        {
            if (GetSelectedGameMode() != null)
            {
                Triggers.TriggerController.Instance.Trigger(Triggers.Trigger.MODE_DESELECTED, GetSelectedGameMode());
            }
            try
            {
                if (selectedGameMode != null && selectedGameMode.specialOptions.nightmare != gameMode.specialOptions.nightmare) NightmareBehaviour.EnableNightmare(gameMode.specialOptions.nightmare);
            }
            catch (Exception e)
            {
                Logger.log.Debug("Error loading Nightmare mode!");
                Logger.log.Debug(e);
            }
            selectedGameMode = gameMode;
            Triggers.TriggerController.Instance.Trigger(Triggers.Trigger.MODE_SELECTED, GetSelectedGameMode());
        }


    }

}
