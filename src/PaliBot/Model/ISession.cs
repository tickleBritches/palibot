namespace PaliBot.Model
{
    public interface ISession
    {
        public string sessionid { get; }
        public string map_name { get; }
        public string match_type { get; }
        public bool private_match { get; }
        public bool tournament_match { get; }

        public string client_name { get; }

        public int orange_team_restart_request { get; }
        public int blue_team_restart_request { get; }
        public string sessionip { get; }

        public string game_status { get; }
        public float game_clock { get; }
        public string game_clock_display { get; }

        public ISessionClientPlayer player { get; }

        public ISessionPause pause { get; }

        public int blue_points { get; }
        public int orange_points { get; }

        public int[] possession { get; }
        public ISessionDisc disc { get; }
        public ISessionLastScore last_score { get; }
        public ISessionTeam[] teams { get; }
    }

    public interface ISessionClientPlayer
    {
        public float[] vr_left { get; }
        public float[] vr_position { get; }
        public float[] vr_forward { get; }
        public float[] vr_up { get; }
    }

    public interface ISessionDisc
    {
        public float[] position { get; }
        public float[] forward { get; }
        public float[] left { get; }
        public float[] up { get; }
        public float[] velocity { get; }
        public int bounce_count { get; }
    }

    public interface ISessionHand
    {
        public float[] pos { get; }
        public float[] forward { get; }
        public float[] left { get; }
        public float[] up { get; }
    }

    public interface ISessionHeadBody
    {
        public float[] position { get; }
        public float[] forward { get; }
        public float[] left { get; }
        public float[] up { get; }
    }

    public interface ISessionLastScore
    {
        public float disc_speed { get; }
        public string team { get; }
        public string goal_type { get; }
        public int point_amount { get; }
        public float distance_thrown { get; }
        public string person_scored { get; }
        public string assist_scored { get; }
    }

    public interface ISessionPause
    {
        public string paused_state { get; }
        public string unpaused_team { get; }
        public string paused_requested_team { get; }
        public float unpaused_timer { get; }
        public float paused_timer { get; }
    }

    public interface ISessionPlayer
    {
        public string name { get; }
        public int playerid { get; }
        public long userid { get; }
        public int level { get; }
        public int number { get; }
        public int ping { get; }

        public bool possession { get; }
        public bool stunned { get; }
        public bool blocking { get; }
        public bool invulnerable { get; }

        public float[] velocity { get; }

        public ISessionHand lhand { get; }
        public ISessionHand rhand { get; }

        public ISessionHeadBody head { get; }
        public ISessionHeadBody body { get; }

        public ISessionStats stats { get; }
    }

    public interface ISessionStats
    {
        public int points { get; }
        public float possession_time { get; }
        public int interceptions { get; }
        public int blocks { get; }
        public int steals { get; }
        public int catches { get; }
        public int passes { get; }
        public int saves { get; }
        public int goals { get; }
        public int stuns { get; }
        public int assists { get; }
        public int shots_taken { get; }
    }

    public interface ISessionTeam
    {
        public string team { get; }
        public bool possession { get; }
        public ISessionStats stats { get; }
        public ISessionPlayer[] players { get; }
    }
}
