using PaliBot.Model.Frame;

namespace PaliBot.Sensors.State
{
    public interface IStateSensor
    {
        void Update(IFrame frame);
    }
}
