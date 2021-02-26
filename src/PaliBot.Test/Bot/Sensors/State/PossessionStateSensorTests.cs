using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaliBot.Model.Frame;
using PaliBot.Sensors.State;
using System.Numerics;

namespace PaliBot.Test.Bot.Sensors.State
{
    [TestClass]
    public class PossessionStateSensorTests
    {
        [DataTestMethod]
        [DataRow(GameStatus.PostMatch)]
        [DataRow(GameStatus.PostSuddenDeath)]
        [DataRow(GameStatus.PreMatch)]
        [DataRow(GameStatus.PreSuddenDeath)]
        [DataRow(GameStatus.RoundOver)]
        [DataRow(GameStatus.RoundStart)]
        [DataRow(GameStatus.Score)]
        [DataRow(GameStatus.SuddenDeath)]
        [DataRow(GameStatus.Unknown)]
        public void Update_NullIfNotPlaying(GameStatus status)
        {
            var frame = new Frame { Match = { Status = status } };
            var sensor = new PossessionStateSensor();
            //
            sensor.Update(frame);
            //
            Assert.IsNull(sensor.Player);
            Assert.IsNull(sensor.Hand);
        }

        [TestMethod]
        public void Update_NullIfNoLastPossessionPlayer()
        {
            var frame = new Frame { Match = { Status = GameStatus.Playing }, LastPossessionPlayer = null };
            var sensor = new PossessionStateSensor();
            //
            sensor.Update(frame);
            //
            Assert.IsNull(sensor.Player);
            Assert.IsNull(sensor.Hand);
        }

        [TestMethod]
        public void Update_NullIfDiscHasVelocity()
        {
            var player = new Player();

            var frame = new Frame
            {
                Match = { Status = GameStatus.Playing },
                Disc =
                {
                    Velocity = new Vector3(1f, 1f, 1f)
                },
                LastPossessionPlayer = player
            };

            var sensor = new PossessionStateSensor();
            //
            sensor.Update(frame);
            //
            Assert.IsNull(sensor.Player);
            Assert.IsNull(sensor.Hand);
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void Update_SelectsLeftHand(bool rightHandInRange)
        {
            var min = PossessionStateSensor.MIN_HAND_DISTANCE;

            var player = new Player
            {
                LeftHand =
                {
                    Position = new Vector3(min / 3, min / 3, min / 3)
                },
                RightHand =
                {
                    Position = rightHandInRange
                        ? new Vector3(min / 2, min / 2 , min / 2)
                        : new Vector3(min, min, min)
                }
            };

            var frame = new Frame
            {
                Match = { Status = GameStatus.Playing },
                Disc =
                {
                    Pose = { Position = new Vector3(0, 0, 0) }
                },
                LastPossessionPlayer = player
            };
            var sensor = new PossessionStateSensor();
            //
            sensor.Update(frame);
            //
            Assert.AreEqual(player, sensor.Player);
            Assert.AreEqual(HandSide.Left, sensor.Hand);
        }

        [DataTestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void Update_SelectsRightHand(bool leftHandInRange)
        {
            var min = PossessionStateSensor.MIN_HAND_DISTANCE;

            var player = new Player
            {
                LeftHand =
                {
                    Position = leftHandInRange
                        ? new Vector3(min / 2, min / 2 , min / 2)
                        : new Vector3(min, min, min)
                },
                RightHand =
                {
                    Position = new Vector3(min / 3, min / 3, min / 3)
                }
            };

            var frame = new Frame
            {
                Match = { Status = GameStatus.Playing },
                Disc =
                {
                    Pose = { Position = new Vector3(0, 0, 0) }
                },
                LastPossessionPlayer = player
            };
            var sensor = new PossessionStateSensor();
            //
            sensor.Update(frame);
            //
            Assert.AreEqual(player, sensor.Player);
            Assert.AreEqual(HandSide.Right, sensor.Hand);
        }
    }
}
