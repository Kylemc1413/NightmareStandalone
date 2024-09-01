using NightmareStandalone.Triggers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
namespace NightmareStandalone.GameMode
{
    public enum BaseGameModeType
    {

        SoloStandard,
        SoloOneSaber,
        SoloNoArrows,
        Unknown

    }
    internal class OfficialGameMode : GameMode
    {


        public OfficialGameMode(string internalName, string displayName, params BaseGameModeType[] allowedModes)
        {

            info.internalName = internalName;
            info.displayName = displayName;
            info.gameplayModes = allowedModes;
            info.showInMenu = true;
        }
    }
    public abstract class GameMode
    {

        public class Info
        {

            [XmlElement("InternalName")]
            public string internalName = "UNNAMED_MODE";

            [XmlElement("DisplayName")]
            public string displayName = "UNNAMED_MODE";

            [XmlElement("ShowInMenu")]
            public bool showInMenu = false;

            [XmlElement("AllowedGamemodes")]
            public BaseGameModeType[] gameplayModes = new BaseGameModeType[] { BaseGameModeType.SoloStandard };

        }

        public class ColourOptions
        {

            [XmlElement("Pentachrome")]
            public bool alternateColours = false;

            [XmlElement("Monochrome")]
            public bool monochrome = false;

            [XmlElement("InvertColoursOnMirror")]
            public bool invertColoursOnMirror = false;

        }

        public class MapOptions
        {

            [XmlElement("AllNotesDots")]
            public bool allNotesDots = false;

            [XmlElement("DoubleHitsDots")]
            public bool doubleHitsDots = false;

            [XmlElement("DoubleHitsMonochrome")]
            public bool doubleHitsMonochrome = false;

            [XmlElement("DoubleHitsDuochrome")]
            public bool doubleHitsDuochrome = false;

            [XmlElement("DoubleHitsRemoved")]
            public bool doubleHitsRemoved = false;

            [XmlElement("ExtendedSlashMonochrome")]
            public bool extendedSlashMonochrome = false;

            [XmlElement("ExtendedSlashRemoved")]
            public bool extendedSlashRemoved = false;

            [XmlElement("MirrorColours")]
            public bool mirrorColour = false;

            [XmlElement("MirrorDirections")]
            public bool mirrorDirection = false;

            [XmlElement("MirrorPositions")]
            public bool mirrorPosition = false;

        }

        public class RandomizationOptions
        {

            [XmlElement("AltColourRandomizationStyle")]
            public int altColourRandomizationStyle = 0;

            [XmlElement("AltColourRandomizationFactor")]
            public float altColourRandomizationFactor = 0;

            [XmlElement("AltColourForcedSwitchFactor")]
            public float altColourForcedSwitchFactor = 0; //Used in Controlled V2

            [XmlElement("AltColourGapLength")]
            public float altColourGapLength = 0; //Used in Controlled V2

            [XmlElement("AltColourRandomizationUniformness")]
            public float altColourRandomizationUniformness = 0; //Used in Controlled V2

            [XmlElement("AltColourGreyInsertionFactor")]
            public float altColourGreyInsertionFactor = 0; //Used in Controlled V2

            [XmlElement("AltColourMinimumSwitchLength")]
            public float altColourMinimumSwitchLength = 0; //Used in Controlled V2

            [XmlElement("AltColourBPMPower")]
            public float altColourBPMPower = 0; //Used in Controlled V2

            [XmlElement("AltColourRevertChanceFactor")]
            public float altColourRevertChanceFactor = 2f; //Used in V2, but not originally made for it - also used in Controlled

            [XmlElement("AltColourIntermediaryGreys")]
            public int altColourIntermediaryGreys = 0;

            [XmlElement("BaseColourRandomizationStyle")]
            public int baseColourRandomizationStyle = 0;

            //[XmlElement("BaseColourRandomizationFactor")]
            //public float baseColourRandomizationFactor = 0;

