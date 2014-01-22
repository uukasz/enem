using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common;
using Mesh.FiniteElement.ElementEdge;
using System.Collections.Generic;

namespace MeshTest.FiniteElement.ElementEdge
{
    [TestClass]
    public class SegmentEdgeTest
    {
        [TestMethod]
        public void TestCreateSegmentEdgeFromTwoPoints()
        {
            Point p1 = new Point(15.888, 3456.998);
            Point p2 = new Point(0.6666, 999.666);

            Edge edge = new SegmentEdge(p1, p2);

            Assert.AreEqual("[ " + p1 + ", " + p2 + " ]", edge.ToString());
        }

        [TestMethod]
        public void TestCreateSegmentEdgeFromEdge()
        {
            Edge edge = new Edge(
                new Point(15.888, 3456.998), 
                new Point(0.6666, 999.666)
                );

            SegmentEdge sEdge = new SegmentEdge(edge);

            Assert.AreEqual(sEdge.P1, edge.P1);
            Assert.AreEqual(sEdge.P2, edge.P2);
        }

        [TestMethod]
        public void TestDivideToTwoSegmentEdges()
        {
            Edge edge = new SegmentEdge(new Point(1.0, 1.0), new Point(5.0, 1.0));

            List<Edge> divided = edge.Divide(2);

            List<Edge> expected = new List<Edge>() 
            {
                new SegmentEdge(new Point(1.0, 1.0), new Point(3.0, 1.0)),
                new SegmentEdge(new Point(3.0, 1.0), new Point(5.0, 1.0))
            };

            for (int i = 0; i < expected.Count; ++i)
            {
                Assert.AreEqual(expected[i], divided[i]);
            }
        }

        [TestMethod]
        public void TestDivideTwoTimes()
        {
            Edge edge = new SegmentEdge(new Point(1.0, 1.0), new Point(5.0, 1.0));

            List<Edge> divided = edge.Divide(2);
            List<Edge> secondDivision = edge.Divide(2);

            List<Edge> expected = new List<Edge>() 
            {
                new SegmentEdge(new Point(1.0, 1.0), new Point(3.0, 1.0)),
                new SegmentEdge(new Point(3.0, 1.0), new Point(5.0, 1.0))
            };

            Assert.AreEqual(expected.Count, secondDivision.Count);
            for (int i = 0; i < expected.Count; ++i)
            {
                Assert.AreEqual(expected[i], secondDivision[i]);
            }
        }
    }
}
