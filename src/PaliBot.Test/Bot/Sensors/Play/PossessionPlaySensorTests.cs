using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaliBot.Model.Event;
using PaliBot.Model.Frame;
using PaliBot.Model.Play;
using PaliBot.Sensors.Play;
using PaliBot.Sensors.State;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaliBot.Test.Bot.Sensors.Play
{
    [TestClass]
    public class PossessionPlaySensorTests
    {
        [TestMethod]
        public void Update_Sequence()
        {
            var player = new Player();

            var frame1 = new Frame { Match = { GameClock = 300f } };
            var possessionChangeEvent1 = new PossessionChangeEvent(frame1, player, HandSide.Left);

            var frame2 = new Frame { Match = { GameClock = 300f } };
            var possessionChangeEvent2 = new PossessionChangeEvent(frame1, null, null);

            var frame3 = new Frame { Match = { GameClock = 300f } };
            var possessionChangeEvent3 = new PossessionChangeEvent(frame1, player, HandSide.Right);

            var frame4 = new Frame { Match = { GameClock = 300f - PossessionPlaySensor.PICKUP_THRESHOLD / 2f } };
            var frame5 = new Frame { Match = { GameClock = 300f - PossessionPlaySensor.PICKUP_THRESHOLD } };
            var frame6 = new Frame { Match = { GameClock = 300f - (PossessionPlaySensor.PICKUP_THRESHOLD + PossessionPlaySensor.CARRY_THRESHOLD) / 2f } };
            var frame7 = new Frame { Match = { GameClock = 300f - PossessionPlaySensor.CARRY_THRESHOLD} };

            var sensor = new PossessionPlaySensor();
            sensor.Initialize(new IStateSensor[0]);

            //
            var result1 = sensor.Update(frame1, new[] { possessionChangeEvent1 });
            var result2 = sensor.Update(frame2, new[] { possessionChangeEvent2 });
            var result3 = sensor.Update(frame3, new[] { possessionChangeEvent3 });
            var result4 = sensor.Update(frame4, new GameEvent[0]);
            var result5 = sensor.Update(frame5, new GameEvent[0]);
            var result6 = sensor.Update(frame6, new GameEvent[0]);
            var result7 = sensor.Update(frame7, new GameEvent[0]);
            //

            Assert.IsNull(result1);
            Assert.IsNull(result2);
            Assert.IsNull(result3);
            Assert.IsNull(result4);

            var play5 = result5 as GrabDiscPlay;
            Assert.IsNotNull(play5);
            Assert.AreEqual(player, play5.Player);
            Assert.AreEqual(HandSide.Right, play5.Hand);

            Assert.IsNull(result6);

            var play7 = result7 as CarryPlay;
            Assert.IsNotNull(play7);
            Assert.AreEqual(player, play7.Player);
        }
    }
}
