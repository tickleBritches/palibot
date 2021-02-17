using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaliBot.Model.Event;
using PaliBot.Model.Frame;
using PaliBot.Sensors.Event;
using PaliBot.Sensors.State;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace PaliBot.Test.Bot.Sensors.Event
{
    [TestClass]
    public class ThrowEventSensorTests
    {
        [TestMethod]
        public void Update_EmitsIfPossessionLostAndDiscIsFast()
        {
            var frame1 = new Frame();
            var frame2 = new Frame
            {
                Disc = { Velocity = new Vector3(ThrowEventSensor.SPEED_DELTA_THRESHOLD, 0, 0) }
            };

            var player = new Player { Velocity = new Vector3(0, 0, 0) };

            var mockPossessionStateSensor = new Mock<IPossessionStateSensor>();
            mockPossessionStateSensor.SetupSequence(x => x.Player)
                .Returns(player) // assignment ref
                .Returns((IPlayer)null) // predicate ref
            ;

            var sensor = new ThrowEventSensor();
            sensor.Initialize(new[] { mockPossessionStateSensor.Object });

            //

            var result1 = sensor.Update(frame1);
            var result2 = sensor.Update(frame2);

            //

            Assert.IsNull(result1);

            var ev = result2 as ThrowEvent;
            Assert.IsNotNull(ev);
            Assert.AreEqual(frame2, ev.Frame);
            Assert.AreEqual(player, ev.Player);
        }

        [TestMethod]
        public void Update_NullIfDiscIsSlow()
        {
            var frame1 = new Frame();
            var frame2 = new Frame
            {
                Disc = { Velocity = new Vector3(ThrowEventSensor.SPEED_DELTA_THRESHOLD / 2, 0, 0) }
            };

            var player = new Player { Velocity = new Vector3(0, 0, 0) };

            var mockPossessionStateSensor = new Mock<IPossessionStateSensor>();
            mockPossessionStateSensor.SetupSequence(x => x.Player)
                .Returns(player) // assignment ref
                .Returns((IPlayer)null) // predicate ref
            ;

            var sensor = new ThrowEventSensor();
            sensor.Initialize(new[] { mockPossessionStateSensor.Object });

            //

            var result1 = sensor.Update(frame1);
            var result2 = sensor.Update(frame2);

            //

            Assert.IsNull(result1);
            Assert.IsNull(result2);
        }

        [TestMethod]
        public void Update_NullIfPossessionNotLost()
        {
            var frame = new Frame();
            var player = new Player();

            var mockPossessionStateSensor = new Mock<IPossessionStateSensor>();
            mockPossessionStateSensor.SetupGet(x => x.Player).Returns(player);

            var sensor = new ThrowEventSensor();
            sensor.Initialize(new[] { mockPossessionStateSensor.Object });

            //

            var result1 = sensor.Update(frame);
            var result2 = sensor.Update(frame);

            //

            Assert.IsNull(result1);
            Assert.IsNull(result2);
        }
    }
}
