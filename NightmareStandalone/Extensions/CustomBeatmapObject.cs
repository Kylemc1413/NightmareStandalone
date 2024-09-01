using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightmareStandalone.Extensions
{
    public abstract class CustomBeatmapObject
    {

        private BeatmapObjectData _data;
        public int id { get; private set; }
        public BeatmapObjectData Data
        {
            get { return _data; }
        }

        public CustomBeatmapObject(BeatmapObjectData data, int id)
        {
            this._data = data;
            this.id = id;
        }

    }
}