            [XmlElement("RandomRemoveChance")]
            public float randomRemoveChance = 0;

            [XmlElement("RandomDotsChance")]
            public float randomDotsChance = 0;

            [XmlElement("RandomGreyBlocksChance")]
            public float randomGreyBlocksChance = 0;

            [XmlElement("RandomSuperBlocksChance")]
            public float randomSuperBlocksChance = 0;

            [XmlElement("RandomDuochromeBlocksAvoidDoubles")]
            public bool randomDuochromeBlocksAvoidDoubles = false;

            [XmlElement("RandomDuochromeBlocksChance")]
            public float randomDuochromeBlocksChance = 0;

            [XmlElement("RandomMirrorChance")]
            public float randomMirrorChance = 0;

            [XmlElement("RandomMirrorDirectionChance")]
            public float randomMirrorDirectionChance = 0;

            [XmlElement("RandomMirrorPositionChance")]
            public float randomMirrorPositionChance = 0;

            [XmlElement("RandomBidirectionalChance")]
            public float randomBidirectionalChance = 0;

            [XmlElement("RandomDeflectChance")]
            public float randomDeflectChance = 0;

        }

        public class BarrierOptions
        {

            [XmlElement("MaxHighBarrierLength")]
            public int maxHighBarrierLength = -1;

            [XmlElement("MaxHighBarrierWidth")]
            public int maxHighBarrierWidth = -1;

            [XmlElement("MaxBarrierLength")]
            public int maxBarrierLength = -1;

            [XmlElement("MaxBarrierWidth")]
            public int maxBarrierWidth = -1;

        }

        public class ScaleOptions
        {

            [XmlElement("BombStartingSize")]
            public float bombStartingSize = 1;

            [XmlElement("NoteStartingSize")]
            public float noteStartingSize = 1;

            [XmlElement("NoteComboScaleFactor")]
            public float noteComboScaleFactor = 0;

        }




        public class TimeOptions
        {

            [XmlElement("BaseTimeScale")]
            public float baseTimeScale = 1f;

        }

        public class SpecialOptions
        {

            [XmlElement("Nightmare"), DefaultValue(false)]
            public bool nightmare = false;

            [XmlElement("RandomizedToggling"), DefaultValue(false)]
            public bool randomToggle = false;

            [XmlElement("RequiresWaiver"), DefaultValue(false)]
            public bool requiresWaiver = false;

            [XmlElement("Insanity")]
            public InsanityOptions insanityOptions = new InsanityOptions();

            [XmlElement("TimeFlux")]
            public TimeFluctuationOptions timeFluxOptions = new TimeFluctuationOptions();

            public class InsanityOptions
            {

                public bool Enabled
                {
                    get { return maxXRot != 0 || maxYRot != 0 || maxZRot != 0; }
                }

                [XmlElement("MaxXRot"), DefaultValue(0f)]
                public float maxXRot = 0f; //15f

                [XmlElement("MaxYRot"), DefaultValue(0f)]
                public float maxYRot = 0f; //360f

                [XmlElement("MaxZRot"), DefaultValue(0f)]
                public float maxZRot = 0f; //15f

            }

            public class TimeFluctuationOptions
            {

                public bool Enabled
                {
                    get { return (fluxSpeed > 0) && (minTime != maxTime); }
                }

                [XmlElement("FluctuationSpeed"), DefaultValue(0f)]
                public float fluxSpeed = 0f;

                [XmlElement("MinTime"), DefaultValue(1f)]
                public float minTime = 1f;

                [XmlElement("MaxTime"), DefaultValue(1f)]
                public float maxTime = 1f;

            }

        }

        public class TriggerOptions
        {

            [XmlElement("TriggerEvent")]
            public TriggeredEvent[] triggeredEvents = new TriggeredEvent[0];

        }

        [XmlElement("Info")]
        public Info info = new Info();

