using PaliBot.Model;
using PaliBot.Model.Event;
using PaliBot.Model.Play;
using PaliBot.Sensors.Event;
using PaliBot.Sensors.Play;
using PaliBot.Sensors.State;
using PaliBot.Utility;
using System;
using System.Collections.Generic;

namespace PaliBot
{
    public class Announcer
    {
        private ISessionConverter _sessionConverter;
        
        internal IStateSensor[] _stateSensors;
        internal IEventSensor[] _eventSensors;
        internal IPlaySensor[] _playSensors;

        private List<GameEvent> _gameEvents;
        private List<GamePlay> _gamePlays;

        public event EventHandler<string> Announce;

        public Announcer() : this(new SessionConverter(), new StateSensorFactory(), new EventSensorFactory(), new PlaySensorFactory())
        {
            
        }

        internal Announcer(ISessionConverter sessionConverter, IStateSensorFactory stateSensorFactory, IEventSensorFactory eventSensorFactory, IPlaySensorFactory playSensorFactory)
        {
            _sessionConverter = sessionConverter;

            InitializeStateSensors(stateSensorFactory);
            InitializeEventSensors(eventSensorFactory);
            InitializePlaySensors(playSensorFactory);
        }

        private void InitializeStateSensors(IStateSensorFactory stateSensorFactory)
        {
            _stateSensors = new IStateSensor[]
            {
                stateSensorFactory.Create<PossessionStateSensor>()
            };
        }

        private void InitializeEventSensors(IEventSensorFactory eventSensorFactory)
        {
            _eventSensors = new IEventSensor[]
            {
                eventSensorFactory.Create<PossessionChangeEventSensor>(),
                eventSensorFactory.Create<ThrowEventSensor>()
            };

            foreach (var eventSensor in _eventSensors)
            {
                eventSensor.Initialize(_stateSensors);
            }

            _gameEvents = new List<GameEvent>(_eventSensors.Length);
        }

        private void InitializePlaySensors(IPlaySensorFactory playSensorFactory)
        {
            _playSensors = new IPlaySensor[]
            {
                playSensorFactory.Create<PossessionPlaySensor>(),
                playSensorFactory.Create<ThrowPlaySensor>()
            };

            foreach (var playSensor in _playSensors)
            {
                playSensor.Initialize(_stateSensors);
            }

            _gamePlays = new List<GamePlay>(_playSensors.Length);
        }

        public void Update(ISession session)
        {
            var frame = _sessionConverter.ToFrame(session);
            
            // update state sensors
            for (var i = 0; i < _stateSensors.Length; i++)
            {
                _stateSensors[i].Update(frame);
            }

            // update event sensors
            _gameEvents.Clear();

            for (var i = 0; i < _eventSensors.Length; i++)
            {
                var gameEvent = _eventSensors[i].Update(frame);
                if (gameEvent != null)
                {
                    _gameEvents.Add(gameEvent);
                }
            }

            // update play sensors
            _gamePlays.Clear();

            for (var i = 0; i < _playSensors.Length; i++)
            {
                var play = _playSensors[i].Update(frame, _gameEvents);
                if (play != null)
                {
                    _gamePlays.Add(play);
                }
            }

            // announce
            //TODO: for now, we'll just emit all plays

            for (var i = 0; i < _gamePlays.Count; i++)
            {
                Announce(this, _gamePlays[i].Describe());
            }
        }
    }
}
