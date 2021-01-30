using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaliBot.Test.Bot
{
    [TestClass]
    public class AnnouncerTests
    {
        [TestMethod]
        public void CoverageStint()
        {
            var announcer = new Announcer();
            announcer.Update(new Frame());
        }
    }
}
