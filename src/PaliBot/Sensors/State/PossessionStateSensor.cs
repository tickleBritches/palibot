using PaliBot.Model.Frame;
using System.Numerics;

namespace PaliBot.Sensors.State
{
    public interface IPossessionStateSensor: IStateSensor
    {
        IPlayer Player { get; }
        HandSide? Hand { get; }
    }

    /// <summary>
    /// This sensor attempts to identify when a player is actually holding the disc.  This is necessary
    /// as the session date coming from the Echo API will continue to report possession for a few seconds
    /// after a player is no longer actually holding the disc.  It seems to equate more to what color the
    /// disc is showing as in game.  Currently the approach here is to check if the disc position is 
    /// actually close to the session-reported player's hands
    /// </summary>
    public class PossessionStateSensor : IPossessionStateSensor
    {
        internal const float MIN_HAND_DISTANCE = 0.2f;
        internal const float MAX_HAND_DISTANCE = 0.5f;

        public IPlayer Player { get; private set; }
        public HandSide? Hand { get; private set; }

        public void Update(IFrame frame)
        {
            if (frame.Match.Status != GameStatus.Playing || frame.LastPossessionPlayer == null)
            {
                Player = null;
                Hand = null;
                return;
            }

            if (frame.Disc.Velocity == Vector3.Zero)
            {
                var lhandDistance = (frame.Disc.Pose.Position - frame.LastPossessionPlayer.LeftHand.Position).Length();
                var rhandDistance = (frame.Disc.Pose.Position - frame.LastPossessionPlayer.RightHand.Position).Length();

                Player = frame.LastPossessionPlayer;
                Hand = lhandDistance < rhandDistance ? HandSide.Left : HandSide.Right;
            }
            else
            {
                Player = null;
                Hand = null;
            }
        }        
    }
}
