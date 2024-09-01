using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using HarmonyLib;
using NightmareStandalone.Extensions;
namespace NightmareStandalone.HarmonyPatches
{
    [HarmonyPatch(typeof(GameNoteController))]
    [HarmonyPatch("HandleCut")]
    class GameNoteControllerHandleCut
    {

        public static bool Prefix(GameNoteController __instance, ref Transform ____noteTransform, ref Saber saber, ref Vector3 cutPoint, ref Quaternion orientation, ref Vector3 cutDirVec, ref bool allowBadCut, AudioTimeSyncController ____audioTimeSyncController, ref float ____cutAngleTolerance)
        {
            if (!Plugin.Safe()) return true;
            try
            {
                float timeDeviation = __instance.noteData.time - ____audioTimeSyncController.songTime;
                bool directionOK;
                bool speedOK;
                bool saberTypeOK;
                float cutDirDeviation;
                float cutDirAngle;
                NoteBehaviour behaviour = NoteBehaviour.GetNoteBehaviour(__instance);
                if (behaviour != null)
                {
                    //if (behaviour.ColourBehaviour == NoteBehaviour.NoteColourBehaviour.DUOCHROME) allowBadCut = false;
                    //if (behaviour.CutBehaviour == NoteBehaviour.NoteCutBehaviour.BIDIRECTIONAL) NoteBasicCutInfoSOGetBasicCutInfo.checkOppositeDirection = true;
                    NoteBasicCutInfoSOGetBasicCutInfo.checkMechanic = behaviour.CutBehaviour;
                    NoteBasicCutInfoSOGetBasicCutInfo.colorCheck = behaviour.ColourBehaviour;
                }
                else
                {
                    NoteBasicCutInfoSOGetBasicCutInfo.checkMechanic = NoteBehaviour.NoteCutBehaviour.NORMAL;
                    NoteBasicCutInfoSOGetBasicCutInfo.colorCheck = NoteBehaviour.NoteColourBehaviour.NORMAL;
                }

                NoteBasicCutInfoSOGetBasicCutInfo.saberDeflectionAngle = saber.transform.forward;
                NoteBasicCutInfoHelper.GetBasicCutInfo(__instance.noteTransform, __instance.noteData.colorType, __instance.noteData.cutDirection, 
                    saber.saberType, saber.bladeSpeed, cutDirVec, ____cutAngleTolerance, out directionOK, out speedOK, out saberTypeOK, out cutDirDeviation, out cutDirAngle);
                NoteBasicCutInfoSOGetBasicCutInfo.checkMechanic = NoteBehaviour.NoteCutBehaviour.NORMAL;
                /*
                if (chroma != null)
                {
                    ChromaSaber.CanCut(saber, behaviour, __instance, ref speedOK, ref directionOK, ref saberTypeOK, ref cutDirDeviation, ref allowBadCut);
                    if (ChromaToggleConfig.OverrideHaptics && speedOK && directionOK && saberTypeOK)
                    {
                        //chroma.AddCuttingSaber(__instance, saber);
                        chroma.TriggerHaptics(saber, ChromaToggleConfig.CustomHapticsDuration, ChromaToggleConfig.CustomHapticsStrength);
                    }
                }
                */
                if (!allowBadCut)
                {
                    return false;
                }
                Vector3 vector = orientation * Vector3.up;
                Plane plane = new Plane(vector, cutPoint);
                float cutDistanceToCenter = Mathf.Abs(plane.GetDistanceToPoint(__instance.noteTransform.position));
                Quaternion worldRotation = __instance.worldRotation;
                Quaternion inverseWorldRotation = __instance.inverseWorldRotation;
                NoteCutInfo noteCutInfo = new NoteCutInfo(__instance.noteData, speedOK, directionOK, saberTypeOK, 
                    false, saber.bladeSpeed, cutDirVec, saber.saberType, timeDeviation,
                    cutDirDeviation, plane.ClosestPointOnPlane(__instance.transform.position), vector, 
                    cutDistanceToCenter, cutDirAngle, worldRotation, inverseWorldRotation, ____noteTransform.rotation, ____noteTransform.position, saber.movementData);

                __instance.GetField<BoxCuttableBySaber>("_bigCuttableBySaber").canBeCut = false;
                __instance.GetField<BoxCuttableBySaber>("_smallCuttableBySaber").canBeCut = false;
                ReflectionUtil.InvokeMethod(__instance, "SendNoteWasCutEvent", noteCutInfo);

          //      ChromaSaber chromaSaber = ChromaSaber.GetChromaSaber(saber);
                //if (chromaSaber != null) chromaSaber.NoteWasCut(__instance);


            }
            catch (Exception e)
            {
                NoteBasicCutInfoSOGetBasicCutInfo.checkMechanic = NoteBehaviour.NoteCutBehaviour.NORMAL;
                Logger.log.Error(e);
            }

            return false;
        }

        /*static void Postfix(GameNoteController __instance, Saber saber) {
            if (Plugin.OverrideHaptics) {
                ChromaBehaviour chroma = ChromaBehaviour.Instance;
                if (chroma != null) {
                    //chroma.AddCuttingSaber(__instance, saber);
                    chroma.TriggerHaptics(saber, Plugin.customHapticsDuration, Plugin.customHapticsStrength);
                }
            }
            ChromaSaber chromaSaber = ChromaSaber.GetChromaSaber(saber);
            if (chromaSaber != null) chromaSaber.NoteWasCut(__instance);


        }*/


    }
}
