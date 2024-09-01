using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NightmareStandalone.Extensions;
namespace NightmareStandalone
{
    class NightmareBehaviour : MonoBehaviour
    {

        private static NightmareBehaviour _instance;

        public static NightmareBehaviour Instance
        {
            get { return _instance; }
        }
        public static NightmareBehaviour InstanceOrDefault
        {
            get
            {
                if (_instance == null)
                {
                    GameObject nightmareObj = new GameObject("NightmareBehaviour");
                    _instance = nightmareObj.AddComponent<NightmareBehaviour>();
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }

        public static bool NightmareAmbienceEnabled
        {
            get
            {
                if (Instance == null) return false;
                else return Instance.gameObject.activeSelf;
            }
        }

        public static Color NightmareAmbient { get; } = new Color(0.08f, 0.08f, 0.08f);

        public static Color NightmareLightning { get; } = new Color(0.3f, 0.3f, 0.3f);

        public static Color NightmareBackgroundLightning { get; } = new Color(0.08f, 0.08f, 0.08f);

        public static Color NightmareA { get; } = new Color(0.05f, 0, 0); //Red

        public static Color NightmareB { get; } = new Color(0.06f, 0, 0.15f); //Purple

        public static Color NightmareAltA { get; } = new Color(0.06f, 0, 0.15f); //Purple

        public static Color NightmareAltB { get; } = new Color(0, 0.08f, 0); //Green

        /*public static bool UseNightmareLights {
            get { return useNightmareLights; }
        }

        public static bool useNightmareLights = false;
        public static void ToggleNightmare(bool enterNightmare) {
            if (enterNightmare) {
                AudioBehaviour.Instance.PlayOneShotSound("NightmareMode.wav");
                useNightmareLights = true;
                ColourManager.RefreshLights(useNightmareLights);
            } else {
                AudioBehaviour.Instance.PlayOneShotSound("NormalWeaklingMode.wav");
                useNightmareLights = false;
                ColourManager.RefreshLights(useNightmareLights);
            }
        }*/

        public static void EnableNightmare(bool enterNightmare, bool playSound = true)
        {
            if (enterNightmare)
            {
                AudioUtil.Instance.PlayOneShotSound("NightmareMode.wav", 0.1f);
                InstanceOrDefault.gameObject.SetActive(true);
                AudioUtil.Instance.StartAmbianceSound("RainLoop.wav", 0.6f);
            }
            else
            {
                AudioUtil.Instance.PlayOneShotSound("NormalWeaklingMode.wav", 0.1f);
                AudioUtil.Instance.StopAmbianceSound();
                if (Instance) Instance.gameObject.SetActive(false);

            }
        }

        public BeatmapObjectManager objectManager;
        public void Setup()
        {
            objectManager = Resources.FindObjectsOfTypeAll<GameEnergyCounter>().FirstOrDefault()?.GetField<BeatmapObjectManager>("_beatmapObjectManager");
            if (objectManager == null)
            {
                Debug.Log("Null manager");
                return;
            }

            objectManager.noteWasCutEvent += NightmareNoteWasCutEvent;
            objectManager.noteWasMissedEvent += NightmareNoteWasMissedEvent;
            HarmonyPatches.NoteScaling.HandleNoteScalingEvent += NightmareNoteScalingEvent;
            //    objectManager.obstacleDidPassThreeQuartersOfMove2Event += NightmareBarrierSpawnedEvent;

            ResetNightmare();
        }


        /**************************************
         *************** LOGIC ****************
         *************************************/

        float nightmareScale = 0.7f;


        void ResetNightmare()
        {
            nightmareScale = 0.7f;
        }

        void OnEnable()
        {
            try
            {

            }
            catch (Exception e)
            {
                Logger.log.Debug("Error enabling NightmareBehaviour");
                Logger.log.Debug(e);
            }
        }

        void OnDisable()
        {
            try
            {

            }
            catch (Exception e)
            {
                Logger.log.Debug("Error disabling NightmareBehaviour");
                Logger.log.Debug(e);
            }
        }


        public void NightmareBarrierSpawnedEvent(ref StretchableObstacle stretchableObstacle, ref BeatmapObjectManager obstacleSpawnController, ref ObstacleController obstacleController, ref bool didRecolour)
        {
            didRecolour = true;
            ReflectionUtil.GetField<StretchableCube>(stretchableObstacle, "_stretchableCore").gameObject.SetActive(false);
        }

        public void NightmareNoteWasCutEvent(NoteController noteController, in NoteCutInfo noteCutInfo)
        {
            NoteBehaviour behaviour = NoteBehaviour.GetNoteBehaviour(noteController.noteData);
            if (behaviour != null && behaviour.ColourBehaviour == NoteBehaviour.NoteColourBehaviour.ANY)
            {
                if (noteCutInfo.allIsOK)
                {
                    nightmareScale = Mathf.Min(nightmareScale + 0.08f, 0.8f);
                    Triggers.TriggerController.Instance.Trigger(Triggers.Trigger.NOTE_CUT_MONOCHROME, GameMode.GameModeController.GetSelectedGameMode(), noteController, behaviour);
                }
                else
                {
                    nightmareScale = nightmareScale * 0.85f;
                }
            }
        }

        public void NightmareNoteWasMissedEvent(NoteController noteController)
        {
            NoteBehaviour behaviour = NoteBehaviour.GetNoteBehaviour(noteController.noteData);
            if (behaviour != null && behaviour.ColourBehaviour == NoteBehaviour.NoteColourBehaviour.ANY)
            {
                nightmareScale = nightmareScale * 0.85f;
            }
        }

        public void NightmareNoteScalingEvent(ref NoteData noteData, ref float tScale)
        {
            if (noteData.colorType != ColorType.None)
            {
                tScale = tScale * nightmareScale;
            }
        }

        private static float lastFlashNote = 0;
        public static bool ConvertToGrey(NoteData note)
        {
            if (note.time < lastFlashNote) lastFlashNote = 0;
            if (note.time - lastFlashNote >= 3.5)
            {
                System.Random bRand = new System.Random(Mathf.CeilToInt(note.time * 47 + 3855));
                if (bRand.NextDouble() < 0.2)
                {
                    lastFlashNote = note.time;
                    return true;
                }

            }
            return false;
        }




    }
}
