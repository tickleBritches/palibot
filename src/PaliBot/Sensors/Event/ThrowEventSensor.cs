using PaliBot.Model.Event;
using PaliBot.Model.Frame;
using PaliBot.Sensors.State;
using System.Collections.Generic;
using System.Linq;

namespace PaliBot.Sensors.Event
{
    public class ThrowEventSensor : IEventSensor
    {
        internal const float SPEED_DELTA_THRESHOLD = 3f;

        private IPossessionStateSensor _possessionStateSensor;
        private IPlayer _lastPossessionPlayer = null;

        public void Initialize(IEnumerable<IStateSensor> stateSensors)
        {
            //TODO: handle possibility of required state sensors not existing more informatively
            _possessionStateSensor = stateSensors.OfType<IPossessionStateSensor>().Single();
        }

        public GameEvent Update(IFrame frame)
        {
            // If the last possession player no longer has possession and the disc is travelling
            // significantly faster than that player, we're guessing it was a throw.  We'll need to do
            // work here to eliminate false positives (loss of possession because of headbutts, etc.)

            try
            {
                if (_lastPossessionPlayer != null && _possessionStateSensor.Player == null)
                {
                    var deltaSpeed = (frame.Disc.Velocity - _lastPossessionPlayer.Velocity).Length();
                    if (deltaSpeed >= SPEED_DELTA_THRESHOLD)
                    {
                        return new ThrowEvent(frame, _lastPossessionPlayer);
                    }
                }
                return null;
            }
            finally
            {
                _lastPossessionPlayer = _possessionStateSensor.Player;
            }
        }
    }
}
