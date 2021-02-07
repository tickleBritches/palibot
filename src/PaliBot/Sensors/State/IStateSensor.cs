using PaliBot.Model;

namespace PaliBot.Sensors.State
{
    public interface IStateSensor
    {
        void Update(IFrame frame);
    }
}
