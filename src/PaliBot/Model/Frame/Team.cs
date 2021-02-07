using System.Collections.Generic;

namespace PaliBot.Model.Frame
{
    public interface ITeam
    {
        TeamColor Color { get; }
        string Name { get; }
        IStats Stats { get; }
        bool HasPossesion { get; }
        IReadOnlyList<IPlayer> Players { get; }
    }

    public class Team : ITeam
    {
        public TeamColor Color { get; set; }
        public string Name { get; set; }
        public Stats Stats { get; } = new Stats();
        public bool HasPossesion { get; set; }
        public List<Player> Players { get; } = new List<Player>();

        IReadOnlyList<IPlayer> ITeam.Players => Players;
        IStats ITeam.Stats => Stats;

    }
}
