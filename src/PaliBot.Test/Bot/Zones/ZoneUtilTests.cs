using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaliBot.Zones;
using System.Linq;
using System.Numerics;

namespace PaliBot.Test.Bot.Zones
{
    [TestClass]
    public class ZoneUtilTests
    {
        [TestMethod("Locate returns zone when point inside")]
        public void Locate_ReturnsZone()
        {
            var mid = ZoneData.MapZones.Single(z => z.Region == RegionKey.Neutral && z.Key == ZoneKey.Mid);

            var position = mid.Center;
            var result = ZoneUtil.Locate(position);

            Assert.AreEqual(ZoneKey.Mid, result.Key);
        }

        [TestMethod("Locate returns null when no zones match")]
        public void Locate_ReturnsNull()
        {
            var mid = ZoneData.MapZones.OfType<SphericalZone>().Single(z => z.Region == RegionKey.Neutral && z.Key == ZoneKey.Mid);

            var position = mid.Center + new Vector3(mid.Radius + 0.1f, 0f, 0f);
            var result = ZoneUtil.Locate(position);

            Assert.IsNull(result);
        }

        //[TestMethod]
        //public void Locate_ReturnsOrangeGoal()
        //{
        //    var position = new Vector3(1f, 1f, 35.5f);
        //    var result = ZoneUtil.Locate(position);

        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(RegionKey.Orange, result.Region);
        //    Assert.AreEqual(ZoneKey.Goal, result.Key);
        //}

        //[TestMethod]
        //public void Locate_DoesNotReturnOrangeGoal()
        //{
        //    var position = new Vector3(1f, 1f, 35f);
        //    var result = ZoneUtil.Locate(position);

        //    Assert.IsNull(result);
        //}
    }
}
