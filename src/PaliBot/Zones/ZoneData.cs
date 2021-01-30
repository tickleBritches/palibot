using System;
using System.Collections.Generic;
using System.Text;

namespace PaliBot.Zones
{
    public static  class ZoneData
    {
        public static IEnumerable<Zone> MapZones => _mapZones;

        private static IEnumerable<Zone> _mapZones = new Zone[]
        {
            // neutral zones
            new SphericalZone(RegionKey.Neutral, ZoneKey.Mid, 0f, 0f, 0f, 6f),

            // orange zones
            new AlignedCubeZone(RegionKey.Orange, ZoneKey.Goal, 0f, 0f, 36f, 3f, 3f, 1f),

            // blue zones
            new AlignedCubeZone(RegionKey.Blue, ZoneKey.Goal, 0f, 0f, -36f, 3f, 3f, 1f)
        };
    }
}
