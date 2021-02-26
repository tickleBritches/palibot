namespace PaliBot.Sensors.Event
{
    public interface IEventSensorFactory
    {
        IEventSensor Create<T>() where T : IEventSensor, new();
    }

    public class EventSensorFactory : IEventSensorFactory
    {
        public IEventSensor Create<T>() where T : IEventSensor, new()
        {
            return new T();
        }
    }
}
