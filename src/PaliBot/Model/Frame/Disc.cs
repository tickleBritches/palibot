using System.Numerics;

namespace PaliBot.Model.Frame
{
    public interface IDisc
    {
        IPose Pose { get; }
        Vector3 Velocity { get; }
        int BounceCount { get; }
    }

    public class Disc : IDisc
    {
        public Pose Pose { get; } = new Pose();
        public Vector3 Velocity { get; set; }
        public int BounceCount { get; set; }

        IPose IDisc.Pose => Pose;
    }
}
