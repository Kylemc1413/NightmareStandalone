using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightmareStandalone.Extensions
{
    public class CustomBeatmapBarrier : CustomBeatmapObject
    {

        private ObstacleData _obstacle;

        public BeatmapObjectData Obstacle
        {
            get { return _obstacle; }
        }

        public CustomBeatmapBarrier(ObstacleData obstacle, int id) : base(obstacle, id)
        {
            this._obstacle = obstacle;
        }

    }
}
