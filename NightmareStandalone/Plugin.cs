using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IPA;
using IPA.Config;
using IPA.Utilities;
using UnityEngine.SceneManagement;
using UnityEngine;
using IPALogger = IPA.Logging.Logger;
using HarmonyLib;
using System.IO;
using NightmareStandalone.Extensions;
using IPA.Utilities;
namespace NightmareStandalone
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static string Name => "NightmareStandalone";
        [Init]
        public void Init(IPALogger logger)
        {
            Logger.log = logger;
        }

        [OnStart]
        public void OnApplicationStart()
        {
            Config.Read();
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
            SceneManager.sceneLoaded += OnSceneLoaded;
            BS_Utils.Utilities.BSEvents.gameSceneLoaded += BSEvents_gameSceneLoaded;
            BeatSaberMarkupLanguage.GameplaySetup.GameplaySetup.instance.AddTab("Nightmare", "NightmareStandalone.UI.BSML.modifierUI.bsml", UI.ModifierUI.instance);
            var harmony = new Harmony("net.kyle1413.nightmarestandalone");
            harmony.PatchAll();
            GameMode.GameMode.InitializeGameModes();
            Extensions.GameModeMapModifier.RegisterListeners();
        }

        private void BSEvents_gameSceneLoaded()
        {
            Extensions.NoteBehaviour.ClearSpecialNotes();
            InitNightmare();

        }
        internal static void InitNightmare()
        {
            HarmonyPatches.NoteScaling.HandleNoteScalingEvent -= NightmareBehaviour.InstanceOrDefault.NightmareNoteScalingEvent;
            if (!BS_Utils.Plugin.LevelData.IsSet) return;
            if (Config.enabled && BS_Utils.Plugin.LevelData.Mode == BS_Utils.Gameplay.Mode.Standard)
            {
                BS_Utils.Gameplay.ScoreSubmission.DisableSubmission("NightmareStandalone");


                NightmareBehaviour.InstanceOrDefault.StartCoroutine(DelayedStart());
            }
        }
        public static IEnumerator DelayedStart()
        {
            yield return new WaitForSeconds(0.1f);

            var callbackController = Resources.FindObjectsOfTypeAll<BeatmapObjectSpawnController>().LastOrDefault().GetField<BeatmapCallbacksController, BeatmapObjectSpawnController>("_beatmapCallbacksController");
            var thedata = callbackController.GetField<IReadonlyBeatmapData, BeatmapCallbacksController>("_beatmapData");
            if (callbackController == null) Debug.Log("null callback");

            var customData = Extensions.MapModifier.CreateTransformedBeatmapData(thedata,
                BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.playerSpecificSettings, GameMode.BaseGameModeType.SoloStandard, BS_Utils.Plugin.LevelData.GameplayCoreSceneSetupData.difficultyBeatmap.level.beatsPerMinute);
            if (customData != null)
            {
                callbackController.ReplaceData(customData as BeatmapData);
                NightmareBehaviour.InstanceOrDefault.Setup();
            }

            else
                Debug.Log("Null Custom");
        }
        public static bool Safe()
        {
            return BS_Utils.Plugin.LevelData.IsSet && BS_Utils.Plugin.LevelData.Mode == BS_Utils.Gameplay.Mode.Standard && Config.enabled;
        }
        [OnExit]
        public void OnApplicationQuit()
        {

            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

    

        /// <summary>
        /// Called when the active scene is changed.
        /// </summary>
        /// <param name="prevScene">The scene you are transitioning from.</param>
        /// <param name="nextScene">The scene you are transitioning to.</param>
        public void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
        {
            Config.Read();
        }

        /// <summary>
        /// Called when the a scene's assets are loaded.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="sceneMode"></param>
        public void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            NightmareBehaviour.InstanceOrDefault.StopAllCoroutines();


        }

    }
}
