using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightmareStandalone.Extensions
{
    public class CustomBeatmapBomb : CustomBeatmapObject
    {

        private NoteData _note;

        public BeatmapObjectData Note
        {
            get { return _note; }
        }

        public CustomBeatmapBomb(NoteData note, int id) : base(note, id)
        {
            this._note = note;
        }

    }
}
