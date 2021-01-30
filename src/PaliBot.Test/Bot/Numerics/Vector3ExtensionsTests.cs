using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaliBot.Numerics;
using System;
using System.Numerics;

namespace PaliBot.Test.Bot.Numerics
{
    [TestClass]
    public class Vector3ExtensionsTests
    {
        [TestMethod]
        public void Normalized_ReturnsNormalized()
        {
            var v = new Vector3(1, 2, 3);
            var frameworkNormalized = Vector3.Normalize(v);

            var result = v.Normalized();

            Assert.AreEqual(frameworkNormalized, result);
        }

        [TestMethod]
        public void Dot_ReturnsDot()
        {
            var v1 = new Vector3(1, 2, 3);
            var v2 = new Vector3(3, 2, 1);
            var frameworkDot = Vector3.Dot(v1,v2);

            var result = v1.Dot(v2);

            Assert.AreEqual(frameworkDot, result);
        }

        [TestMethod]
        public void ToVector3_ThrowsIfNull()
        {
            float[] values = null;
            Assert.ThrowsException<Exception>(() => values.ToVector3());
        }

        [DataTestMethod]
        [DataRow()]
        [DataRow(0,1)]
        [DataRow(0, 1, 2, 3)]
        public void ToVector3_ThrowsWithout3Floats(params float[] values)
        {
            Assert.ThrowsException<Exception>(() => values.ToVector3());
        }

        [TestMethod]
        public void ToVector3_ReturnsVector3()
        {
            var values = new float[] { 1, 2, 3 };
            var result = values.ToVector3();

            Assert.AreEqual(1, result.X);
            Assert.AreEqual(2, result.Y);
            Assert.AreEqual(3, result.Z);
        }
    }
}
