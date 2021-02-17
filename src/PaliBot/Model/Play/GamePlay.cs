namespace PaliBot.Model.Play
{
    public abstract class GamePlay
    {
        public int Priority { get; }

        public GamePlay(int priority)
        {
            Priority = priority;
        }

        public abstract string Describe();
    }
}
