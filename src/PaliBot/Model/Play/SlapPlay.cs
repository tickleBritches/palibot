using PaliBot.Model.Frame;

namespace PaliBot.Model.Play
{
    public class SlapPlay : GamePlay
    {
        public IPlayer Player { get; }

        public SlapPlay(IPlayer player) : base(0)
        {
            Player = player;
        }

        public override string Describe()
        {
            return $"{Player.Name} slaps the disc";
        }
    }
}
