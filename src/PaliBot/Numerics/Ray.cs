using System.Numerics;

namespace PaliBot.Numerics
{
    public class Ray
    {
        public Vector3 Origin { get; }
        public Vector3 Direction { get; }

        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction.Normalized();
        }

        public Ray(float x, float y, float z, float dx, float dy, float dz) : this(new Vector3(x, y, z), new Vector3(dx, dy, dz))
        {

        }
    }
}