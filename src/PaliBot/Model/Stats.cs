namespace PaliBot.Model
{
    public interface IStats
    {
        int Assists { get; }
        int Blocks { get; }
        int Catches { get; }
        int Goals { get; }
        int Interceptions { get; }
        int Passes { get; }
        int Points { get; }
        float PossessionTime { get; }
        int Saves { get; }
        int ShotsTaken { get; }
        int Steals { get; }
        int Stuns { get; }
    }

    public class Stats : IStats
    {
        public float PossessionTime { get; set; }

        public int Points { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public int ShotsTaken { get; set; }
        public int Saves { get; set; }

        public int Interceptions { get; set; }
        public int Steals { get; set; }
        public int Blocks { get; set; }
        public int Stuns { get; set; }

        public int Catches { get; set; }
        public int Passes { get; set; }
    }
}
