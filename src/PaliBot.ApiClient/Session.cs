using PaliBot.Model;

namespace PaliBot.ApiClient
{
    public class Session : ISession
    {
        public string sessionid { get; set; }
        public string map_name { get; set; }
        public string match_type { get; set; }
        public bool private_match { get; set; }
        public bool tournament_match { get; set; }

        public string client_name { get; set; }

        public int orange_team_restart_request { get; set; }
        public int blue_team_restart_request { get; set; }
        public string sessionip { get; set; }


        public string game_status { get; set; }
        public float game_clock { get; set; }
        public string game_clock_display { get; set; }

        public SessionClientPlayer player { get; set; }

        public SessionPause pause { get; set; }

        public int blue_points { get; set; }
        public int orange_points { get; set; }

        public int[] possession { get; set; }
        public SessionDisc disc { get; set; }
        public SessionLastScore last_score { get; set; }
        public SessionTeam[] teams { get; set; }

        ISessionClientPlayer ISession.player => player;
        ISessionPause ISession.pause => pause;
        ISessionDisc ISession.disc => disc;
        ISessionLastScore ISession.last_score => last_score;
        ISessionTeam[] ISession.teams => teams;
    }

    public class SessionClientPlayer : ISessionClientPlayer
    {
        public float[] vr_left { get; set; }
        public float[] vr_position { get; set; }
        public float[] vr_forward { get; set; }
        public float[] vr_up { get; set; }
    }

    public class SessionDisc : ISessionDisc
    {
        public float[] position { get; set; }
        public float[] forward { get; set; }
        public float[] left { get; set; }
        public float[] up { get; set; }
        public float[] velocity { get; set; }
        public int bounce_count { get; set; }
    }

    public class SessionHand : ISessionHand
    {
        public float[] pos { get; set; }
        public float[] forward { get; set; }
        public float[] left { get; set; }
        public float[] up { get; set; }
    }

    public class SessionHeadBody : ISessionHeadBody
    {
        public float[] position { get; set; }
        public float[] forward { get; set; }
        public float[] left { get; set; }
        public float[] up { get; set; }
    }

    public class SessionLastScore : ISessionLastScore
    {
        public float disc_speed { get; set; }
        public string team { get; set; }
        public string goal_type { get; set; }
        public int point_amount { get; set; }
        public float distance_thrown { get; set; }
        public string person_scored { get; set; }
        public string assist_scored { get; set; }
    }

    public class SessionPause : ISessionPause
    {
        public string paused_state { get; set; }
        public string unpaused_team { get; set; }
        public string paused_requested_team { get; set; }
        public float unpaused_timer { get; set; }
        public float paused_timer { get; set; }
    }

    public class SessionPlayer : ISessionPlayer
    {
        public string name { get; set; }
        public int playerid { get; set; }
        public long userid { get; set; }
        public int level { get; set; }
        public int number { get; set; }
        public int ping { get; set; }

        public bool possession { get; set; }
        public bool stunned { get; set; }
        public bool blocking { get; set; }
        public bool invulnerable { get; set; }

        public float[] velocity { get; set; }

        public SessionHand lhand { get; set; }
        public SessionHand rhand { get; set; }

        public SessionHeadBody head { get; set; }
        public SessionHeadBody body { get; set; }

        public SessionStats stats { get; set; }

        ISessionHand ISessionPlayer.lhand => lhand;
        ISessionHand ISessionPlayer.rhand => rhand;
        ISessionHeadBody ISessionPlayer.head => head;
        ISessionHeadBody ISessionPlayer.body => body;
        ISessionStats ISessionPlayer.stats => stats;
    }

    public class SessionStats : ISessionStats
    {
        public int points { get; set; }
        public float possession_time { get; set; }
        public int interceptions { get; set; }
        public int blocks { get; set; }
        public int steals { get; set; }
        public int catches { get; set; }
        public int passes { get; set; }
        public int saves { get; set; }
        public int goals { get; set; }
        public int stuns { get; set; }
        public int assists { get; set; }
        public int shots_taken { get; set; }
    }

    public class SessionTeam : ISessionTeam
    {
        public string team { get; set; }
        public bool possession { get; set; }
        public SessionStats stats { get; set; }
        public SessionPlayer[] players { get; set; }

        ISessionStats ISessionTeam.stats => stats;
        ISessionPlayer[] ISessionTeam.players => players;
    }
    
}
