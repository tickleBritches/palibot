using PaliBot.Model;
using PaliBot.Sensors.State;
using PaliBot.Utility;

namespace PaliBot
{
    public class Announcer
    {
        private ISessionConverter _sessionConverter;
        internal IStateSensor[] _stateSensors;

        public Announcer() : this(new SessionConverter(), new StateSensorFactory())
        {
            
        }

        internal Announcer(ISessionConverter sessionConverter, IStateSensorFactory stateSensorFactory)
        {
            _sessionConverter = sessionConverter;

            _stateSensors = new IStateSensor[]
            {
                stateSensorFactory.Create<PossessionStateSensor>()
            };
        }

        public void Update(ISession session)
        {
            var frame = _sessionConverter.ToFrame(session);
            
            for (var i = 0; i < _stateSensors.Length; i++)
            {
                _stateSensors[i].Update(frame);
            }
        }
    }
}
