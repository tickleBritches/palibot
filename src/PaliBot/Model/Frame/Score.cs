namespace PaliBot.Model.Frame
{
    public interface IScore
    {
        float DiscSpeed { get; }
        float Distance { get; }
        ITeam Team { get; }
        IPlayer ScoredBy { get; }
        IPlayer AssistedBy { get; }
        int Points { get; }
        GoalType GoalType { get; }
    }

    public class Score : IScore
    {
        public float DiscSpeed { get; set; }
        public float Distance { get; set; }
        public Team Team { get; set; }
        public Player ScoredBy { get; set; }
        public Player AssistedBy { get; set; }
        public int Points { get; set; }
        public GoalType GoalType { get; set; }

        ITeam IScore.Team => Team;
        IPlayer IScore.ScoredBy => ScoredBy;
        IPlayer IScore.AssistedBy => AssistedBy;
    }
}
