using PaliBot.Sensors.State;
using System.Collections.Generic;

namespace PaliBot
{
    public class Announcer
    {
        private StateSensor[] _stateSensors;

        public Announcer()
        {
            _stateSensors = new StateSensor[]
            {
                new PossessionStateSensor()
            };
        }

        public void Update(Frame frame)
        {
            for (var i = 0; i < _stateSensors.Length; i++)
            {
                _stateSensors[i].Update(frame);
            }
        }
    }
}
