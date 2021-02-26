﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaliBot.Model.Frame;
using PaliBot.Model.Play;
using System;

namespace PaliBot.Test.Bot.Model
{
    [TestClass]
    public class ThrowPlayTests
    {
        [TestMethod]
        public void InstallsPlayerAndHand()
        {
            var player = new Player();
            var play = new ThrowPlay(player);

            Assert.AreEqual(player, play.Player);
        }

        [TestMethod]
        public void Describe_ReturnsTextWithPlayerName()
        {
            var name = new Random().Next(10000).ToString("00000");

            var player = new Player
            {
                Name = name
            };
            var play = new ThrowPlay(player);
            var result = play.Describe();

            Assert.IsFalse(String.IsNullOrWhiteSpace(result));
            Assert.IsTrue(result.Contains(name));
        }
    }
}
