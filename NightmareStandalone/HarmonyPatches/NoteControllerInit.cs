using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
namespace NightmareStandalone.HarmonyPatches
{
    [HarmonyPriority(Priority.Low)]
    [HarmonyPatch(typeof(NoteController))]
    [HarmonyPatch("Init")]
    class NoteControllerInit
    {

        static void Postfix(NoteController __instance, NoteData ____noteData)
        {
            if (!Plugin.Safe()) return;
            __instance.noteTransform.localScale = Vector3.one /*__instance.noteTransform.localScale*/ * NoteScaling.GetNoteScale(____noteData); //ChromaNoteScaleEvent.GetScale(____noteData.time);
        }
    }
    public static class NoteScaling
    {

        public delegate void HandleNoteScalingDelegate(ref NoteData note, ref float scale);
        public static event HandleNoteScalingDelegate HandleNoteScalingEvent;

        public static float GetNoteScale(NoteData note)
        {
            try
            {
                float s = 1f;
                HandleNoteScalingEvent?.Invoke(ref note, ref s);
                return s;
            }
            catch (Exception e)
            {
                Logger.log.Debug(e);
                return 1;
            }
        }

    }
}
