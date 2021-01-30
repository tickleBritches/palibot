using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PaliBot.Zones
{
    public static class ZoneUtil
    {
        public static Zone Locate(Vector3 position)
        {
            var nearestZone = (Zone)null;
            var nearestZoneDistance = float.MaxValue;

            foreach (var zone in ZoneData.MapZones)
            {
                if (!zone.Contains(position))
                    continue;

                var distance = (position - zone.Center).Length();
                if (distance < nearestZoneDistance)
                {
                    nearestZone = zone;
                    nearestZoneDistance = distance;
                }
            }

            return nearestZone;
        }
    }
}
