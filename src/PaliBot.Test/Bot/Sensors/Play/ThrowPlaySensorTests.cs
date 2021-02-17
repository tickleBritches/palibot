using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaliBot.Model.Event;
using PaliBot.Model.Frame;
using PaliBot.Model.Play;
using PaliBot.Sensors.Play;
using PaliBot.Sensors.State;

namespace PaliBot.Test.Bot.Sensors.Play
{
    [TestClass]
    public class ThrowPlaySensorTests
    {
        [DataTestMethod]
        [DataRow(300f - ThrowPlaySensor.SLAP_THRESHOLD_TIME)]
        [DataRow(300f - (ThrowPlaySensor.SLAP_THRESHOLD_TIME - 0.01f))]
        public void Update_YieldsSlap(float throwClock)
        {
            var player = new Player();

            var frame1 = new Frame { Match = { GameClock = 300f } };
            var possessionChangeEvent1 = new PossessionChangeEvent(frame1, null, null);

            var frame2 = new Frame { Match = { GameClock = 300f } };
            var possessionChangeEvent2 = new PossessionChangeEvent(frame1, player, HandSide.Right);

            var frame3 = new Frame { Match = { GameClock = 300f } };

            var frame4 = new Frame { Match = { GameClock = throwClock } };
            var throwEvent = new ThrowEvent(frame4, player);

            var sensor = new ThrowPlaySensor();
            sensor.Initialize(new IStateSensor[0]);

            //
            var result1 = sensor.Update(frame1, new[] { possessionChangeEvent1 });
            var result2 = sensor.Update(frame2, new[] { possessionChangeEvent2 });
            var result3 = sensor.Update(frame3, new GameEvent[0]);
            var result4 = sensor.Update(frame4, new[] { throwEvent });
            //

            Assert.IsNull(result1);
            Assert.IsNull(result2);
            Assert.IsNull(result3);

            var slap = result4 as SlapPlay;
            Assert.IsNotNull(slap);
            Assert.AreEqual(player, slap.Player);
        }

        [TestMethod]
        public void Update_YieldsThrow()
        {
            var player = new Player();

            var frame1 = new Frame { Match = { GameClock = 300f } };
            var possessionChangeEvent1 = new PossessionChangeEvent(frame1, null, null);

            var frame2 = new Frame { Match = { GameClock = 300f } };
            var possessionChangeEvent2 = new PossessionChangeEvent(frame1, player, HandSide.Right);

            var frame3 = new Frame { Match = { GameClock = 300f } };

            var frame4 = new Frame { Match = { GameClock = 300f - ThrowPlaySensor.SLAP_THRESHOLD_TIME - 1 } };
            var throwEvent = new ThrowEvent(frame4, player);

            var sensor = new ThrowPlaySensor();
            sensor.Initialize(new IStateSensor[0]);

            //
            var result1 = sensor.Update(frame1, new[] { possessionChangeEvent1 });
            var result2 = sensor.Update(frame2, new[] { possessionChangeEvent2 });
            var result3 = sensor.Update(frame3, new GameEvent[0]);
            var result4 = sensor.Update(frame4, new[] { throwEvent });
            //

            Assert.IsNull(result1);
            Assert.IsNull(result2);
            Assert.IsNull(result3);

            var slap = result4 as ThrowPlay;
            Assert.IsNotNull(slap);
            Assert.AreEqual(player, slap.Player);
        }

        [TestMethod]
        public void Update_YieldsThrowIfPlayerMismatch()
        {
            var player1 = new Player { Id = 1 };
            var player2 = new Player { Id = 2 };

            var frame1 = new Frame { Match = { GameClock = 300f } };
            var possessionChangeEvent1 = new PossessionChangeEvent(frame1, player1, HandSide.Right);

            var frame2 = new Frame { Match = { GameClock = 300f - ThrowPlaySensor.SLAP_THRESHOLD_TIME } };
            var throwEvent = new ThrowEvent(frame2, player2);

            var sensor = new ThrowPlaySensor();
            sensor.Initialize(new IStateSensor[0]);

            //
            var result1 = sensor.Update(frame1, new[] { possessionChangeEvent1 });
            var result2 = sensor.Update(frame2, new[] { throwEvent });
            //

            Assert.IsNull(result1);

            var slap = result2 as ThrowPlay;
            Assert.IsNotNull(slap);
            Assert.AreEqual(player2, slap.Player);
        }

        [TestMethod]
        public void Update_YieldsThrowIfPossessionMissed()
        {
            var player = new Player();

            var frame = new Frame { Match = { GameClock = 300f - ThrowPlaySensor.SLAP_THRESHOLD_TIME - 1 } };
            var throwEvent = new ThrowEvent(frame, player);

            var sensor = new ThrowPlaySensor();
            sensor.Initialize(new IStateSensor[0]);

            //
            var result = sensor.Update(frame, new[] { throwEvent });
            //
            var slap = result as ThrowPlay;
            Assert.IsNotNull(slap);
            Assert.AreEqual(player, slap.Player);
        }
    }
}
