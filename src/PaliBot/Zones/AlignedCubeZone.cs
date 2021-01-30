using PaliBot.Numerics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PaliBot.Zones
{
    public class AlignedCubeZone : Zone
    {
        public Vector3 Width;

        private Vector3 _min;
        private Vector3 _max;

        public AlignedCubeZone(RegionKey region, ZoneKey key, Vector3 center, Vector3 width) : base(region, key, center)
        {
            Width = width;

            _min = new Vector3(center.X - width.X / 2, center.Y - width.Y / 2, center.Z - width.Z / 2);
            _max = new Vector3(center.X + width.X / 2, center.Y + width.Y / 2, center.Z + width.Z / 2);
        }

        public AlignedCubeZone(RegionKey region, ZoneKey key, float x, float y, float z, float wx, float wy, float wz) : this(region, key, new Vector3(x, y, z), new Vector3(wx, wy, wz))
        {

        }

        public override bool IntersectedBy(Ray ray)
        {
            var invDirection = new Vector3(1.0f / ray.Direction.X, 1.0f / ray.Direction.Y, 1.0f / ray.Direction.Z);
            var txmin = (_min.X - ray.Origin.X) * invDirection.X;
            var txmax = (_max.X - ray.Origin.X) * invDirection.X;
            var tymin = (_min.Y - ray.Origin.Y) * invDirection.Y;
            var tymax = (_max.Y - ray.Origin.Y) * invDirection.Y;
            var tzmin = (_min.Z - ray.Origin.Z) * invDirection.Z;
            var tzmax = (_max.Z - ray.Origin.Z) * invDirection.Z;

            var tmin = Math.Max(Math.Max(Math.Min(txmin, txmax), Math.Min(tymin, tymax)), Math.Min(tzmin, tzmax));
            var tmax = Math.Min(Math.Min(Math.Max(txmin, txmax), Math.Max(tymin, tymax)), Math.Max(tzmin, tzmax));

            if (tmax < 0) // behind
                return false;

            if (tmin > tmax) // no intersection
                return false;

            return true;
        }

        public override bool Contains(Vector3 position)
        {
            return position.X >= _min.X && position.X <= _max.X
                && position.Y >= _min.Y && position.Y <= _max.Y
                && position.Z >= _min.Z && position.Z <= _max.Z;
        }
    }
}
