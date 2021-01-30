using PaliBot.Numerics;
using System;
using System.Numerics;

namespace PaliBot.Zones
{
    public class SphericalZone : Zone
    {
        public float Radius { get; }

        public SphericalZone(RegionKey region, ZoneKey key, Vector3 center, float radius) : base(region, key, center)
        {
            Radius = radius;
        }

        public SphericalZone(RegionKey region, ZoneKey key, float x, float y, float z, float radius) : this(region, key, new Vector3(x, y, z), radius)
        {
        }

        public override bool IntersectedBy(Ray ray)
        {
            var m = ray.Origin - Center;
            var b = Vector3.Dot(m, ray.Direction);
            var c = Vector3.Dot(m, m) - Radius * Radius;
            if (c > 0f && b > 0f) return false;
            var d = b * b - c;
            if (d < 0f) return false;
            return true;
        }

        public override bool Contains(Vector3 position)
        {
            return (position - Center).Length() <= Radius;
        }
    }
}
