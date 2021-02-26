using System.Numerics;

namespace PaliBot.Model.Frame
{
    public interface IPlayer
    {
        int Id { get; set; }
        long UserId { get; }

        string Name { get; }
        int Level { get; }
        int Number { get; }
        int Ping { get; }

        Stats Stats { get; }

        bool HasPossesion { get; }
        bool IsStunned { get; }
        bool IsBlocking { get; }
        bool IsInvulnerable { get; }
        
        IPose Head { get; }
        IPose Body { get; }
        IPose LeftHand { get; }
        IPose RightHand { get; }

        Vector3 Velocity { get; }
        
        ITeam Team { get; }
    }

    public class Player : IPlayer
    {
        public int Id { get; set; }
        public long UserId { get; set; }

        public string Name { get; set; }
        public int Level { get; set; }
        public int Number { get; set; }
        public int Ping { get; set; }

        public Stats Stats { get; } = new Stats();

        public bool HasPossesion { get; set; }
        public bool IsStunned { get; set; }
        public bool IsBlocking { get; set; }
        public bool IsInvulnerable { get; set; }


        public Pose Head { get; } = new Pose();
        public Pose Body { get; } = new Pose();
        public Pose LeftHand { get; } = new Pose();
        public Pose RightHand { get; } = new Pose();

        public Vector3 Velocity { get; set; }

        public Team Team { get; set; }

        IPose IPlayer.Head => Head;
        IPose IPlayer.Body => Body;
        IPose IPlayer.LeftHand => LeftHand;
        IPose IPlayer.RightHand => RightHand;
        ITeam IPlayer.Team => Team;
    }

    public static class IPlayerExtensions
    {
        public static bool IsSame(this IPlayer a, IPlayer b)
        {
            if (a == null && b == null) return true;
            if (a == null || b == null) return false;
            return a.Id == b.Id;
        }
    }
}
