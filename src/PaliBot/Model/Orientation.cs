using System.Numerics;

namespace PaliBot.Model
{
    public interface IOrientation
    {
        Vector3 Forward { get; }
        Vector3 Left { get; }
        Vector3 Up { get; }
    }

    public class Orientation : IOrientation
    {
        public Vector3 Forward { get; set; }
        public Vector3 Left { get; set; }
        public Vector3 Up { get; set; }
    }
}
