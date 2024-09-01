using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace NightmareStandalone.Extensions
{
    public class MapModifier
    {
        public delegate void ModifyCustomBeatmapDelegate(int randSeed, ref CustomBeatmap customBeatmap, ref BeatmapData baseBeatmapData, ref PlayerSpecificSettings playerSettings, ref GameMode.BaseGameModeType baseGameMode, ref float bpm);

        public static event ModifyCustomBeatmapDelegate ModifyCustomBeatmapEvent;

        public static CustomBeatmap CreateTransformedData(IReadonlyBeatmapData beatmapData, ref PlayerSpecificSettings playerSettings, ref GameMode.BaseGameModeType baseGameMode, ref float bpm)
        {


            if (beatmapData == null) Logger.log.Error("Null beatmapData");
            if (playerSettings == null) Logger.log.Error("Null playerSettings");

            List<CustomBeatmapObject> customBeatmapData = new List<CustomBeatmapObject>();

            beatmapData = beatmapData.GetCopy();
            var beatmapObjects = beatmapData.allBeatmapDataItems.Where(x => x is BeatmapObjectData).Cast<BeatmapObjectData>();
            var beatmapEvents = beatmapData.allBeatmapDataItems.Where(x => x is BeatmapEventData).Cast<BeatmapEventData>();
            UnityEngine.Random.InitState(0);
            int id = 0;
            foreach(var beatmapObject in beatmapObjects)
            {
                if (beatmapObject is NoteData note)
                {
                    if (note.gameplayType == NoteData.GameplayType.Bomb)
                        customBeatmapData.Add(new CustomBeatmapBomb(note, id));
                    else
                        customBeatmapData.Add(new CustomBeatmapNote(note, id));
                }
                else if (beatmapObject is ObstacleData obstacle)
                    customBeatmapData.Add(new CustomBeatmapBarrier(obstacle, id));
                id++;
            }
            CustomBeatmap customBeatmap = new CustomBeatmap(customBeatmapData.OrderBy(x => x.Data.time).ToList());

            try
            {
                Logger.log.Debug("Modifying map data...");
                if (beatmapData == null) Logger.log.Error("Null beatmapData");
                if (beatmapEvents == null) Logger.log.Error("Null beatmapData.beatmapEventData");
                //ModifyCustomBeatmapEvent?.Invoke(beatmapData.beatmapObjectsData.Count() * beatmapData.beatmapEventsData.Count, ref customBeatmap, ref beatmapData, ref playerSettings, ref baseGameMode, ref bpm);
                GameModeMapModifier.ModifyCustomBeatmap(beatmapData.allBeatmapDataItems.Count(), ref customBeatmap, ref beatmapData, ref playerSettings, ref baseGameMode, ref bpm);
            }
            catch (Exception e)
            {
                Logger.log.Error("Exception modifying map data...");
                Logger.log.Error(e);
            }

            customBeatmap.BeatmapData = beatmapData;


            return customBeatmap;

        }
        public static IReadonlyBeatmapData CreateTransformedBeatmapData(IReadonlyBeatmapData beatmapData, PlayerSpecificSettings playerSettings, GameMode.BaseGameModeType baseGameMode, float bpm)
        {
            if (beatmapData == null)
            {
                Debug.Log("Null Data");
                return null;
            }
            CustomBeatmap customBeatmap = CreateTransformedData(beatmapData, ref playerSettings, ref baseGameMode, ref bpm);
            return customBeatmap.BeatmapData;
        }
    }
}
