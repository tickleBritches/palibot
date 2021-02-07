using PaliBot.Model.Frame;

namespace PaliBot.Sensors.State
{
    /// <summary>
    /// This sensor attempts to identify when a player is actually holding the disc.  This is necessary
    /// as the session date coming from the Echo API will continue to report possession for a few seconds
    /// after a player is no longer actually holding the disc.  It seems to equate more to what color the
    /// disc is showing as in game.  Currently the approach here is to check if the disc position is 
    /// actually close to the session-reported player's hands
    /// </summary>
    public class PossessionStateSensor : IStateSensor
    {
        internal const float MAX_HAND_DISTANCE = 0.25f;

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

            var lhandDistance = (frame.Disc.Pose.Position - frame.LastPossessionPlayer.LeftHand.Position).Length();
            var rhandDistance = (frame.Disc.Pose.Position - frame.LastPossessionPlayer.RightHand.Position).Length();

            var currentlyLeft = lhandDistance < MAX_HAND_DISTANCE && lhandDistance < rhandDistance;
            var currentlyRight = rhandDistance < MAX_HAND_DISTANCE && rhandDistance <= lhandDistance;

            if (currentlyLeft || currentlyRight)
            {
                Player = frame.LastPossessionPlayer;
                Hand = currentlyLeft ? HandSide.Left : HandSide.Right;
            }
            else
            {
                Player = null;
                Hand = null;
            }
        }
    }
}
