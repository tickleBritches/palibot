using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaliBot.Model.Play;
using System;

namespace PaliBot.Test.Bot.Model
{
    [TestClass]
    public class GamePlayTests
    {
        private class TestGamePlay : GamePlay
        {
            public TestGamePlay(int priority) : base(priority)
            {
            }

            public override string Describe()
            {
                throw new NotImplementedException();
            }
        }

        [TestMethod]
        public void InstallsPriority()
        {
            var priority = new Random().Next(100);
            var play = new TestGamePlay(priority);
            Assert.AreEqual(priority, play.Priority);
        }
    }
}
