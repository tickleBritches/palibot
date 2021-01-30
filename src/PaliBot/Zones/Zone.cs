using PaliBot.Numerics;
using System.Numerics;

namespace PaliBot.Zones
{
    public abstract class Zone
    {
        public RegionKey Region { get; }
        public ZoneKey Key { get; }

        public Vector3 Center { get; }

        public Zone(RegionKey region, ZoneKey key, Vector3 center)
        {
            Region = region;
            Key = key;
            Center = center;
        }

        public abstract bool IntersectedBy(Ray ray);

        public abstract bool Contains(Vector3 position);
    }
}
