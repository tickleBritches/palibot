using System.Numerics;

namespace PaliBot.Model
{
    public interface IPose
    {
        IOrientation Orientation { get; }
        Vector3 Position { get; }
    }

    public class Pose : IPose
    {
        public Vector3 Position { get; set; }
        public Orientation Orientation { get; } = new Orientation();

        IOrientation IPose.Orientation => Orientation;
    }
}
