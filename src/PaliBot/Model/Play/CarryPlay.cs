using PaliBot.Model.Frame;

namespace PaliBot.Model.Play
{
    public class CarryPlay : GamePlay
    {
        public IPlayer Player { get; }

        public CarryPlay(IPlayer player) : base(0)
        {
            Player = player;
        }

        public override string Describe()
        {
            return $"{Player.Name} is carrying the disc";
        }
    }
}
