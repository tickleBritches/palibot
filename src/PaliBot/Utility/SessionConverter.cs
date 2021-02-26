using PaliBot.Model;
using PaliBot.Model.Frame;
using PaliBot.Numerics;
using System.Collections.Generic;
using System.Linq;

namespace PaliBot.Utility
{
    internal interface ISessionConverter
    {
        IFrame ToFrame(ISession session);
    }

    public class SessionConverter : ISessionConverter
    {
        private static Dictionary<string, GameStatus> _gameStatusMapping = new Dictionary<string, GameStatus>
        {
            ["playing"] = GameStatus.Playing,
            ["post_match"] = GameStatus.PostMatch,
            ["post_sudden_death"] = GameStatus.PostSuddenDeath,
            ["pre_match"] = GameStatus.PreMatch,
            ["pre_sudden_death"] = GameStatus.PreSuddenDeath,
            ["round_start"] = GameStatus.RoundStart,
            ["round_over"] = GameStatus.RoundOver,
            ["score"] = GameStatus.Score,
            ["sudden_death"] = GameStatus.SuddenDeath
        };

        private static Dictionary<string, GoalType> _goalTypeMapping = new Dictionary<string, GoalType>
        {
            ["[NO GOAL]"] = GoalType.NoGoal,
            ["SLAM DUNK"] = GoalType.SlamDunk,
            ["INSIDE SHOT"] = GoalType.InsideShot,
            ["LONG SHOT"] = GoalType.LongShot,
            ["BOUNCE SHOT"] = GoalType.BounceShot,
            ["LONG BOUNCE SHOT"] = GoalType.LongBounceShot
        };

        public IFrame ToFrame(ISession session)
        {
            var frame = new Frame
            {
                Client =
                {
                    Name = session.client_name,
                    Pose = new Pose
                    {
                        Position = session.player.vr_position.ToVector3(),
                        Orientation =
                        {
                            Forward = session.player.vr_forward.ToVector3(),
                            Left = session.player.vr_left.ToVector3(),
                            Up = session.player.vr_up.ToVector3()
                        }
                    }
                },

                Match =
                {
                    SessionId = session.sessionid,
                    SessionIp = session.sessionip,
                    Status = _gameStatusMapping.GetValueOrDefault(session.game_status, GameStatus.Unknown),
                    GameClock = session.game_clock,
                    PauseState = (session.pause.paused_state == "paused" && session.pause.unpaused_timer == 0)
                        ? PauseState.Paused
                        : session.pause.unpaused_timer > 0f
                            ? PauseState.Unpausing
                            : PauseState.Unpaused,
                    PausedTimer = session.pause.paused_timer,
                    UnpausingTimer = session.pause.unpaused_timer
                },

                Disc =
                {
                    BounceCount = session.disc.bounce_count,
                    Pose =
                    {
                        Position = session.disc.position.ToVector3(),
                        Orientation =
                        {
                            Forward = session.disc.forward.ToVector3(),
                            Left = session.disc.left.ToVector3(),
                            Up = session.disc.up.ToVector3()
                        }
                    },
                    Velocity = session.disc.velocity.ToVector3()
                },

                LastScore =
                {
                    DiscSpeed = session.last_score.disc_speed,
                    Distance = session.last_score.distance_thrown,
                    GoalType = _goalTypeMapping.GetValueOrDefault(session.last_score.goal_type, GoalType.Unknown),
                    Points = session.last_score.point_amount
                }
            };

            // fill out teams and players
            FillTeam(frame.BlueTeam, session.teams[0], TeamColor.Blue);
            FillTeam(frame.OrangeTeam, session.teams[1], TeamColor.Orange);

            // wire up pause teams
            frame.Match.PausedBy = session.pause.paused_requested_team != "none"
                ? frame.BlueTeam.Name == session.pause.paused_requested_team ? frame.BlueTeam : frame.OrangeTeam
                : null;

            frame.Match.UnpausedBy = session.pause.unpaused_team != "none"
                ? frame.BlueTeam.Name == session.pause.unpaused_team ? frame.BlueTeam : frame.OrangeTeam
                : null;

            // wire up last score teams and players
            frame.LastScore.Team = session.last_score.team == "blue"
                ? frame.BlueTeam
                : frame.OrangeTeam;

            frame.LastScore.ScoredBy = frame.LastScore.Team.Players.FirstOrDefault(p => p.Name == session.last_score.person_scored);
            frame.LastScore.AssistedBy = frame.LastScore.Team.Players.FirstOrDefault(p => p.Name == session.last_score.assist_scored);

            // wire up last possession player
            if (session.possession.Length == 2)
            {
                var possessionTeam = session.possession[0] == 0 ? frame.BlueTeam : frame.OrangeTeam;
                frame.LastPossessionPlayer = possessionTeam.Players.Count > session.possession[1] ? possessionTeam.Players[session.possession[1]] : null;
            }

            return frame;
        }

