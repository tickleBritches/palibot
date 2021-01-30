namespace PaliBot.ApiClient
{
    public class Session
    {
        //public string sessionid { get; set; }
        //public string match_type { get; set; }
        //public bool private_match { get; set; }
        //public bool tournament_match { get; set; }


        public float game_clock { get; set; }
        public string game_status { get; set; }

        public int blue_points { get; set; }
        public int orange_points { get; set; }

        public int[] possession { get; set; }
        public SessionDisc disc { get; set; }
        public SessionLastScore last_score { get; set; }
        public SessionTeam[] teams { get; set; }
    }

    public class SessionDisc
    {
        public float[] position { get; set; }
        public float[] velocity { get; set; }
        public int bounce_count { get; set; }
    }

    public class SessionLastScore
    {
        public float disc_speed { get; set; }
        public string team { get; set; }
        public string goal_type { get; set; }
        public int point_amount { get; set; }
        public float distance_thrown { get; set; }
        public string person_scored { get; set; }
        public string assist_scored { get; set; }
    }

    public class SessionTeam
    {
        public string team { get; set; }
        public bool possession { get; set; }
        public SessionStats stats { get; set; }
        public SessionPlayer[] players { get; set; }
    }

    public class SessionStats
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

    public class SessionPlayer
    {
        public string name { get; set; }
        public int playerid { get; set; }
        public long userid { get; set; }
        public int level { get; set; }
        public int number { get; set; }
        
        public bool possession { get; set; }
        public bool stunned { get; set; }
        public bool blocking { get; set; }
        public bool invulnerable { get; set; }
        
        public float[] position { get; set; }
        public float[] velocity { get; set; }
        
        public float[] lhand { get; set; }
        public float[] rhand { get; set; }
        
        public float[] forward { get; set; }
        public float[] left { get; set; }
        public float[] up { get; set; }
        public SessionStats stats { get; set; }
    }
}
