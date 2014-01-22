using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Common;

namespace CommonTest
{
    [TestClass]
    public class PointTest
    {
        [TestMethod]
        public void ReturnCoordinatesAsToString()
        {
            Point p = new Point(1.255, 999.9999);
            Assert.AreEqual("(" + 1.255 + ", " + 999.9999 + ")", p.ToString());
        }

        [TestMethod]
        public void ReturnCreateDefaultPointAndReturnToString()
        {
            Point p = new Point();
            Assert.AreEqual("(" + 0.0 + ", " + 0.0 + ")", p.ToString());
        }
    }
}