        private static void FillTeam(Team team, ISessionTeam sessionTeam, TeamColor color)
        {

            team.Color = color;
            team.Name = sessionTeam.team;
            team.HasPossesion = sessionTeam.possession;

            var tStats = team.Stats;
            var sStats = sessionTeam.stats;
            tStats.Assists = sStats.assists;
            tStats.Blocks = sStats.blocks;
            tStats.Catches = sStats.catches;
            tStats.Goals = sStats.goals;
            tStats.Interceptions = sStats.interceptions;
            tStats.Passes = sStats.passes;
            tStats.Points = sStats.points;
            tStats.PossessionTime = sStats.possession_time;
            tStats.Saves = sStats.saves;
            tStats.ShotsTaken = sStats.shots_taken;
            tStats.Steals = sStats.steals;
            tStats.Stuns = sStats.stuns;

            team.Players.Capacity = sessionTeam.players.Length;

            for (var i = 0; i < sessionTeam.players.Length; i++)
            {
                var sessionPlayer = sessionTeam.players[i];
                sStats = sessionPlayer.stats;
                team.Players.Add(new Player
                {
                    Id = sessionPlayer.playerid,
                    UserId = sessionPlayer.userid,
                    Name = sessionPlayer.name,
                    Level = sessionPlayer.level,
                    Number = sessionPlayer.number,
                    Ping = sessionPlayer.ping,
                    HasPossesion = sessionPlayer.possession,
                    IsBlocking = sessionPlayer.blocking,
                    IsInvulnerable = sessionPlayer.invulnerable,
                    IsStunned = sessionPlayer.stunned,

                    Head =
                    {
                        Position = sessionPlayer.head.position.ToVector3(),
                        Orientation =
                        {
                            Forward = sessionPlayer.head.forward.ToVector3(),
                            Left = sessionPlayer.head.left.ToVector3(),
                            Up = sessionPlayer.head.up.ToVector3()
                        }
                    },

                    Body =
                    {
                        Position = sessionPlayer.body.position.ToVector3(),
                        Orientation =
                        {
                            Forward = sessionPlayer.body.forward.ToVector3(),
                            Left = sessionPlayer.body.left.ToVector3(),
                            Up = sessionPlayer.body.up.ToVector3()
                        }
                    },

                    LeftHand =
                    {
                        Position = sessionPlayer.lhand.pos.ToVector3(),
                        Orientation =
                        {
                            Forward = sessionPlayer.lhand.forward.ToVector3(),
                            Left = sessionPlayer.lhand.left.ToVector3(),
                            Up = sessionPlayer.lhand.up.ToVector3()
                        }
                    },

                    RightHand =
                    {
                        Position = sessionPlayer.rhand.pos.ToVector3(),
                        Orientation =
                        {
                            Forward = sessionPlayer.rhand.forward.ToVector3(),
                            Left = sessionPlayer.rhand.left.ToVector3(),
                            Up = sessionPlayer.rhand.up.ToVector3()
                        }
                    },

                    Velocity = sessionPlayer.velocity.ToVector3(),

                    Stats = {
                        Assists = sStats.assists,
                        Blocks = sStats.blocks,
                        Catches = sStats.catches,
                        Goals = sStats.goals,
                        Interceptions = sStats.goals,
                        Passes = sStats.passes,
                        Points = sStats.points,
                        PossessionTime = sStats.possession_time,
                        Saves = sStats.saves,
                        ShotsTaken = sStats.shots_taken,
                        Steals = sStats.steals,
                        Stuns = sStats.stuns
                    },

                    Team = team
                });
            }
        }
    }
}
