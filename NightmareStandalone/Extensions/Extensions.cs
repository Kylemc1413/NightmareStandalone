using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using IPA.Utilities;
namespace NightmareStandalone.Extensions
{
    public static class ColorNoteVisualsExtensions
    {


        public enum CNVDisplayType
        {
            NORMAL,
            BIDIRECTIONAL,
            DEFLECT,
        }

        private static Sprite dotSprite;

        public static void SetDotVisible(this ColorNoteVisuals cnv, SpriteRenderer dotRenderer, bool show)
        {
            /*if (dotSprite == null) dotSprite = dotRenderer.sprite;
            if (UnityEngine.Random.value < 0.001f) dotRenderer.sprite = CMBSprite;
            else dotRenderer.sprite = dotSprite;*/
            cnv.SetProperty("showCircle", show);
        }

        public static void SetArrowVisible(this ColorNoteVisuals cnv, bool show)
        {
            cnv.SetProperty("showArrow", show);
        }

        public static void SetSecondArrowVisible(this ColorNoteVisuals cnv, bool show, Color color)
        {
            if (show) cnv.EnableSecondArrow(color);
            else cnv.DisableSecondArrow();
        }


        private static readonly Vector3 defaultScale = Vector3.one;
        private static readonly Vector3 deflectScale = new Vector3(0.65f, 2.25f, 1);

        private static Sprite originalDotSprite;

        //private const string deflectSpriteString = "";
        //private static Sprite deflectSprite;

        public static void SetDisplayType(this ColorNoteVisuals cnv, Color color, SpriteRenderer dotRenderer, CNVDisplayType type, NoteCutDirection dir)
        {

            /*if (dotRenderer != null) {
                cnv.SetDotVisible(dotRenderer, true);
                cnv.SetArrowVisible(false);
                dotRenderer.transform.localScale = defaultScale / 3f;
                dotRenderer.sprite = CMBSprite;
            }*/

            if (type == CNVDisplayType.BIDIRECTIONAL)
            {
                cnv.SetDotVisible(dotRenderer, false);
                cnv.SetArrowVisible(true);
                cnv.SetSecondArrowVisible(true, color);
            }
            else
            {
                cnv.SetSecondArrowVisible(false, color);
            }

            dotRenderer.transform.localScale = type == CNVDisplayType.DEFLECT ? deflectScale : defaultScale;

            if (type == CNVDisplayType.NORMAL)
            {
                cnv.SetArrowVisible(dir != NoteCutDirection.Any);
                cnv.SetDotVisible(dotRenderer, dir == NoteCutDirection.Any);
            }
            else if (type == CNVDisplayType.DEFLECT)
            {
                cnv.SetArrowVisible(false);
                cnv.SetDotVisible(dotRenderer, true);
            }
        }


        private static Dictionary<ColorNoteVisuals, Tuple<MeshRenderer, SpriteRenderer>> secondArrows = new Dictionary<ColorNoteVisuals, Tuple<MeshRenderer, SpriteRenderer>>();

        private static void SetupDeflectSprite()
        {
        }

        public static void ClearExtraSprites()
        {
            secondArrows.Clear();
        }

        private static void DisableSecondArrow(this ColorNoteVisuals cnv)
        {
            if (secondArrows.ContainsKey(cnv))
            {
                Tuple<MeshRenderer, SpriteRenderer> t;
                if (secondArrows.TryGetValue(cnv, out t))
                {
                    t.Item1.enabled = false;
                    t.Item2.enabled = false;
                }
            }
        }

        public static void SwitchNoteColorType(this NoteData note)
        {
            note.SetProperty("colorType", note.colorType.Opposite());
        }
        internal static readonly FieldAccessor<BeatmapCallbacksController, IReadonlyBeatmapData>.Accessor _beatmapDataAccessor = FieldAccessor<BeatmapCallbacksController, IReadonlyBeatmapData>.GetAccessor("_beatmapData");

