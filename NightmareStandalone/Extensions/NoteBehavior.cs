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

    public class NoteBehaviour
    {
        private static ColorManager _colorManager;
        internal static ColorManager colorManager
        {
            get
            {

                if (_colorManager != null) return _colorManager;
                else
                {
                    _colorManager = Resources.FindObjectsOfTypeAll<NoteCutCoreEffectsSpawner>().LastOrDefault().GetField<ColorManager, NoteCutCoreEffectsSpawner>("_colorManager");
                    return _colorManager;
                }
            }

        }
        public enum NoteColourBehaviour
        {
            NORMAL,
            ALTERNATE,
            DUOCHROME,
            SUPER,
            ANY
        }

        public enum NoteCutBehaviour
        {
            NORMAL,
            BIDIRECTIONAL,
            DEFLECT,
        }

        public static NoteColourBehaviour overrideNoteColour = NoteColourBehaviour.NORMAL;
        public static NoteCutBehaviour overrideNoteCut = NoteCutBehaviour.NORMAL;

        private static Dictionary<NoteData, NoteBehaviour> specialNotes = new Dictionary<NoteData, NoteBehaviour>();

        public static void ClearSpecialNotes()
        {
            overrideNoteColour = NoteColourBehaviour.NORMAL;
            overrideNoteCut = NoteCutBehaviour.NORMAL;
            specialNotes.Clear();
        }

        public static NoteBehaviour SetSpecialNote(NoteData note, NoteBehaviour behaviour)
        {
            if (specialNotes.ContainsKey(note))
            {
                specialNotes.Remove(note);
            }
            specialNotes.Add(note, behaviour);
            return behaviour;
        }

        public static void RemoveSpecialNote(NoteData note)
        {
            if (specialNotes.ContainsKey(note)) specialNotes.Remove(note);
        }

        public static NoteBehaviour GetNoteBehaviour(NoteData note)
        {
            if (specialNotes.ContainsKey(note))
            {
                if (specialNotes.TryGetValue(note, out NoteBehaviour behaviour)) return behaviour;
            }
            return null;
        }

        public static NoteBehaviour GetNoteBehaviour(NoteControllerBase note, bool initialSpawn = false)
        {

            if (initialSpawn)
            {
                if (overrideNoteColour != NoteColourBehaviour.NORMAL || overrideNoteCut != NoteCutBehaviour.NORMAL)
                {
                    NoteBehaviour overridden = SetSpecialNote(note.noteData, new NoteBehaviour(overrideNoteColour, overrideNoteCut, note.noteData.colorType, note.noteData.time, note.noteData.cutDirection));
                }
            }

            if (specialNotes.ContainsKey(note.noteData))
            {
                if (specialNotes.TryGetValue(note.noteData, out NoteBehaviour behaviour)) return behaviour;
            }
            return null;
        }

        public static Color GetColor(NoteController noteController)
        {
            NoteBehaviour noteBehaviour = NoteBehaviour.GetNoteBehaviour(noteController);
            if (noteBehaviour != null)
            {
                return GetColor(noteBehaviour.ColourBehaviour, noteController.noteData.colorType, noteController.noteData.time);
            }
            else
            {
                return GetColor(NoteColourBehaviour.NORMAL, noteController.noteData.colorType, noteController.noteData.time);
            }
        }

        public static Color GetColor(NoteColourBehaviour colourBehaviour, ColorType colorType, float time)
        {
            switch (colourBehaviour)
            {
                case NoteColourBehaviour.NORMAL: return colorManager.ColorForType(colorType);
                case NoteColourBehaviour.ANY: return Config.MonoChromeColor;
            }
            return Color.clear;
        }

        private NoteColourBehaviour _colourBehaviour;
        private NoteCutBehaviour _noteCutBehaviour;
        private Color colour;

        public NoteColourBehaviour ColourBehaviour
        {
            get { return _colourBehaviour; }
            set { _colourBehaviour = value; }
        }

        public NoteCutBehaviour CutBehaviour
        {
            get { return _noteCutBehaviour; }
            set { _noteCutBehaviour = value; }
        }

        public Color Colour
        {
            get { return colour; }
        }

        public NoteBehaviour(NoteColourBehaviour colourBehaviour, NoteCutBehaviour cutBehaviour, ColorType colorType, float time, NoteCutDirection cutDirection)
        {

            if (cutDirection == NoteCutDirection.Any)
            {
                if (cutBehaviour == NoteCutBehaviour.BIDIRECTIONAL) cutBehaviour = NoteCutBehaviour.NORMAL;
                if (cutBehaviour == NoteCutBehaviour.DEFLECT) cutBehaviour = NoteCutBehaviour.NORMAL;
            }

            this._colourBehaviour = colourBehaviour;
            this._noteCutBehaviour = cutBehaviour;
            this.colour = GetColor(colourBehaviour, colorType, time);
        }

        private int lastCutID = -1;

    }

}
