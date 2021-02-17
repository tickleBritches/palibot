using PaliBot.Model.Event;
using PaliBot.Model.Frame;
using PaliBot.Sensors.State;
using System.Collections.Generic;
using System.Linq;

namespace PaliBot.Sensors.Event
{
    public class PossessionChangeEventSensor : IEventSensor
    {
        internal IPossessionStateSensor _possessionStateSensor;

        private IPlayer _player = null;
        private HandSide? _hand = null;

        public void Initialize(IEnumerable<IStateSensor> stateSensors)
        {
            //TODO: handle possibility of required state sensors not existing more informatively
            _possessionStateSensor = stateSensors.OfType<IPossessionStateSensor>().Single();
        }

        public GameEvent Update(IFrame frame)
        {
            if (!_possessionStateSensor.Player.IsSame(_player))
            {
                _player = _possessionStateSensor.Player;
                _hand = _possessionStateSensor.Hand;                

                return new PossessionChangeEvent(frame, _player, _hand);
            }
            return null;
        }
    }
}