        public static void ModifyBeatmap(this BeatmapCallbacksController callbackController, Func<BeatmapDataItem, BeatmapDataItem> func, float startTime = 0, float endTime = float.MaxValue)
        {
            var beatmapData = _beatmapDataAccessor(ref callbackController) as BeatmapData;
            _beatmapDataAccessor(ref callbackController) = beatmapData.GetFilteredCopy(x => {

                if (x.time > startTime && x.time < endTime)
                    return func(x);
                return x;
            });
            callbackController.ResetCallbacksController(startTime, startTime);
        }

        public static void ResetCallbacksController(this BeatmapCallbacksController callbackController, float? prevSongTime = null, float? startFilterTime = null)
        {
            if (prevSongTime != null)
                callbackController.SetField("_prevSongTime", prevSongTime.Value);
            if (startFilterTime != null)
                callbackController.SetField("_startFilterTime", startFilterTime.Value);
            var dic = callbackController.GetField<Dictionary<float, CallbacksInTime>, BeatmapCallbacksController>("_callbacksInTimes");
            foreach (var item in dic.Values)
            {
                item.lastProcessedNode = null;
            }
        }

        public static void AddObjectsToBeatmap(this BeatmapCallbacksController callbackController, List<BeatmapObjectData> items)
        {
            var beatmapData = _beatmapDataAccessor(ref callbackController) as BeatmapData;
            foreach (var item in items)
                beatmapData.AddBeatmapObjectData(item);
        }
        public static void ReplaceData(this BeatmapCallbacksController callbackController, BeatmapData newData)
        {
            _beatmapDataAccessor(ref callbackController) = newData;
        }
        /*

        private static bool did = false;
        private static void EnableSecondArrow(ColorNoteVisuals cnv) {
            if (did) return;
            did = true;
            MeshRenderer omr = cnv.GetField<MeshRenderer>("_arrowMeshRenderer");
            SpriteRenderer osr = cnv.GetField<SpriteRenderer>("_arrowGlowSpriteRenderer");

            try {
                ChromaLogger.Log(" ");
                ChromaLogger.Log(" ");
                ChromaLogger.Log(" ");
                ChromaLogger.Log("CNV Transform: " + (cnv.transform.name));
                ChromaLogger.Log("CNV Components: " + (cnv.GetComponents<Component>().Length));
                foreach (Component c in cnv.GetComponents<Component>()) {
                    ChromaLogger.Log("____" + c.name);
                }
                ChromaLogger.Log(" ");
                ChromaLogger.Log("CNV parent: " + cnv.transform.parent.name);
                ChromaLogger.Log(" ");
                ChromaLogger.Log("CNV Children: " + cnv.transform.childCount);
                for (int i = 0; i < cnv.transform.childCount; i++) {
                    ChromaLogger.Log("Child 1 : " + cnv.transform.GetChild(i).name);
                    foreach (Component c in cnv.GetComponents<Component>()) {
                        ChromaLogger.Log("____" + c.name);
                    }
                }
            } catch (Exception e) {
                ChromaLogger.Log(e);
            }
            try {
                ChromaLogger.Log(" ");
                ChromaLogger.Log(" ");
                ChromaLogger.Log(" ");
                ChromaLogger.Log("OMR Transform: " + (omr.transform.name));
                ChromaLogger.Log("OMR Components: " + (omr.GetComponents<Component>().Length));
                foreach (Component c in omr.GetComponents<Component>()) {
                    ChromaLogger.Log("____" + c.name);
                }
                ChromaLogger.Log(" ");
                ChromaLogger.Log("OMR parent: " + omr.transform.parent.name);
                ChromaLogger.Log(" ");
                ChromaLogger.Log("OMR Children: " + omr.transform.childCount);
                for (int i = 0; i < omr.transform.childCount; i++) {
                    ChromaLogger.Log("Child 1 : " + omr.transform.GetChild(i).name);
                    foreach (Component c in omr.GetComponents<Component>()) {
                        ChromaLogger.Log("____" + c.name);
                    }
                }
            } catch (Exception e) {
                ChromaLogger.Log(e);
            }
            try {
                ChromaLogger.Log(" ");
                ChromaLogger.Log(" ");
                ChromaLogger.Log(" ");
                ChromaLogger.Log("OSR Transform: " + (osr.transform.name));
                ChromaLogger.Log("OSR Components: " + (osr.GetComponents<Component>().Length));
                foreach (Component c in osr.GetComponents<Component>()) {
                    ChromaLogger.Log("____" + c.name);
                }
                ChromaLogger.Log(" ");
                ChromaLogger.Log("OSR parent: " + osr.transform.parent.name);
                ChromaLogger.Log(" ");
                ChromaLogger.Log("OSR Children: " + osr.transform.childCount);
                for (int i = 0; i < osr.transform.childCount; i++) {
                    ChromaLogger.Log("Child 1 : " + osr.transform.GetChild(i).name);
                    foreach (Component c in osr.GetComponents<Component>()) {
                        ChromaLogger.Log("____" + c.name);
                    }
                }
            } catch (Exception e) {
                ChromaLogger.Log(e);
            }
            //foreach (Transform transform in cnv.transform.GetChildCount())
        }

        */

