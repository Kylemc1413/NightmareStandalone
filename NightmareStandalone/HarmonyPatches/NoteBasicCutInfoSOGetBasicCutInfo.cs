using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NightmareStandalone.Extensions;
using HarmonyLib;
namespace NightmareStandalone.HarmonyPatches
{
    [HarmonyPatch(typeof(NoteBasicCutInfoHelper))]
    [HarmonyPatch("GetBasicCutInfo")]
    class NoteBasicCutInfoSOGetBasicCutInfo
    {

        private static readonly Vector3 upleft = (Vector3.up + Vector3.left).normalized;
        private static readonly Vector3 downleft = (Vector3.down + Vector3.left).normalized;
        private static readonly Vector3 downright = (Vector3.down + Vector3.right).normalized;
        private static readonly Vector3 upright = (Vector3.up + Vector3.right).normalized;

        //public static bool checkOppositeDirection = false;
        public static NoteBehaviour.NoteCutBehaviour checkMechanic = NoteBehaviour.NoteCutBehaviour.NORMAL;
        public static NoteBehaviour.NoteColourBehaviour colorCheck = NoteBehaviour.NoteColourBehaviour.NORMAL;
        public static Vector3 saberDeflectionAngle;

        public static bool Prefix(Transform noteTransform, ColorType colorType, NoteCutDirection cutDirection, SaberType saberType, float saberBladeSpeed, Vector3 cutDirVec,
            out bool directionOK, out bool speedOK, out bool saberTypeOK, out float cutDirDeviation)
        {
            if (!Plugin.Safe())
            {
                directionOK = false;
                speedOK = false;
                saberTypeOK = false;
                cutDirDeviation = 0f;
                return true;
            }
            cutDirVec = noteTransform.InverseTransformVector(cutDirVec);
            bool flag = Mathf.Abs(cutDirVec.z) > Mathf.Abs(cutDirVec.x) * 10f && Mathf.Abs(cutDirVec.z) > Mathf.Abs(cutDirVec.y) * 10f;
            float num = Mathf.Atan2(cutDirVec.y, cutDirVec.x) * 57.29578f;
            float num2 = 60f; //strictAngles not part of basicCutInfo
            if (checkMechanic == NoteBehaviour.NoteCutBehaviour.NORMAL || checkMechanic == NoteBehaviour.NoteCutBehaviour.BIDIRECTIONAL)
            {
                directionOK = ((!flag && num > -90f - num2 && num < -90f + num2) || cutDirection == NoteCutDirection.Any);
                if (!directionOK && checkMechanic == NoteBehaviour.NoteCutBehaviour.BIDIRECTIONAL)
                {
                    float numRev = Mathf.Atan2(-cutDirVec.y, -cutDirVec.x) * 57.29578f;
                    bool flagRev = Mathf.Abs(-cutDirVec.z) > Mathf.Abs(-cutDirVec.x) * 10f && Mathf.Abs(-cutDirVec.z) > Mathf.Abs(-cutDirVec.y) * 10f;
                    directionOK = (!flagRev && numRev > -90f - num2 && numRev < -90f + num2);
                }
            }
            else if (checkMechanic == NoteBehaviour.NoteCutBehaviour.DEFLECT)
            {
                saberDeflectionAngle.z = saberDeflectionAngle.z / 2f; //saber.transform.forward
                Vector3 targetDirection;
                switch (cutDirection)
                {
                    case NoteCutDirection.Down: targetDirection = Vector3.down; break;
                    case NoteCutDirection.Left: targetDirection = Vector3.left; break;
                    case NoteCutDirection.Right: targetDirection = Vector3.right; break;
                    case NoteCutDirection.Up: targetDirection = Vector3.up; break;
                    case NoteCutDirection.DownLeft: targetDirection = downleft; break;
                    case NoteCutDirection.DownRight: targetDirection = downright; break;
                    case NoteCutDirection.UpRight: targetDirection = upright; break;
                    case NoteCutDirection.UpLeft: targetDirection = upleft; break;
                    default: targetDirection = noteTransform.up; break;
                }
                float t = Vector3.Angle(saberDeflectionAngle, targetDirection);
                directionOK = t < 30f;
                if (!directionOK)
                {
                    t = Vector3.Angle(saberDeflectionAngle, -targetDirection);
                    directionOK = t < 30f;
                }
                if (directionOK)
                {
                    t = Mathf.Max(t - 5f, 0);
              //      ChromaScoring.overrideCutDistanceScore = Mathf.RoundToInt(Mathf.Lerp(0, 10, t / 25f));
                }
            }
            else
            {
             //   ChromaLogger.Log("This code should never be reached!  Invalid NoteCutBehaviour in NoteBasicCutInfoSO_GetBasicCutInfo Patch", ChromaLogger.Level.ERROR);
                directionOK = false;
            }
            speedOK = (saberBladeSpeed > 4f) || checkMechanic == NoteBehaviour.NoteCutBehaviour.DEFLECT;
            saberTypeOK = ((colorType == ColorType.ColorA && saberType == SaberType.SaberA) || (colorType == ColorType.ColorB && saberType == SaberType.SaberB) || colorCheck == NoteBehaviour.NoteColourBehaviour.ANY);
            cutDirDeviation = ((!flag) ? (num + 90f) : 90f);
            if (cutDirDeviation > 180f)
            {
                cutDirDeviation -= 360f;
            }

            return false;
        }

    }
}
