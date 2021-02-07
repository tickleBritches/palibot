namespace PaliBot.Model.Frame
{
    public interface IFrame
    {
        IClientInfo Client { get; }
        IMatchInfo Match { get; }

        ITeam BlueTeam { get; }
        ITeam OrangeTeam { get; }

        IDisc Disc { get; }
        IScore LastScore { get; } 
        IPlayer LastPossessionPlayer { get; }
    }

    public class Frame : IFrame
    {
        public ClientInfo Client { get; } = new ClientInfo();
        public MatchInfo Match { get; } = new MatchInfo();

        public Team BlueTeam { get; } = new Team();
        public Team OrangeTeam { get; } = new Team();

        public Disc Disc { get; } = new Disc();
        public Score LastScore { get; } = new Score();
        public Player LastPossessionPlayer { get; set; }

        IClientInfo IFrame.Client => Client;
        IMatchInfo IFrame.Match => Match;
        ITeam IFrame.BlueTeam => BlueTeam;
        ITeam IFrame.OrangeTeam => OrangeTeam;
        IDisc IFrame.Disc => Disc;
        IScore IFrame.LastScore => LastScore;
        IPlayer IFrame.LastPossessionPlayer => LastPossessionPlayer;
    }
}
