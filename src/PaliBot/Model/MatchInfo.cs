namespace PaliBot.Model
{
    public interface IMatchInfo
    {
        string SessionId { get; }
        string SessionIp { get; }
        GameStatus Status { get; }
        float GameClock { get; }

        PauseState PauseState { get; }
        float PausedTimer { get; }
        float UnpausingTimer { get; }
        ITeam PausedBy { get; }
        ITeam UnpausedBy { get; }
    }

    public class MatchInfo : IMatchInfo
    {
        public string SessionId { get; set; }
        public string SessionIp { get; set; }
        public GameStatus Status { get; set; }
        public float GameClock { get; set; }

        public PauseState PauseState { get; set; }
        public float PausedTimer { get; set; }
        public float UnpausingTimer { get; set; }
        public Team PausedBy { get; set; }
        public Team UnpausedBy { get; set; }

        // restart requests ??

        ITeam IMatchInfo.PausedBy => PausedBy;
        ITeam IMatchInfo.UnpausedBy => UnpausedBy;
    }
}
