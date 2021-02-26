using PaliBot.Model.Event;
using PaliBot.Model.Frame;
using PaliBot.Model.Play;
using PaliBot.Sensors.State;
using System.Collections.Generic;
using System.Linq;

namespace PaliBot.Sensors.Play
{
    public class PossessionPlaySensor : IPlaySensor
    {
        internal const float PICKUP_THRESHOLD = 0.25f;
        internal const float CARRY_THRESHOLD = 3f;

        private IPlayer _player = null;
        private HandSide? _hand = null;
        private PossessionStage _stage = PossessionStage.None;        
        private float? _startTime = null;

        private enum PossessionStage
        {
            None,
            New,
            Established,
            Carried
        }

        public void Initialize(IEnumerable<IStateSensor> stateSensors)
        {
            // don't need any state sensors
        }

        public GamePlay Update(IFrame frame, IEnumerable<GameEvent> gameEvents)
        {
            //TODO: fear this OfType<> lookup will be slow.  Need to rethink this whole mechanism.  event delegates? hash by type?

            // apply changes
            var possessionChangeEvent = gameEvents.OfType<PossessionChangeEvent>().FirstOrDefault();

            if (possessionChangeEvent != null)
            {
                if (possessionChangeEvent.Player != null)
                {
                    _startTime = frame.Match.GameClock;
                    _stage = PossessionStage.New;
                }
                else
                {
                    _startTime = null;
                    _stage = PossessionStage.None;
                }

                _player = possessionChangeEvent.Player;
                _hand = possessionChangeEvent.Hand;
            }

            var possessionTime = _startTime - frame.Match.GameClock;

            if (_stage == PossessionStage.New && possessionTime >= PICKUP_THRESHOLD)
            {
                _stage = PossessionStage.Established;
                return new GrabDiscPlay(_player, _hand.Value);
            }
            else if (_stage == PossessionStage.Established && possessionTime >= CARRY_THRESHOLD)
            {
                _stage = PossessionStage.Carried;
                return new CarryPlay(_player);
            }

            return null;
        }
    }
}