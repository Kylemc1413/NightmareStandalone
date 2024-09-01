using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using IPA.Utilities;
namespace NightmareStandalone.Triggers
{
    public class TriggeredEventRealtimeMirror : TriggeredEvent
    {

        public TriggeredEventRealtimeMirror()
        {

        }

        protected override void OnTriggerEvent(TriggerController controller, params System.Object[] values)
        {

            NoteJump[] movers = Resources.FindObjectsOfTypeAll<NoteJump>();
            foreach (NoteJump mover in movers)
            {
                Vector3 endPos = mover.GetField<Vector3, NoteJump>("_endPos");
                endPos.x = -endPos.x;
                mover.SetField("_endPos", endPos);
            }

        }

    }
}
