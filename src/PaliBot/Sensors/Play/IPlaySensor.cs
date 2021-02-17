using PaliBot.Model.Event;
using PaliBot.Model.Frame;
using PaliBot.Model.Play;
using PaliBot.Sensors.State;
using System.Collections.Generic;

namespace PaliBot.Sensors.Play
{
    public interface IPlaySensor
    {
        void Initialize(IEnumerable<IStateSensor> stateSensors);

        GamePlay Update(IFrame frame, IEnumerable<GameEvent> gameEvents);
    }
}
