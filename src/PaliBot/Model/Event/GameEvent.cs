using PaliBot.Model.Frame;

namespace PaliBot.Model.Event
{
    public abstract class GameEvent
    {
        public IFrame Frame { get; }

        public GameEvent(IFrame frame)
        {
            Frame = frame;
        }
    }
}
