using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Object = System.Object;
using UnityEngine;

namespace NightmareStandalone.Triggers {

    public class TriggerController : MonoBehaviour {


        private static TriggerController _instance;

        public static TriggerController Instance {
            get {
                if (_instance == null) {
                    Logger.log.Debug("Initializing TriggerController...");
                    GameObject ob = new GameObject("TriggerController");
                    _instance = ob.AddComponent<TriggerController>();
                    DontDestroyOnLoad(ob);
                    _instance.Init();
                }
                return _instance;
            }
        }

        public void Init() {

        }

        public void SongSceneInit() {

        }

        public void Trigger(Trigger trigger, GameMode.GameMode gameMode, params Object[] values) {
            if (gameMode.triggerOptions.triggeredEvents == null) return;
            foreach (TriggeredEvent triggeredEvent in gameMode.triggerOptions.triggeredEvents) {
                if (triggeredEvent.CriteriaMet(trigger)) triggeredEvent.TriggerEvent(this, values);
            }
        }

    }

}
