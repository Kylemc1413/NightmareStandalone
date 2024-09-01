using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;
namespace NightmareStandalone.Extensions
{
    class GameModeMapModifier
    {

        internal static void RegisterListeners()
        {
          //  MapModifier.ModifyCustomBeatmapEvent += ModifyCustomBeatmap;
        }

        public static void ModifyCustomBeatmap(int randSeed, ref CustomBeatmap customBeatmap, ref IReadonlyBeatmapData baseBeatmapData, ref PlayerSpecificSettings playerSettings, ref GameMode.BaseGameModeType baseGameMode, ref float bpm)
        {
         
            GameMode.GameMode mode;
            try
            {
                mode = GameMode.GameModeController.GetSelectedGameMode();
            }
            catch (Exception e)
            {
                Logger.log.Error("Failure to obtain ChromaGameMode from ChromaToggleBehaviour");
                Logger.log.Error(e);
                mode = null;
                return;
            }
            Logger.log.Debug("Rand seed: " + randSeed);

            System.Random intenseRandom = new System.Random(randSeed);
            System.Random dotsRandom = new System.Random(randSeed * 2);
            System.Random removeRandom = new System.Random((randSeed * 2) + 4);
            //System.Random mirrorDirectionRandom = new System.Random((randSeed + 2) * 2);
            //System.Random mirrorRandom = new System.Random((randSeed + 4) * 2);
            //System.Random mirrorPositionRandom = new System.Random((randSeed + 8) * 2);
            System.Random altColourRandom = new System.Random((randSeed + 16) * 2);
            System.Random greyRandom = new System.Random((randSeed + 32) * 2);
            System.Random superRandom = new System.Random((randSeed + 40) * 2);
            System.Random duochromeRandom = new System.Random((randSeed + 48) * 2);
            System.Random bidirectionalRandom = new System.Random((randSeed + 64) * 2);
            System.Random deflectRandom = new System.Random((randSeed + 80) * 2);
            System.Random altControlledV2GenARng = new System.Random((randSeed + 96) * 2);
            System.Random altControlledV2GenBRng = new System.Random((randSeed + 112) * 2);
            System.Random trueRandom = new System.Random(System.DateTime.Now.Millisecond);

            float lastAToggle = 0f;
            float lastBToggle = 0f;
            float lastANote = 0f;
            float lastBNote = 0f;
            float lastAToggleDeny = 0f;
            float lastBToggleDeny = 0f;
            int intermediaryAGreys = 0;
            int intermediaryBGreys = 0;
            bool altA = false;
            bool altB = false;

            Logger.log.Debug("Custom beatmap generated, now modifying via fancy rules...");

            for (int m = 0; m < customBeatmap.CustomBeatmapObjects.Count; m++)
            {

             //   CustomBeatmapObject cm = customBeatmap.CustomBeatmapObjects[m];

            }

            Logger.log.Debug("Tailored ? " + customBeatmap.isTailored);

            bool checkExtended = mode.mapOptions.extendedSlashMonochrome || mode.mapOptions.extendedSlashRemoved;
            bool checkDoubles = checkExtended || mode.mapOptions.doubleHitsDots || mode.mapOptions.doubleHitsMonochrome || mode.mapOptions.doubleHitsDuochrome || mode.mapOptions.doubleHitsRemoved || mode.randomizationOptions.randomDuochromeBlocksAvoidDoubles;

            if (checkDoubles) Logger.log.Debug("Checking doubles...");
            if (checkDoubles) Logger.log.Debug("Checking extended slashes...");

            //int i = customBeatmap.CustomBeatmapObjects.Count;
            int i = 0;
            CustomBeatmapObject co;
            bool erase;
            //while (i > 0) {
            while (i < customBeatmap.CustomBeatmapObjects.Count)
            {
                erase = false;
                //if (i >= customBeatmap.CustomBeatmapObjects.Count) continue;
                co = customBeatmap.CustomBeatmapObjects[i];
              //  Logger.log.Info($"Object: {co.Data is NoteData} {co is CustomBeatmapBomb}");
                if (co.Data is NoteData note && !(co is CustomBeatmapBomb))
                {
                    if (note.colorType == ColorType.ColorA || note.colorType == ColorType.ColorB)
                    {

                        ref float lastSameNoteTime = ref GetAltColourTime(ref lastANote, ref lastBNote, note.colorType);

                        //Randomizations

                        if (mode.randomizationOptions.randomRemoveChance > 0)
                        {
                            if (removeRandom.NextDouble() < mode.randomizationOptions.randomRemoveChance) erase = true;
                        }

                        if (!erase)
                        {
                            if (mode.randomizationOptions.baseColourRandomizationStyle > 0 && !customBeatmap.isTailored)
                            {
                                ColorType targetType;
                                switch (mode.randomizationOptions.baseColourRandomizationStyle)
                                {
                                    case 1: //Simple
                                        note.TransformNoteAOrBToRandomType();
                                        break;
                                    case 2: //Controlled
                                        if ((note.timeToNextColorNote != 0 && note.timeToNextColorNote < 0.3f) && (note.timeToPrevColorNote != 0 && note.timeToPrevColorNote < 0.3f))
                                        {
                                            System.Random timeRand = new System.Random(Mathf.FloorToInt(note.time * 2));
                                            if (timeRand.NextDouble() < 0.5f)
                                            { //50% chance of dual colour
                                                targetType = timeRand.NextDouble() < 0.5f ? ColorType.ColorA : ColorType.ColorB;
                                                if (note.colorType != targetType) note.SwitchNoteColorType();
                                            }
                                            else
                                            {
                                                if (timeRand.NextDouble() < 0.5f)
                                                { //25% (50% of 50%) for full invert
                                                    note.SwitchNoteColorType();
                                                } //25% of being normal
                                            }
                                        }
                                        else
                                        {
                                            note.TransformNoteAOrBToRandomType();
                                        }
                                        break;
                                    case 3: //Intense
                                        targetType = intenseRandom.NextDouble() < 0.5f ? ColorType.ColorA : ColorType.ColorB;
                                        if (note.colorType != targetType) note.SwitchNoteColorType();
                                        break;
                                    case 4: //True
                                        targetType = trueRandom.NextDouble() < 0.5f ? ColorType.ColorA : ColorType.ColorB;
                                        if (note.colorType != targetType) note.SwitchNoteColorType();
                                        break;
                                }
                            }

                            if (mode.mapOptions.mirrorColour) note.SwitchNoteColorType();
                            if (mode.mapOptions.mirrorDirection && note.time > 1) note.ChangeNoteCutDirection(note.cutDirection.OppositeDirection());
                            if (mode.mapOptions.mirrorPosition && note.time > 1) note.Mirror(4);

                            if (mode.randomizationOptions.randomDotsChance > 0 && !customBeatmap.isTailored)
                            {
                                if (dotsRandom.NextDouble() < mode.randomizationOptions.randomDotsChance) note.SetNoteToAnyCutDirection();
                            }

                            if (mode.randomizationOptions.randomMirrorChance > 0 && !customBeatmap.isTailored && note.time > 1)
                            {
                                System.Random mirrorRandom = new System.Random((randSeed + 2) * 2 * Mathf.FloorToInt(note.time));
                                if (mirrorRandom.NextDouble() < mode.randomizationOptions.randomMirrorChance)
                                {
                                    note.SwitchNoteColorType();
                                    note.Mirror(4);
                                    note.ChangeNoteCutDirection(note.cutDirection.OppositeDirection());
                                }
                            }

                            if (mode.randomizationOptions.randomMirrorDirectionChance > 0 && !customBeatmap.isTailored)
                            {
                                System.Random mirrorDirectionRandom = new System.Random((randSeed + 8) * 2 * Mathf.FloorToInt(note.time));
                                if (mirrorDirectionRandom.NextDouble() < mode.randomizationOptions.randomMirrorDirectionChance) note.ChangeNoteCutDirection(note.cutDirection.OppositeDirection());
                            }
                            if (mode.randomizationOptions.randomMirrorPositionChance > 0 && !customBeatmap.isTailored && note.time > 1)
                            {
                                System.Random mirrorPositionRandom = new System.Random((randSeed + 4) * 2 * Mathf.FloorToInt(note.time));
                                if (mirrorPositionRandom.NextDouble() < mode.randomizationOptions.randomMirrorPositionChance) note.Mirror(4);
                            }

                        }

                        bool isDouble = false;

                        //Doubles
                        if (checkDoubles)
                        {
                            CustomBeatmapNote[] doublesNotes = CustomBeatmap.PurgeNonNotes(customBeatmap.GetCustomBeatmapObjectsByTime(note.time));

                            if (doublesNotes.Length > 1)
                            {
                                isDouble = true;

                                //Extended Slashes
                                if (checkExtended)
                                {
                                    List<CustomBeatmapNote> extendedRemoval = new List<CustomBeatmapNote>();
                                    for (int m = 0; m < doublesNotes.Length; m++)
                                    {
                                        List<CustomBeatmapNote> extended = GetExtendedSlash(doublesNotes[m], doublesNotes);

                                        if (extended.Count < 2) continue;

                                        Logger.log.Debug("Extended slash found, note count: " + extended.Count);
                                        //We have our extended list
                                        //Lettuce do stuff with it.  The last object added to extended will be our base.
                                        if (mode.mapOptions.extendedSlashRemoved)
                                        {
                                            for (int r = 0; r < extended.Count - 1; r++) extendedRemoval.Add(extended[r]);
                                        }
                                        else if (mode.mapOptions.extendedSlashMonochrome)
                                        {
                                            for (int r = 0; r < extended.Count - 1; r++) extended[r].ColorType = extended[extended.Count - 1].ColorType; //TODO fix
                                        }
                                    }

                                    foreach (CustomBeatmapNote extendedRemovalNote in extendedRemoval)
                                    {
                                        if (extendedRemovalNote.Note == note) erase = true;
                                        else EraseObject(customBeatmap.CustomBeatmapObjects, extendedRemovalNote);
                                    }
                                }

                                try
                                {

                                    //General Doubles
                                    if (mode.mapOptions.doubleHitsRemoved)
                                    {
                                        System.Random sr = new System.Random(Mathf.FloorToInt(note.time * 8000));
                                        int survivor = sr.Next(0, doublesNotes.Length);
                                        for (int k = 0; k < doublesNotes.Length; k++)
                                        {
                                            if (k == survivor) continue;
                                            if (doublesNotes[k].Note == note) erase = true;
                                            else EraseObject(customBeatmap.CustomBeatmapObjects, doublesNotes[k]);
                                        }
                                        doublesNotes[survivor].Note.SetProperty("startNoteLineLayer", doublesNotes[survivor].Note.noteLineLayer);
                                        doublesNotes[survivor].Note.SetProperty("flipLineIndex", doublesNotes[survivor].Note.lineIndex);
                                    }
                                    else
                                    {
                                        if (mode.mapOptions.doubleHitsDots)
                                        {
                                            for (int k = 0; k < doublesNotes.Length; k++) doublesNotes[k].Note.SetNoteToAnyCutDirection();
                                        }
                                        if (mode.mapOptions.doubleHitsMonochrome)
                                        {
                                            System.Random sr = new System.Random(Mathf.FloorToInt(note.time * 8000));
                                            ColorType targetType = doublesNotes[sr.Next(0, doublesNotes.Length)].ColorType;
                                            for (int k = 0; k < doublesNotes.Length; k++) doublesNotes[k].ColorType = targetType;
                                        }
                                        else if (mode.mapOptions.doubleHitsDuochrome && doublesNotes.Length == 2)
                                        {
                                            if (doublesNotes[0].ColorType == doublesNotes[1].ColorType) doublesNotes[1].Note.SwitchNoteColorType();
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    Logger.log.Debug("Error with doubles management!");
                                    Logger.log.Debug(ex);
                                }

                            }

                        }

                        if (mode.mapOptions.allNotesDots) note.SetNoteToAnyCutDirection();

                        if (mode.colourOptions.monochrome && !erase)
                        {
                            ColorType targetType = mode.mapOptions.mirrorColour || (mode.colourOptions.invertColoursOnMirror && playerSettings.leftHanded) ? ColorType.ColorA : ColorType.ColorB;
                            if (note.colorType != targetType)
                            {
                                note.SwitchNoteColorType();
                            }
                        }

                        //Alt colours
                        if (!erase)
                        {

                            NoteBehaviour behaviour = null;

                            if (mode.colourOptions.alternateColours)
                            {

                                if (mode.randomizationOptions.altColourRandomizationStyle > 0 && !customBeatmap.isTailored)
                                {
                                    switch (mode.randomizationOptions.altColourRandomizationStyle)
                                    {
                                        case 1: //Simple
                                            if (altColourRandom.NextDouble() < mode.randomizationOptions.altColourRandomizationFactor)
                                            {
                                                behaviour = ApplyAlternateColour(co as CustomBeatmapNote);
                                            }
                                            break;
                                        case 2: //Controlled
                                            System.Random timeRandom = new System.Random(Mathf.CeilToInt(note.time * (note.colorType == ColorType.ColorA ? 1000f : 2000f)));
                                            ref float lastTime = ref GetAltColourTime(ref lastAToggle, ref lastBToggle, note.colorType);
                                            ref int intermediaryGreys = ref GetIntermediaryGreys(ref intermediaryAGreys, ref intermediaryBGreys, note.colorType);
                                            if (intermediaryGreys > 0)
                                            {
                                                intermediaryGreys--;
                                                behaviour = ApplyGreyColour(co as CustomBeatmapNote);
                                                lastTime = note.time;
                                                break;
                                            }
                                            ref float toggleDeny = ref GetAltColourTime(ref lastAToggleDeny, ref lastBToggleDeny, note.colorType);
                                            ref bool lastToggle = ref GetAltColourToggleBool(ref altA, ref altB, note.colorType);
                                            float diff = Mathf.Abs(note.time - lastTime);
                                            if (Mathf.Abs(note.time - lastSameNoteTime) < 1f) toggleDeny += note.time - lastSameNoteTime;
                                            else toggleDeny = 0f;
                                            if (lastToggle) behaviour = ApplyAlternateColour(co as CustomBeatmapNote);
                                            if (diff > 1)
                                            {
                                                if (timeRandom.NextDouble() < mode.randomizationOptions.altColourRandomizationFactor * (lastToggle ? mode.randomizationOptions.altColourRevertChanceFactor : 1) && (toggleDeny == 0 || toggleDeny >= 1.1f))
                                                {
                                                    lastToggle = !lastToggle;
                                                    intermediaryGreys = mode.randomizationOptions.altColourIntermediaryGreys;
                                                    lastTime = note.time;
                                                }
                                            }
                                            break;
                                        case 3: //Intense
                                            if (altColourRandom.NextDouble() < mode.randomizationOptions.altColourRandomizationFactor)
                                            {
                                                behaviour = ApplyAlternateColour(co as CustomBeatmapNote);
                                            }
                                            break;
                                        case 4: //True
                                            if (trueRandom.NextDouble() < mode.randomizationOptions.altColourRandomizationFactor)
                                            {
                                                behaviour = ApplyAlternateColour(co as CustomBeatmapNote);
                                            }
                                            break;
                                        case 5: //LegacyControlled
                                            if (new System.Random(Mathf.FloorToInt(note.time * (note.colorType == ColorType.ColorA ? 1000f : 2000f))).NextDouble() < mode.randomizationOptions.altColourRandomizationFactor)
                                            {
                                                behaviour = ApplyAlternateColour(co as CustomBeatmapNote);
                                            }
                                            break;
                                    }
                                }

                                if (mode.randomizationOptions.randomGreyBlocksChance > 0 && behaviour == null && !customBeatmap.isTailored)
                                {
                                    if (greyRandom.NextDouble() < mode.randomizationOptions.randomGreyBlocksChance)
                                    {
                                        behaviour = ApplyGreyColour(co as CustomBeatmapNote);
                                    }
                                }

                                if (mode.randomizationOptions.randomSuperBlocksChance > 0 && behaviour == null && !customBeatmap.isTailored)
                                {
                                    if (superRandom.NextDouble() < mode.randomizationOptions.randomSuperBlocksChance)
                                    {
                                        behaviour = ApplySuperColour(co as CustomBeatmapNote);
                                    }
                                }

                                if (mode.randomizationOptions.randomDuochromeBlocksChance > 0 && behaviour == null && !customBeatmap.isTailored)
                                {
                                    if ((!isDouble || !mode.randomizationOptions.randomDuochromeBlocksAvoidDoubles) && duochromeRandom.NextDouble() < mode.randomizationOptions.randomDuochromeBlocksChance)
                                    {
                                        behaviour = ApplyDuochromeColour(co as CustomBeatmapNote);
                                    }
                                }
                          //      Logger.log.Info($"Nightmare Check: {mode.specialOptions.nightmare} {behaviour == null}");
                                if (mode.specialOptions.nightmare && behaviour == null && !customBeatmap.isTailored)
                                {
                                    if (NightmareBehaviour.ConvertToGrey(note)) behaviour = ApplyGreyColour(co as CustomBeatmapNote);
                                }
                            }

                            if (mode.randomizationOptions.randomBidirectionalChance > 0 && !customBeatmap.isTailored)
                            {
                                if (bidirectionalRandom.NextDouble() < mode.randomizationOptions.randomBidirectionalChance && !(note.cutDirection == NoteCutDirection.Any))
                                {
                                   // behaviour = ApplyBidirectionalCut(co as CustomBeatmapNote);
                                }
                            }

                            if (mode.randomizationOptions.randomDeflectChance > 0 && !customBeatmap.isTailored)
                            {
                                if (deflectRandom.NextDouble() < mode.randomizationOptions.randomDeflectChance)
                                {
                            //        behaviour = ApplyDeflectCut(co as CustomBeatmapNote);
                                }
                            }

                        }

                        lastSameNoteTime = note.time;

                    }
                    else if (note.colorType == ColorType.None)
                    {

                        if (customBeatmap.isTailored)
                        {
                            //Note flip flag
                            List<CustomBeatmapObject> flaggedObjects = customBeatmap.GetCustomBeatmapObjectsByTime(note.time);
                            for (int j = 0; j < flaggedObjects.Count; j++)
                            {
                                if (flaggedObjects[j] is CustomBeatmapNote flaggedNote)
                                {
                                    if (flaggedNote.Note.lineIndex == note.lineIndex && flaggedNote.Note.noteLineLayer == note.noteLineLayer)
                                    {
                                        if (flaggedObjects[j].Data is NoteData)
                                        {
                                            if (flaggedNote.ColorType == ColorType.ColorA || flaggedNote.ColorType == ColorType.ColorB)
                                            {
                                                switch (note.cutDirection)
                                                {
                                                    case NoteCutDirection.Up: //Bomb is up-slash
                                                        ApplyGreyColour(flaggedNote); //Note becomes monochrome
                                                        break;
                                                    case NoteCutDirection.Left: //Bomb is left-slash
                                                        ApplyBidirectionalCut(flaggedNote); //Note becomes bidirectional
                                                        break;
                                                    case NoteCutDirection.Right: //Bomb is right-slash
                                                        ApplyDuochromeColour(flaggedNote); //Note becomes duochrome
                                                        break;
                                                    case NoteCutDirection.DownRight: //Bomb is down-right-slash
                                                        ApplySuperColour(flaggedNote); //Note becomes garbage
                                                        break;
                                                    case NoteCutDirection.Down: //Bomb is down-slash
                                                        ApplyAlternateColour(flaggedNote); //Note becomes alternate (blue->green, red->magenta)
                                                        break;
                                                    case NoteCutDirection.UpRight: //Bomb is up-slash
                                                        ApplyDeflectCut(flaggedNote); //Note becomes deflect
                                                        break;
                                                }
                                                //if (note.cutDirection == NoteCutDirection.Up) ApplyGreyColour(flaggedNote.Note);
                                                //else if (note.cutDirection == NoteCutDirection.Down) ApplyAlternateColour(flaggedNote.Note);

                                                erase = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (note.time == 0) erase = true;

                    }
                }

                else
                if (co.Data is ObstacleData obstacle)
                {
                    //TODO obstacle stuff
                }

                if (erase) EraseObject(customBeatmap.CustomBeatmapObjects, co, ref i);

                i++;
            }


        }

        public static List<CustomBeatmapNote> GetExtendedSlash(CustomBeatmapNote note, CustomBeatmapNote[] doublesNotes)
        {
            List<CustomBeatmapNote> extended = new List<CustomBeatmapNote>();
            NoteCutDirection oppositeDir = GetOppositeCutDirection(note.Note.cutDirection);
            CustomBeatmapNote adjacent = GetNoteFromDirection(note.Note, oppositeDir, doublesNotes, true);
            while (adjacent != null)
            {
                extended.Add(adjacent);
                adjacent = GetNoteFromDirection(adjacent.Note, oppositeDir, doublesNotes, true);
            }
            extended.Add(note);
            adjacent = GetNoteFromDirection(note.Note, note.Note.cutDirection, doublesNotes, true);
            while (adjacent != null)
            {
                extended.Add(adjacent);
                adjacent = GetNoteFromDirection(adjacent.Note, note.Note.cutDirection, doublesNotes, true);
            }
            return extended;
        }

        public static CustomBeatmapNote GetNoteFromDirection(NoteData originating, NoteCutDirection direction, CustomBeatmapNote[] doublesNotes, bool onlyMatchingDirection)
        {
            int targetIndex = originating.lineIndex;
            int targetLayer = (int)originating.noteLineLayer;

            switch (direction)
            {
                case NoteCutDirection.Any:
                    return null;
                case NoteCutDirection.Down:
                    targetLayer -= 1;
                    break;
                case NoteCutDirection.Up:
                    targetLayer += 1;
                    break;
                case NoteCutDirection.Left:
                    targetIndex -= 1;
                    break;
                case NoteCutDirection.Right:
                    targetIndex += 1;
                    break;
                case NoteCutDirection.UpLeft:
                    targetIndex -= 1;
                    targetLayer += 1;
                    break;
                case NoteCutDirection.UpRight:
                    targetIndex += 1;
                    targetLayer += 1;
                    break;
                case NoteCutDirection.DownRight:
                    targetIndex += 1;
                    targetLayer -= 1;
                    break;
                case NoteCutDirection.DownLeft:
                    targetIndex -= 1;
                    targetLayer -= 1;
                    break;
            }

            CustomBeatmapNote note = GetNoteFromPosition(targetIndex, targetLayer, doublesNotes);
            if (note != null)
            {
                if (onlyMatchingDirection) return note.Note.cutDirection == direction ? note : null;
                return note;
            }
            return null;
        }

        public static NoteCutDirection GetOppositeCutDirection(NoteCutDirection dir)
        {
            switch (dir)
            {
                case NoteCutDirection.Any: return NoteCutDirection.Any;
                case NoteCutDirection.Down: return NoteCutDirection.Up;
                case NoteCutDirection.DownLeft: return NoteCutDirection.UpRight;
                case NoteCutDirection.DownRight: return NoteCutDirection.UpLeft;
                case NoteCutDirection.Left: return NoteCutDirection.Right;
                case NoteCutDirection.None: return NoteCutDirection.None;
                case NoteCutDirection.Right: return NoteCutDirection.Left;
                case NoteCutDirection.Up: return NoteCutDirection.Down;
                case NoteCutDirection.UpLeft: return NoteCutDirection.DownRight;
                case NoteCutDirection.UpRight: return NoteCutDirection.DownLeft;
            }
            return dir;
        }

        public static CustomBeatmapNote GetNoteFromPosition(int targetIndex, int targetLayer, CustomBeatmapNote[] doublesNotes)
        {
            if (targetIndex < 0 || targetIndex > 3 || targetLayer < 0 || targetLayer > 2) return null;
            for (int i = 0; i < doublesNotes.Length; i++)
            {
                if ((doublesNotes[i].Data as NoteData).lineIndex == targetIndex && (int)(doublesNotes[i].Note.noteLineLayer) == targetLayer) return doublesNotes[i];
            }
            return null;
        }

        public static void EraseObject(List<CustomBeatmapObject> customBeatmap, CustomBeatmapObject customBeatmapObject, ref int i)
        {
            i--;
            EraseObject(customBeatmap, customBeatmapObject);
        }

        public static void EraseObject(List<CustomBeatmapObject> customBeatmap, CustomBeatmapObject customBeatmapObject)
        {
            customBeatmap.Remove(customBeatmapObject);
        }

        public static NoteBehaviour ApplyAlternateColour(CustomBeatmapNote note)
        {
            NoteData data = note.Data as NoteData;
            NoteBehaviour temp = NoteBehaviour.GetNoteBehaviour(data);
            if (temp != null)
            {
                temp.ColourBehaviour = NoteBehaviour.NoteColourBehaviour.ALTERNATE;
                return temp;
            }
            else return NoteBehaviour.SetSpecialNote(data, new NoteBehaviour(NoteBehaviour.NoteColourBehaviour.ALTERNATE, NoteBehaviour.NoteCutBehaviour.NORMAL, data.colorType, data.time, data.cutDirection));
        }

        public static NoteBehaviour ApplyGreyColour(CustomBeatmapNote note)
        {
            NoteData data = note.Data as NoteData;
            NoteBehaviour temp = NoteBehaviour.GetNoteBehaviour(data);
            if (temp != null)
            {
                temp.ColourBehaviour = NoteBehaviour.NoteColourBehaviour.ANY;
                return temp;
            }
            else return NoteBehaviour.SetSpecialNote(data, new NoteBehaviour(NoteBehaviour.NoteColourBehaviour.ANY, NoteBehaviour.NoteCutBehaviour.NORMAL, data.colorType, data.time, data.cutDirection));
        }

        public static NoteBehaviour ApplyDuochromeColour(CustomBeatmapNote note)
        {
            NoteData data = note.Data as NoteData;
            NoteBehaviour temp = NoteBehaviour.GetNoteBehaviour(data);
            if (temp != null)
            {
                temp.ColourBehaviour = NoteBehaviour.NoteColourBehaviour.DUOCHROME;
                return temp;
            }
            else return NoteBehaviour.SetSpecialNote(data, new NoteBehaviour(NoteBehaviour.NoteColourBehaviour.DUOCHROME, NoteBehaviour.NoteCutBehaviour.NORMAL, data.colorType, data.time, data.cutDirection));
        }

        public static NoteBehaviour ApplySuperColour(CustomBeatmapNote note)
        {
            NoteData data = note.Data as NoteData;
            NoteBehaviour temp = NoteBehaviour.GetNoteBehaviour(data);
            if (temp != null)
            {
                temp.ColourBehaviour = NoteBehaviour.NoteColourBehaviour.SUPER;
                return temp;
            }
            else return NoteBehaviour.SetSpecialNote(data, new NoteBehaviour(NoteBehaviour.NoteColourBehaviour.SUPER, NoteBehaviour.NoteCutBehaviour.NORMAL, data.colorType, data.time, data.cutDirection));
        }

        public static NoteBehaviour ApplyBidirectionalCut(CustomBeatmapNote note)
        {
            NoteData data = note.Data as NoteData;
            NoteBehaviour temp = NoteBehaviour.GetNoteBehaviour(data);
            if (temp != null)
            {
                temp.CutBehaviour = NoteBehaviour.NoteCutBehaviour.BIDIRECTIONAL;
                return temp;
            }
            else return NoteBehaviour.SetSpecialNote(data, new NoteBehaviour(NoteBehaviour.NoteColourBehaviour.NORMAL, NoteBehaviour.NoteCutBehaviour.BIDIRECTIONAL, data.colorType, data.time, data.cutDirection));
        }

        public static NoteBehaviour ApplyDeflectCut(CustomBeatmapNote note)
        {
            NoteData data = note.Data as NoteData;
            NoteBehaviour temp = NoteBehaviour.GetNoteBehaviour(data);
            if (temp != null)
            {
                temp.CutBehaviour = NoteBehaviour.NoteCutBehaviour.DEFLECT;
                return temp;
            }
            else return NoteBehaviour.SetSpecialNote(data, new NoteBehaviour(NoteBehaviour.NoteColourBehaviour.NORMAL, NoteBehaviour.NoteCutBehaviour.DEFLECT, data.colorType, data.time, data.cutDirection));
        }


        public static ref int GetIntermediaryGreys(ref int AGreys, ref int BGreys, ColorType colorType)
        {
            if (colorType == ColorType.ColorA) return ref AGreys;
            else return ref BGreys;
        }

        public static ref float GetAltColourTime(ref float AFloat, ref float BFloat, ColorType colorType)
        {
            if (colorType == ColorType.ColorA) return ref AFloat;
            else return ref BFloat;
        }

        public static ref bool GetAltColourToggleBool(ref bool ABool, ref bool BBool, ColorType colorType)
        {
            if (colorType == ColorType.ColorA) return ref ABool;
            else return ref BBool;
        }

        public static ref T SwitchOnColorType<T>(ref T A, ref T B, ColorType colorType)
        {
            if (colorType == ColorType.ColorA) return ref A;
            else return ref B;
        }


    }
}
