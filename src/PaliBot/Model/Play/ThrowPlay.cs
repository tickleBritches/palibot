using PaliBot.Model.Frame;

namespace PaliBot.Model.Play
{
    public class ThrowPlay : GamePlay
    {
        public IPlayer Player { get; }

        public ThrowPlay(IPlayer player) : base(0)
        {
            Player = player;
        }

        public override string Describe()
        {
            return $"{Player.Name} throws the disc";
        }
    }
}
