using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IPA.Utilities;

namespace NightmareStandalone.Extensions
{
    public class CustomBeatmapNote : CustomBeatmapObject
    {

        private NoteData _note;

        public NoteData Note
        {
            get { return _note; }
        }

        public ColorType ColorType
        {
            get { return _note.colorType; }
            set
            {
                if (_note.colorType != value)
                {
                    _note.SetProperty("colorType", _note.colorType.Opposite());
                }
            }
        }

        public CustomBeatmapNote(NoteData note, int id) : base(note, id)
        {
            this._note = note;
        }

    }
}
