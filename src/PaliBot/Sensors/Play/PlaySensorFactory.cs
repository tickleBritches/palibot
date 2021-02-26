namespace PaliBot.Sensors.Play
{
    public interface IPlaySensorFactory
    {
        IPlaySensor Create<T>() where T : IPlaySensor, new();
    }

    public class PlaySensorFactory : IPlaySensorFactory
    {
        public IPlaySensor Create<T>() where T : IPlaySensor, new()
        {
            return new T();
        }
    }
}
