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
        private const string SAMPLE_SESSION = @"{
  ""client_name"": ""ajedi32"",
  ""sessionid"": ""0BD7D136-E487-11E8-9F32-F2801F1B9FD1"",
  ""match_type"": ""Echo_Arena_Private"",
  ""map_name"": ""mpl_arena_a"",
  ""private_match"": true,
  ""tournament_match"": false,
  ""game_clock_display"": ""00:45.65"",
  ""game_clock"": 45.659531,
  ""game_status"": ""playing"",
  ""possession"": [1, 0],
  ""blue_points"": 9,
  ""orange_points"": 5,
  ""disc"": {
    ""position"": [1, 2, 3],
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
          ""possession"": true,
          ""stunned"": true,
          ""blocking"": true,
          ""invulnerable"": true,
          ""position"": [1.1, 2.2, 3.3],
          ""velocity"": [1.1, 2.2, 3.3],
          ""lhand"": [1.1, 2.2, 3.3],
          ""rhand"": [1.1, 2.2, 3.3],
          ""forward"": [1.1, 2.2, 3.3],
          ""left"": [1.1, 2.2, 3.3],
          ""up"": [1.1, 2.2, 3.3],
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
            Assert.AreEqual(45.659531f, lastSession.game_clock);
            Assert.AreEqual("playing", lastSession.game_status);
            Assert.AreEqual(9, lastSession.blue_points);
            Assert.AreEqual(5, lastSession.orange_points);            
            Assert.IsNotNull(lastSession.possession);
            Assert.AreEqual(2, lastSession.possession.Length);
            Assert.AreEqual(1, lastSession.possession[0]);
            Assert.AreEqual(0, lastSession.possession[1]);
            
            Assert.IsNotNull(lastSession.disc);
            Assert.IsNotNull(lastSession.disc.position);
            Assert.AreEqual(3, lastSession.disc.position.Length);
            Assert.AreEqual(1f, lastSession.disc.position[0]);
            Assert.AreEqual(2f, lastSession.disc.position[1]);
            Assert.AreEqual(3f, lastSession.disc.position[2]);
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
            Assert.IsTrue(player.possession);
            Assert.IsTrue(player.stunned);
            Assert.IsTrue(player.blocking);
            Assert.IsTrue(player.invulnerable);
            
            Assert.IsNotNull(player.position);
            Assert.AreEqual(3, player.position.Length);
            Assert.AreEqual(1.1f, player.position[0]);
            Assert.AreEqual(2.2f, player.position[1]);
            Assert.AreEqual(3.3f, player.position[2]);
            
            Assert.IsNotNull(player.velocity);
            Assert.AreEqual(3, player.velocity.Length);
            Assert.AreEqual(1.1f, player.velocity[0]);
            Assert.AreEqual(2.2f, player.velocity[1]);
            Assert.AreEqual(3.3f, player.velocity[2]);

            Assert.IsNotNull(player.lhand);
            Assert.AreEqual(3, player.lhand.Length);
            Assert.AreEqual(1.1f, player.lhand[0]);
            Assert.AreEqual(2.2f, player.lhand[1]);
            Assert.AreEqual(3.3f, player.lhand[2]);

            Assert.IsNotNull(player.rhand);
            Assert.AreEqual(3, player.rhand.Length);
            Assert.AreEqual(1.1f, player.rhand[0]);
            Assert.AreEqual(2.2f, player.rhand[1]);
            Assert.AreEqual(3.3f, player.rhand[2]);

            Assert.IsNotNull(player.forward);
            Assert.AreEqual(3, player.forward.Length);
            Assert.AreEqual(1.1f, player.forward[0]);
            Assert.AreEqual(2.2f, player.forward[1]);
            Assert.AreEqual(3.3f, player.forward[2]);

            Assert.IsNotNull(player.left);
            Assert.AreEqual(3, player.left.Length);
            Assert.AreEqual(1.1f, player.left[0]);
            Assert.AreEqual(2.2f, player.left[1]);
            Assert.AreEqual(3.3f, player.left[2]);

            Assert.IsNotNull(player.up);
            Assert.AreEqual(3, player.up.Length);
            Assert.AreEqual(1.1f, player.up[0]);
            Assert.AreEqual(2.2f, player.up[1]);
            Assert.AreEqual(3.3f, player.up[2]);

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
        public void IgnoresFetchErrors() //TODO: for now.  ultimately, we should be ignoring errors.  need to at least log them
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
        public void IgnoresParseErrors() //TODO: for now.  ultimately, we should be ignoring errors.  need to at least log them
        {
            var apiConfig = new ApiConfig();
            var mockApiWebClient = new Mock<IApiWebClient>();
            mockApiWebClient.SetupSequence(x => x.DownloadString($"http://127.0.0.1:{apiConfig.Port}/session"))
                .Returns("notjson")
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
