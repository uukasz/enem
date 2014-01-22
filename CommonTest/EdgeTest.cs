using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common;
using System.Collections.Generic;

namespace CommonTest
{
    [TestClass]
    public class EdgeTest
    {
        [TestMethod]
        public void ReturnToStringAsPointCoordinates()
        {
            Point p1 = new Point(15.888, 3456.998);
            Point p2 = new Point(0.6666, 999.666);

            Edge edge = new Edge(p1, p2);

            Assert.AreEqual("[ " + p1 + ", " + p2 + " ]", edge.ToString());
        }

        [TestMethod]
        public void ReturnToStringFromDefaultEdge()
        {
            Edge edge = new Edge();

            Assert.AreEqual("[ " + new Point() + ", " + new Point() + " ]", edge.ToString());
        }

        [TestMethod]
        public void TestRevertEdgesEquality()
        {
            Point p1 = new Point(1.0, 2.0);
            Point p2 = new Point(15.0, 21.0);

            Edge edge1 = new Edge(p1, p2);
            Edge edge2 = new Edge(p2, p1);

            Assert.AreEqual(edge1, edge2);
        }

        [TestMethod]
        public void DivideToTwoEdges()
        {
            Edge edge = new Edge(new Point(1.0, 1.0), new Point(5.0, 1.0));

            List<Edge> divided = edge.Divide(2);

            List<Edge> expected = new List<Edge>() 
            {
                new Edge(new Point(1.0, 1.0), new Point(3.0, 1.0)),
                new Edge(new Point(3.0, 1.0), new Point(5.0, 1.0))
            };

            for (int i = 0; i < expected.Count; ++i )
            {
                Assert.AreEqual(expected[i], divided[i]);
            }
        }

        [TestMethod]
        public void DivideEdgeToZeroElements()
        {
            Edge edge = new Edge(new Point(1.0, 1.0), new Point(5.0, 1.0));

            List<Edge> divided = edge.Divide(0);

            Assert.AreEqual(0, divided.Count);
        }
    }
}
