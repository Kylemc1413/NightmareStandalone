using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Object = System.Object;
using UnityEngine;
using System.Collections;

namespace NightmareStandalone.Triggers {

    /*
    [XmlInclude(typeof())]
    */

    [XmlInclude(typeof(TriggeredEventRealtimeMirror))]
    public abstract class TriggeredEvent {

        [XmlElement("Triggers")]
        public Trigger[] triggers;
        [XmlElement("Cooldown")]
        public float cooldown = 0f;
        [XmlElement("Chance")]
        public float chance = 1f;
        [XmlElement("CooldownOnChanceFail")]
        public bool failCooldown = false;
        [XmlElement("Delay")]
        public float delay = 0f;

        private float lastTriggered = 0f;

        public TriggeredEvent() {

        }

        public TriggeredEvent(params Trigger[] triggers) {
            this.triggers = triggers;
        }

        public bool CriteriaMet(Trigger trigger) {
            for (int i = 0; i < triggers.Length; i++) {
                if (triggers[i] == trigger) {
                    if (cooldown > 0f) {
                        if (lastTriggered + cooldown > Time.time) return false;
                    }
                    if (chance < 1f) {
                        if (UnityEngine.Random.value >= chance) {
                            if (failCooldown) lastTriggered = Time.time;
                            return false;
                        }
                    }
                    lastTriggered = Time.time;
                    return true;
                }
            }
            return false;
        }

        public void TriggerEvent(TriggerController controller, params Object[] values) {
            if (delay > 0f) {
                controller.StartCoroutine(DelayedActivation(this, delay, controller, values));
            } else OnTriggerEvent(controller, values);
        }

        IEnumerator DelayedActivation(TriggeredEvent triggeredEvent, float delay, TriggerController controller, params Object[] values) {
            yield return new WaitForSeconds(delay);
            triggeredEvent.OnTriggerEvent(controller, values);
        }

        protected abstract void OnTriggerEvent(TriggerController controller, params Object[] values);

    }

}