        private static void EnableSecondArrow(this ColorNoteVisuals cnv, Color color)
        {
            Tuple<MeshRenderer, SpriteRenderer> t;
            if (secondArrows.ContainsKey(cnv))
            {
                if (secondArrows.TryGetValue(cnv, out t))
                {
                    t.Item1.enabled = true;
                    t.Item2.enabled = false; // TODO re-enable this after fixing brightness
                    t.Item2.color = color;
                    return;
                }
            }
            MeshRenderer omr = cnv.GetField<MeshRenderer>("_arrowMeshRenderer");
            SpriteRenderer osr = cnv.GetField<SpriteRenderer>("_arrowGlowSpriteRenderer");

            Transform flipper = new GameObject("Reverse Arrow Parent").transform;
            flipper.SetParent(omr.transform.parent);
            flipper.localPosition = Vector3.zero; flipper.localRotation = Quaternion.identity;

            GameObject mrObj = GameObject.Instantiate(omr.gameObject);
            //mrObj.transform.SetParent(omr.transform.parent);
            //mrObj.transform.position = omr.transform.position; mrObj.transform.rotation = omr.transform.rotation;
            MeshRenderer mr = mrObj.GetComponent<MeshRenderer>();

            GameObject srObj = GameObject.Instantiate(osr.gameObject);
            //srObj.transform.SetParent(osr.transform.parent);
            //srObj.transform.position = osr.transform.position; srObj.transform.rotation = osr.transform.rotation;
            SpriteRenderer sr = srObj.GetComponent<SpriteRenderer>();

            mrObj.transform.SetParent(flipper);
            mrObj.transform.localPosition = omr.transform.localPosition; mrObj.transform.localRotation = omr.transform.localRotation;
            srObj.transform.SetParent(flipper);
            srObj.transform.localPosition = osr.transform.localPosition; srObj.transform.localRotation = osr.transform.localRotation;

            flipper.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));

            //mrObj.transform.localPosition = new Vector3(mr.transform.localPosition.x, -mr.transform.localPosition.y, mr.transform.localPosition.z);// mrObj.transform.localPosition + Vector3.up;
            //srObj.transform.localPosition = new Vector3(sr.transform.localPosition.x, -sr.transform.localPosition.y, sr.transform.localPosition.z);// srObj.transform.localPosition + Vector3.up;

            //sr.flipY = true;

            //mrObj.transform.localRotation = Quaternion.Euler(omr.transform.localRotation.eulerAngles + new Vector3(0, 180, 0));
            //srObj.transform.localRotation = Quaternion.Euler(osr.transform.localRotation.eulerAngles + new Vector3(0, 180, 0));

            t = new Tuple<MeshRenderer, SpriteRenderer>(mr, sr);
            t.Item1.enabled = true;
            t.Item2.enabled = false; // TODO re-enable this after fixing brightness
            t.Item2.color = color;

            flipper.localScale = omr.transform.localScale;

            secondArrows.Add(cnv, t);
        }


    }
}
