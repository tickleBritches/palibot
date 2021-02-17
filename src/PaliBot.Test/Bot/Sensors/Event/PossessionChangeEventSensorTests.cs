using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaliBot.Model.Event;
using PaliBot.Model.Frame;
using PaliBot.Sensors.Event;
using PaliBot.Sensors.State;

namespace PaliBot.Test.Bot.Sensors.Event
{
    [TestClass]
    public class PossessionChangeEventSensorTests
    {
        [TestMethod]
        public void Update_EmitsIfPlayerChanged()
        {
            var frame = new Frame();
            var player = new Player();

            var mockPossessionStateSensor = new Mock<IPossessionStateSensor>();
            mockPossessionStateSensor.SetupSequence(x => x.Player)
                .Returns((IPlayer)null) // predicate ref
                .Returns(player) // predicate ref
                .Returns(player) // assignment ref
                .Returns((IPlayer)null) // predicate ref
            ;
            mockPossessionStateSensor.SetupSequence(x => x.Hand)
                .Returns(HandSide.Right)
                .Returns((HandSide?)null)
            ;

            var sensor = new PossessionChangeEventSensor();
            sensor.Initialize(new[] { mockPossessionStateSensor.Object });

            //
            var result1 = sensor.Update(frame);
            var result2 = sensor.Update(frame);
            var result3 = sensor.Update(frame);
            //            
            
            Assert.IsNull(result1);

            var ev2 = result2 as PossessionChangeEvent;
            Assert.IsNotNull(ev2);
            Assert.AreEqual(frame, ev2.Frame);
            Assert.AreEqual(player, ev2.Player);
            Assert.AreEqual(HandSide.Right, ev2.Hand);

            var ev3 = result3 as PossessionChangeEvent;
            Assert.IsNotNull(ev3);
            Assert.AreEqual(frame, ev3.Frame);
            Assert.IsNull(ev3.Player);
            Assert.IsNull(ev3.Hand);
        }
    }
}
