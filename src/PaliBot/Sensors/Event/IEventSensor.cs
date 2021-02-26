using PaliBot.Model.Event;
using PaliBot.Model.Frame;
using PaliBot.Sensors.State;
using System.Collections.Generic;

namespace PaliBot.Sensors.Event
{
    public interface IEventSensor
    {
        void Initialize(IEnumerable<IStateSensor> stateSensors);
        GameEvent Update(IFrame frame);
    }
}
