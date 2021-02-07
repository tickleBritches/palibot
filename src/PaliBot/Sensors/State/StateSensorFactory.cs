using System;
using System.Collections.Generic;
using System.Text;

namespace PaliBot.Sensors.State
{
    public interface IStateSensorFactory
    {
        IStateSensor Create<T>() where T : IStateSensor, new();
    }

    public class StateSensorFactory : IStateSensorFactory
    {
        public IStateSensor Create<T>() where T : IStateSensor, new()
        {
            return new T();
        }
    }
}