        [XmlElement("Barrier")]
        public BarrierOptions barrierOptions = new BarrierOptions();
        [XmlElement("Colour")]
        public ColourOptions colourOptions = new ColourOptions();
        [XmlElement("Map")]
        public MapOptions mapOptions = new MapOptions();
        [XmlElement("Randomization")]
        public RandomizationOptions randomizationOptions = new RandomizationOptions();

        [XmlElement("Scale")]
        public ScaleOptions scaleOptions = new ScaleOptions();

        [XmlElement("Time")]
        public TimeOptions timeOptions = new TimeOptions();
        [XmlElement("TriggerEvents")]
        public TriggerOptions triggerOptions = new TriggerOptions();
        [XmlElement("Special")]
        public SpecialOptions specialOptions = new SpecialOptions();

        protected GameMode()
        {
        }

        public void ValidateName()
        {
            if (info.displayName == null) info.displayName = info.internalName;
            info.internalName = StringHelper.RemoveSpecialCharacters(info.internalName);
            if (info.displayName.Length > 10) info.displayName = info.displayName.Substring(0, 10);
        }


        public static GameMode DefaultMode { get; protected set; }

        public static Dictionary<int, GameMode> gameModes = new Dictionary<int, GameMode>();

        public static GameMode GetGameMode(int i)
        {
            if (gameModes.ContainsKey(i)) return gameModes[i];
            else return DefaultMode;
        }

        public static GameMode GetGameMode(String s)
        {
            foreach (int i in gameModes.Keys)
            {
                if (gameModes[i].info.internalName == s) return gameModes[i];
            }
            return DefaultMode;
        }

        protected static bool AddGameMode(GameMode mode)
        {
            Logger.log.Debug("Registering Gamemode " + mode.info.internalName + "...");
            GameMode duplicate = GetGameMode(mode.info.internalName);
            if (duplicate != DefaultMode)
            {
                Logger.log.Debug("Duplicate gamemode found!  The name " + mode.info.internalName + " was used multiple times!");
                return false;
            }
            int i = gameModes.Count;
            while (gameModes.ContainsKey(i) || (i >= -1 && i <= 0)) i++;
            gameModes.Add(i, mode);
            return true;
        }

        public static void InitializeGameModes()
        {

            Logger.log.Debug("Initializing game modes...");

            gameModes.Clear();

            SetupNightmare();


            Logger.log.Debug("Game modes initialized.");
        }

        public static void SetupNightmare()
        {
            GameMode nightmare = new OfficialGameMode("OFFICIAL_nightmare", "Nightmare", BaseGameModeType.SoloStandard, BaseGameModeType.SoloNoArrows);
            nightmare.colourOptions.alternateColours = true;
            nightmare.randomizationOptions.randomDotsChance = 0.2f;
            nightmare.randomizationOptions.randomMirrorChance = 0.3f;
            nightmare.randomizationOptions.randomMirrorDirectionChance = 0.3f;
            nightmare.randomizationOptions.randomMirrorPositionChance = 0.3f;
            nightmare.randomizationOptions.randomBidirectionalChance = 0.15f;
        //    nightmare.randomizationOptions.randomDeflectChance = 0.05f;
            nightmare.triggerOptions = new TriggerOptions
            {
                triggeredEvents = new TriggeredEvent[]
                {
                    new TriggeredEventRealtimeMirror { chance = 1, triggers = new Trigger[] { Trigger.NOTE_CUT_MONOCHROME}, cooldown = 0, delay = 0.1f, failCooldown = false }
                }
            };
            //nightmare.randomizationOptions.randomDuochromeBlocksAvoidDoubles = true;
            //nightmare.randomizationOptions.randomDuochromeBlocksChance = 0.045f;
            nightmare.specialOptions.nightmare = true;
            GameMode.AddGameMode(nightmare);
            GameModeController.SetGameMode(nightmare);
            nightmare = null;
        }

    }

}
