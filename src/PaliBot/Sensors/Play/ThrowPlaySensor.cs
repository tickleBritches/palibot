using PaliBot.Model.Event;
using PaliBot.Model.Frame;
using PaliBot.Model.Play;
using PaliBot.Sensors.State;
using System.Collections.Generic;
using System.Linq;

namespace PaliBot.Sensors.Play
{
    public class ThrowPlaySensor : IPlaySensor
    {
        internal const float SLAP_THRESHOLD_TIME = 0.25f;

        private IPlayer _player = null;
        private float? _possessionStartTime = null;

        public void Initialize(IEnumerable<IStateSensor> stateSensors)
        {
            // don't need any state sensors
        }

        public GamePlay Update(IFrame frame, IEnumerable<GameEvent> gameEvents)
        {
            //TODO: fear this OfType<> lookup will be slow.  Need to rethink this whole mechanism.  event delegates? hash by type? harness?

            var possessionChangeEvent = gameEvents.OfType<PossessionChangeEvent>().FirstOrDefault();
            var throwEvent = gameEvents.OfType<ThrowEvent>().FirstOrDefault();

            if (possessionChangeEvent?.Player != null)
            {
                _player = possessionChangeEvent.Player;
                _possessionStartTime = frame.Match.GameClock;
            }

            if (throwEvent != null)
            {
                try
                {
                    // yeah, yeah..some null case branch handling missing for this
                    if (throwEvent.Player.IsSame(_player) && (_possessionStartTime - frame.Match.GameClock) <= SLAP_THRESHOLD_TIME)
                    {
                        return new SlapPlay(throwEvent.Player);
                    }
                    else
                    {
                        return new ThrowPlay(throwEvent.Player);
                    }
                }
                finally
                {
                    _player = null;
                    _possessionStartTime = null;
                }
            }

            return null;
        }
    }
}