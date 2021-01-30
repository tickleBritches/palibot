using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaliBot.Numerics;
using PaliBot.Zones;
using System.Numerics;

namespace PaliBot.Test.Bot.Zones
{
    [TestClass]
    public class AlignedCubeZoneTests
    {
        [DataTestMethod]
        [DataRow(1, 0, 0)]
        [DataRow(9.5f, 0.5f, 0)]
        public void IntersectedBy_Yes_ReturnTrue(float dx, float dy, float dz)
        {
            var zone = new AlignedCubeZone(RegionKey.Neutral, ZoneKey.Goal, 10, 0, 0, 1, 1, 1);
            var ray = new Ray(0, 0, 0, dx, dy, dz);
            var result = zone.IntersectedBy(ray);

            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow(-1, 0, 0)]
        [DataRow(9.0f, 0.5f, 0)]
        public void IntersectedBy_No_ReturnFalse(float dx, float dy, float dz)
        {
            var zone = new AlignedCubeZone(RegionKey.Neutral, ZoneKey.Goal, 10, 0, 0, 1, 1, 1);
            var ray = new Ray(0, 0, 0, dx, dy, dz);
            var result = zone.IntersectedBy(ray);

            Assert.IsFalse(result);
        }

        [DataTestMethod]
        [DataRow(10, 0, 0)]
        [DataRow(10.5f, 0, 0)]
        [DataRow(10f, 0.5f, 0)]
        [DataRow(10f, 0, 0.5f)]
        public void Contains_Yes_ReturnsTrue(float x, float y, float z)
        {
            var zone = new AlignedCubeZone(RegionKey.Neutral, ZoneKey.Goal, 10, 0, 0, 1, 1, 1);
            var position = new Vector3(x, y, z);
            var result = zone.Contains(position);

            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow(10.6f, 0, 0)]
        [DataRow(10f, 0.6f, 0)]
        [DataRow(10f, 0, 0.6f)]
        public void Contains_No_ReturnsFalse(float x, float y, float z)
        {
            var zone = new AlignedCubeZone(RegionKey.Neutral, ZoneKey.Goal, 10, 0, 0, 1, 1, 1);
            var position = new Vector3(x, y, z);
            var result = zone.Contains(position);

            Assert.IsFalse(result);
        }
    }
}
