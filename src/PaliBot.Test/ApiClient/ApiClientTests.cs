using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaliBot.ApiClient;
using PaliBot.ApiClient.WebClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PaliBot.Test.ApiClient
{
    [TestClass]
    public class ApiClientTests
    {
        // taken from https://github.com/Graicc/echovr_api_docs and modified to non-default primitives for testing
        internal const string SAMPLE_SESSION = @"{
  ""client_name"": ""ajedi32"",
  ""sessionid"": ""0BD7D136-E487-11E8-9F32-F2801F1B9FD1"",
  ""sessionip"": ""127.0.0.1"",
  ""match_type"": ""Echo_Arena_Private"",
  ""map_name"": ""mpl_arena_a"",
  ""private_match"": true,
  ""tournament_match"": true,
  ""game_clock_display"": ""00:45.65"",
  ""game_clock"": 45.659531,
  ""game_status"": ""playing"",
  ""possession"": [1, 0],
  ""blue_points"": 9,
  ""orange_points"": 5,
  ""orange_team_restart_request"": 1,
  ""blue_team_restart_request"": 1,
  ""player"": {
    ""vr_left"": [
        -0.99600005,
        0.1,
        -0.088000007
    ],
    ""vr_position"": [
        0.26900002,
        4.671,
        52.986004
    ],
    ""vr_forward"": [
        0.088000007,
        -0.001,
        -0.99600005
    ],
    ""vr_up"": [
        0.1,
        1.0,
        -0.001
    ]
  },
  ""pause"": {
    ""paused_state"": ""unpaused"",
    ""unpaused_team"": ""none"",
    ""paused_requested_team"": ""none"",
    ""unpaused_timer"": 1.0,
    ""paused_timer"": 1.0
  },
  ""disc"": {
    ""position"": [
      0.0,
      4.5360003,
      27.500002
    ],
    ""forward"": [
      0.001,
      -0.001,
      1.0
    ],
    ""left"": [
      1.0,
      0.001,
      -0.001
    ],
    ""up"": [
      -0.001,
      1.0,
      0.001
    ],
    ""velocity"": [1, 2, 3],
    ""bounce_count"": 1
  },
  ""last_score"": {
    ""disc_speed"": 1,
    ""team"": ""blue"",
    ""goal_type"": ""[NO GOAL]"",
    ""point_amount"": 2,
    ""distance_thrown"": 1,
    ""person_scored"": ""[INVALID]"",
    ""assist_scored"": ""[INVALID]""
  },
  ""teams"": [
    {
      ""team"": ""BLUE TEAM"",
      ""possession"": true,
      ""stats"": {
        ""points"": 9,
        ""possession_time"": 132.18958,
        ""interceptions"": 1,
        ""blocks"": 1,
        ""steals"": 1,
        ""catches"": 1,
        ""passes"": 1,
        ""saves"": 2,
        ""goals"": 4,
        ""stuns"": 29,
        ""assists"": 2,
        ""shots_taken"": 7
      },
      ""players"": [
        {
          ""name"": ""Bob"",
          ""playerid"": 1,
          ""userid"": 9221405949665979,
          ""level"": 16,
          ""number"": 88,
          ""ping"": 20,
          ""possession"": true,
          ""stunned"": true,
          ""blocking"": true,
          ""invulnerable"": true,
          ""head"": {
            ""position"": [
              0.65900004,
              4.7800002,
              -51.256004
            ],
            ""forward"": [
              -0.043000001,
              -0.0070000002,
              0.99900007
            ],
            ""left"": [
              0.99700004,
              0.062000003,
              0.043000001
            ],
            ""up"": [
              -0.062000003,
              0.99800003,
              0.0050000004
            ]
          },
          ""body"": {
            ""position"": [
              0.65900004,
              4.7800002,
              -51.256004
            ],
            ""forward"": [
              0.039000001,
              0.001,
              0.99900007
            ],
            ""left"": [
              0.99900007,
              -0.001,
              -0.039000001
            ],
            ""up"": [
              0.001,
              1.0,
              -0.001
            ]
          },
          ""velocity"": [1.1, 2.2, 3.3],
          ""lhand"": {
            ""pos"": [
                0.88000005,
                4.4900002,
                -50.889004
            ],
            ""forward"": [
                0.12100001,
                0.55900002,
                0.82000005
            ],
            ""left"": [
                0.88800007,
                0.30800003,
                -0.34100002
            ],
            ""up"": [
                -0.44300002,
                0.77000004,
                -0.45900002
            ]
          },
          ""rhand"": {
            ""pos"": [
                0.57600003,
                4.4050002,
                -50.778004
            ],
            ""forward"": [
                -0.82000005,
                -0.45500001,
                0.34800002
            ],
            ""left"": [
                0.333,
                0.116,
                0.93600005
            ],
            ""up"": [
                -0.46600002,
                0.88300002,
                0.056000002
            ]
          },
          ""stats"": {
            ""points"": 5,
            ""possession_time"": 78.645569,
            ""interceptions"": 1,
            ""blocks"": 1,
            ""steals"": 1,
            ""catches"": 1,
            ""passes"": 1,
            ""saves"": 1,
            ""goals"": 1,
            ""stuns"": 14,
            ""assists"": 1,
            ""shots_taken"": 5
          }
        }
      ]
    }
  ]
}";

        [TestMethod]
        public void Session_Fires()
        {
            var apiConfig = new ApiConfig();
            apiConfig.Port = 1234;

            var mockApiWebClient = new Mock<IApiWebClient>();
            mockApiWebClient.Setup(x => x.DownloadString($"http://127.0.0.1:{apiConfig.Port}/session")).Returns(SAMPLE_SESSION);

            var apiClient = new PaliBot.ApiClient.ApiClient(apiConfig, mockApiWebClient.Object);

            var sessionReset = new AutoResetEvent(false);
            var lastSession = (Session)null;
            apiClient.Session += (s, e) =>
            {
                lastSession = e;
                sessionReset.Set();
            };

            //

            apiClient.Start();
            sessionReset.WaitOne();
            apiClient.Stop();

            Assert.IsNotNull(lastSession);
            Assert.AreEqual("ajedi32", lastSession.client_name);
            Assert.AreEqual("0BD7D136-E487-11E8-9F32-F2801F1B9FD1", lastSession.sessionid);
            Assert.AreEqual("127.0.0.1", lastSession.sessionip);
            Assert.AreEqual("Echo_Arena_Private", lastSession.match_type);
            Assert.AreEqual("mpl_arena_a", lastSession.map_name);
            Assert.IsTrue(lastSession.private_match);
            Assert.IsTrue(lastSession.tournament_match);
            Assert.AreEqual(45.659531f, lastSession.game_clock);
            Assert.AreEqual("playing", lastSession.game_status);
            Assert.AreEqual("00:45.65", lastSession.game_clock_display);
            Assert.AreEqual(1, lastSession.orange_team_restart_request);
            Assert.AreEqual(1, lastSession.blue_team_restart_request);

            Assert.IsNotNull(lastSession.player);
            Assert.IsNotNull(lastSession.player.vr_left);
            Assert.AreEqual(3, lastSession.player.vr_left.Length);
            Assert.AreEqual(-0.99600005f, lastSession.player.vr_left[0]);
            Assert.AreEqual(0.1f, lastSession.player.vr_left[1]);
            Assert.AreEqual(-0.088000007f, lastSession.player.vr_left[2]);
            Assert.IsNotNull(lastSession.player.vr_position);
            Assert.AreEqual(3, lastSession.player.vr_position.Length);
            Assert.AreEqual(0.26900002f, lastSession.player.vr_position[0]);
            Assert.AreEqual(4.671f, lastSession.player.vr_position[1]);
            Assert.AreEqual(52.986004f, lastSession.player.vr_position[2]);
            Assert.IsNotNull(lastSession.player.vr_forward);
            Assert.AreEqual(3, lastSession.player.vr_forward.Length);
            Assert.AreEqual(0.088000007f, lastSession.player.vr_forward[0]);
            Assert.AreEqual(-0.001f, lastSession.player.vr_forward[1]);
            Assert.AreEqual(-0.99600005f, lastSession.player.vr_forward[2]);
            Assert.IsNotNull(lastSession.player.vr_up);
            Assert.AreEqual(3, lastSession.player.vr_up.Length);
            Assert.AreEqual(0.1f, lastSession.player.vr_up[0]);
            Assert.AreEqual(1.0f, lastSession.player.vr_up[1]);
            Assert.AreEqual(-0.001f, lastSession.player.vr_up[2]);

            Assert.IsNotNull(lastSession.pause);
            Assert.AreEqual("unpaused", lastSession.pause.paused_state);
            Assert.AreEqual("none", lastSession.pause.unpaused_team);
            Assert.AreEqual("none", lastSession.pause.paused_requested_team);
            Assert.AreEqual(1f, lastSession.pause.unpaused_timer);
            Assert.AreEqual(1f, lastSession.pause.paused_timer);

            Assert.AreEqual(9, lastSession.blue_points);
            Assert.AreEqual(5, lastSession.orange_points);            
            Assert.IsNotNull(lastSession.possession);
            Assert.AreEqual(2, lastSession.possession.Length);
            Assert.AreEqual(1, lastSession.possession[0]);
            Assert.AreEqual(0, lastSession.possession[1]);
            
            Assert.IsNotNull(lastSession.disc);
            Assert.IsNotNull(lastSession.disc.position);
            Assert.AreEqual(3, lastSession.disc.position.Length);
            Assert.AreEqual(0f, lastSession.disc.position[0]);
            Assert.AreEqual(4.5360003f, lastSession.disc.position[1]);
            Assert.AreEqual(27.500002f, lastSession.disc.position[2]);
            Assert.IsNotNull(lastSession.disc.forward);
            Assert.AreEqual(3, lastSession.disc.forward.Length);
            Assert.AreEqual(0.001f, lastSession.disc.forward[0]);
            Assert.AreEqual(-0.001f, lastSession.disc.forward[1]);
            Assert.AreEqual(1.0f, lastSession.disc.forward[2]);
            Assert.IsNotNull(lastSession.disc.left);
            Assert.AreEqual(3, lastSession.disc.left.Length);
            Assert.AreEqual(1.0f, lastSession.disc.left[0]);
            Assert.AreEqual(0.001f, lastSession.disc.left[1]);
            Assert.AreEqual(-0.001f, lastSession.disc.left[2]);
            Assert.IsNotNull(lastSession.disc.up);
            Assert.AreEqual(3, lastSession.disc.up.Length);
            Assert.AreEqual(-0.001f, lastSession.disc.up[0]);
            Assert.AreEqual(1.0f, lastSession.disc.up[1]);
            Assert.AreEqual(0.001f, lastSession.disc.up[2]);
            Assert.IsNotNull(lastSession.disc.velocity);
            Assert.AreEqual(3, lastSession.disc.velocity.Length);
            Assert.AreEqual(1f, lastSession.disc.velocity[0]);
            Assert.AreEqual(2f, lastSession.disc.velocity[1]);
            Assert.AreEqual(3f, lastSession.disc.velocity[2]);
            Assert.AreEqual(1, lastSession.disc.bounce_count);

            Assert.IsNotNull(lastSession.last_score);
            Assert.AreEqual(1f, lastSession.last_score.disc_speed);
            Assert.AreEqual("blue", lastSession.last_score.team);
            Assert.AreEqual("[NO GOAL]", lastSession.last_score.goal_type);
            Assert.AreEqual(2, lastSession.last_score.point_amount);
            Assert.AreEqual(1f, lastSession.last_score.distance_thrown);
            Assert.AreEqual("[INVALID]", lastSession.last_score.person_scored);
            Assert.AreEqual("[INVALID]", lastSession.last_score.assist_scored);

            Assert.IsNotNull(lastSession.teams);
            Assert.AreEqual(1, lastSession.teams.Length);

            var team = lastSession.teams[0];
            Assert.AreEqual("BLUE TEAM", team.team);
            Assert.IsTrue(team.possession);

            Assert.IsNotNull(team.stats);
            Assert.AreEqual(9, team.stats.points);
            Assert.AreEqual(132.18958f, team.stats.possession_time);
            Assert.AreEqual(1, team.stats.interceptions);
            Assert.AreEqual(1, team.stats.blocks);
            Assert.AreEqual(1, team.stats.steals);
            Assert.AreEqual(1, team.stats.catches);
            Assert.AreEqual(1, team.stats.passes);
            Assert.AreEqual(2, team.stats.saves);
            Assert.AreEqual(4, team.stats.goals);
            Assert.AreEqual(29, team.stats.stuns);
            Assert.AreEqual(2, team.stats.assists);
            Assert.AreEqual(7, team.stats.shots_taken);

            Assert.IsNotNull(team.players);
            Assert.AreEqual(1, team.players.Length);

            var player = team.players[0];
            Assert.AreEqual("Bob", player.name);
            Assert.AreEqual(1, player.playerid);
            Assert.AreEqual(9221405949665979L, player.userid);
            Assert.AreEqual(16, player.level);
            Assert.AreEqual(88, player.number);
            Assert.AreEqual(20, player.ping);
            Assert.IsTrue(player.possession);
            Assert.IsTrue(player.stunned);
            Assert.IsTrue(player.blocking);
            Assert.IsTrue(player.invulnerable);

            Assert.IsNotNull(player.lhand);
            Assert.AreEqual(3, player.lhand.pos.Length);
            Assert.AreEqual(0.88000005f, player.lhand.pos[0]);
            Assert.AreEqual(4.4900002f, player.lhand.pos[1]);
            Assert.AreEqual(-50.889004f, player.lhand.pos[2]);
            Assert.AreEqual(3, player.lhand.forward.Length);
            Assert.AreEqual(0.12100001f, player.lhand.forward[0]);
            Assert.AreEqual(0.55900002f, player.lhand.forward[1]);
            Assert.AreEqual(0.82000005f, player.lhand.forward[2]);
            Assert.AreEqual(3, player.lhand.left.Length);
            Assert.AreEqual(0.88800007f, player.lhand.left[0]);
            Assert.AreEqual(0.30800003f, player.lhand.left[1]);
            Assert.AreEqual(-0.34100002f, player.lhand.left[2]);
            Assert.AreEqual(3, player.lhand.up.Length);
            Assert.AreEqual(-0.44300002f, player.lhand.up[0]);
            Assert.AreEqual(0.77000004f, player.lhand.up[1]);
            Assert.AreEqual(-0.45900002f, player.lhand.up[2]);

            Assert.IsNotNull(player.rhand);
            Assert.AreEqual(3, player.rhand.pos.Length);
            Assert.AreEqual(0.57600003f, player.rhand.pos[0]);
            Assert.AreEqual(4.4050002f, player.rhand.pos[1]);
            Assert.AreEqual(-50.778004f, player.rhand.pos[2]);
            Assert.AreEqual(3, player.rhand.forward.Length);
            Assert.AreEqual(-0.82000005f, player.rhand.forward[0]);
            Assert.AreEqual(-0.45500001f, player.rhand.forward[1]);
            Assert.AreEqual(0.34800002f, player.rhand.forward[2]);
            Assert.AreEqual(3, player.rhand.left.Length);
            Assert.AreEqual(0.333f, player.rhand.left[0]);
            Assert.AreEqual(0.116f, player.rhand.left[1]);
            Assert.AreEqual(0.93600005f, player.rhand.left[2]);
            Assert.AreEqual(3, player.rhand.up.Length);
            Assert.AreEqual(-0.46600002f, player.rhand.up[0]);
            Assert.AreEqual(0.88300002f, player.rhand.up[1]);
            Assert.AreEqual(0.056000002f, player.rhand.up[2]);

            Assert.IsNotNull(player.head);
            Assert.AreEqual(3, player.head.position.Length);
            Assert.AreEqual(0.65900004f, player.head.position[0]);
            Assert.AreEqual(4.7800002f, player.head.position[1]);
            Assert.AreEqual(-51.256004f, player.head.position[2]);
            Assert.AreEqual(3, player.head.forward.Length);
            Assert.AreEqual(-0.043000001f, player.head.forward[0]);
            Assert.AreEqual(-0.0070000002f, player.head.forward[1]);
            Assert.AreEqual(0.99900007f, player.head.forward[2]);
            Assert.AreEqual(3, player.head.left.Length);
            Assert.AreEqual(0.99700004f, player.head.left[0]);
            Assert.AreEqual(0.062000003f, player.head.left[1]);
            Assert.AreEqual(0.043000001f, player.head.left[2]);
            Assert.AreEqual(3, player.head.up.Length);
            Assert.AreEqual(-0.062000003f, player.head.up[0]);
            Assert.AreEqual(0.99800003f, player.head.up[1]);
            Assert.AreEqual(0.0050000004f, player.head.up[2]);

            Assert.IsNotNull(player.body);
            Assert.AreEqual(3, player.body.position.Length);
            Assert.AreEqual(0.65900004f, player.body.position[0]);
            Assert.AreEqual(4.7800002f, player.body.position[1]);
            Assert.AreEqual(-51.256004f, player.body.position[2]);
            Assert.AreEqual(3, player.body.forward.Length);
            Assert.AreEqual(0.039000001f, player.body.forward[0]);
            Assert.AreEqual(0.001f, player.body.forward[1]);
            Assert.AreEqual(0.99900007f, player.body.forward[2]);
            Assert.AreEqual(3, player.body.left.Length);
            Assert.AreEqual(0.99900007f, player.body.left[0]);
            Assert.AreEqual(-0.001f, player.body.left[1]);
            Assert.AreEqual(-0.039000001f, player.body.left[2]);
            Assert.AreEqual(3, player.body.up.Length);
            Assert.AreEqual(0.001f, player.body.up[0]);
            Assert.AreEqual(1.0f, player.body.up[1]);
            Assert.AreEqual(-0.001f, player.body.up[2]);

            Assert.IsNotNull(player.velocity);
            Assert.AreEqual(3, player.velocity.Length);
            Assert.AreEqual(1.1f, player.velocity[0]);
            Assert.AreEqual(2.2f, player.velocity[1]);
            Assert.AreEqual(3.3f, player.velocity[2]);

            Assert.IsNotNull(player.stats);
            Assert.AreEqual(5, player.stats.points);
            Assert.AreEqual(78.645569f, player.stats.possession_time);
            Assert.AreEqual(1, player.stats.interceptions);
            Assert.AreEqual(1, player.stats.blocks);
            Assert.AreEqual(1, player.stats.steals);
            Assert.AreEqual(1, player.stats.catches);
            Assert.AreEqual(1, player.stats.passes);
            Assert.AreEqual(1, player.stats.saves);
            Assert.AreEqual(1, player.stats.goals);
            Assert.AreEqual(14, player.stats.stuns);
            Assert.AreEqual(1, player.stats.assists);
            Assert.AreEqual(5, player.stats.shots_taken);
        }

       [TestMethod]
       public void IgnoresFetchErrors() 
        {
            var apiConfig = new ApiConfig();
            var mockApiWebClient = new Mock<IApiWebClient>();
            mockApiWebClient.SetupSequence(x => x.DownloadString($"http://127.0.0.1:{apiConfig.Port}/session"))
                .Throws<Exception>()
                .Returns(SAMPLE_SESSION);

            var apiClient = new PaliBot.ApiClient.ApiClient(apiConfig, mockApiWebClient.Object);

            var sessionReset = new AutoResetEvent(false);
            var lastSession = (Session)null;
            apiClient.Session += (s, e) =>
            {
                lastSession = e;
                sessionReset.Set();
            };

            //

            apiClient.Start();
            sessionReset.WaitOne();
            apiClient.Stop();

            //

            Assert.IsNotNull(lastSession);
        }

        [TestMethod]
        public void IgnoresParseErrors()
        {
            var apiConfig = new ApiConfig();
            var mockApiWebClient = new Mock<IApiWebClient>();
            mockApiWebClient.SetupSequence(x => x.DownloadString($"http://127.0.0.1:{apiConfig.Port}/session"))
                .Returns("{notjson")
                .Returns(SAMPLE_SESSION);

            var apiClient = new PaliBot.ApiClient.ApiClient(apiConfig, mockApiWebClient.Object);

            var sessionReset = new AutoResetEvent(false);
            var lastSession = (Session)null;
            apiClient.Session += (s, e) =>
            {
                lastSession = e;
                sessionReset.Set();
            };

            //

            apiClient.Start();
            sessionReset.WaitOne();
            apiClient.Stop();

            //

            Assert.IsNotNull(lastSession);
        }

        [TestMethod]
        public void UsesDefaultApiWebClient()
        {
            var apiConfig = new ApiConfig();
            var apiClient = new PaliBot.ApiClient.ApiClient(apiConfig);
        }
    }
}
