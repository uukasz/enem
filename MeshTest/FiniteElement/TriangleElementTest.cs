using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common;
using Mesh.FiniteElement;
using System.Collections.Generic;
using Mesh.FiniteElement.ElementEdge;

namespace MeshTest.FiniteElement
{
    [TestClass]
    public class TriangleElementTest
    {
        [TestMethod]
        public void TestReturnPointCoordinatesAsToString()
        {
            Point p1 = new Point(11.11, 11.12);
            Point p2 = new Point(22.21, 22.22);
            Point p3 = new Point(33.31, 33.32);

            TriangleElement tElem = new TriangleElement(p1, p2, p3);
            Assert.AreEqual("< " + p1 + ", " + p2 + ", " + p3 + " >", tElem.ToString());
        }

        [TestMethod]
        public void TestToStringFromAllNullPoints()
        {
            TriangleElement tElem = new TriangleElement(null as Point, null, null);
            Assert.AreEqual("<  >", tElem.ToString());
        }

        [TestMethod]
        public void TestPrintEdges()
        {
            Point p1 = new Point(11.11, 11.12);
            Point p2 = new Point(22.21, 22.22);
            Point p3 = new Point(33.31, 33.32);

            Edge e1 = new SegmentEdge(p1, p2);
            Edge e2 = new SegmentEdge(p2, p3);
            Edge e3 = new SegmentEdge(p3, p1);

            TriangleElement tElem = new TriangleElement(e1, e2, e3);
            Assert.AreEqual("< " + e1 + ", " + e2 + ", " + e3 + " >", tElem.PrintEdges());
        }

        [TestMethod]
        public void TestTriangleElementEquality()
        {
            Point p1 = new Point(11.11, 11.12);
            Point p2 = new Point(22.21, 22.22);
            Point p3 = new Point(33.31, 33.32);

            Edge e1 = new SegmentEdge(p1, p2);
            Edge e2 = new SegmentEdge(p2, p3);
            Edge e3 = new SegmentEdge(p3, p1);

            TriangleElement tElem = new TriangleElement(e1, e2, e3);

            Point p11 = new Point(11.11, 11.12);
            Point p12 = new Point(22.21, 22.22);
            Point p13 = new Point(33.31, 33.32);

            Edge e11 = new SegmentEdge(p11, p12);
            Edge e12 = new SegmentEdge(p12, p13);
            Edge e13 = new SegmentEdge(p13, p11);

            TriangleElement tElem1 = new TriangleElement(e11, e12, e13);

            Assert.AreEqual(tElem, tElem1);
        }

        [TestMethod]
        public void TestDivideElementToFourSmallerOnes()
        {
            Point p1 = new Point(0.0, 0.0);
            Point p2 = new Point(0.0, 2.0);
            Point p3 = new Point(2.0, 2.0);

            Edge e1 = new SegmentEdge(p1, p2);
            Edge e2 = new SegmentEdge(p2, p3);
            Edge e3 = new SegmentEdge(p3, p1);

            TriangleElement tElem = new TriangleElement(e1, e2, e3);

            List<AbstractElement> afterDivision = tElem.Divide();

            // utworz przewidziane elementy
            Point e1Middle = new Point(0.0, 1.0);
            Edge e11 = new SegmentEdge(p1, e1Middle);
            Edge e12 = new SegmentEdge(e1Middle, p2);

            Point e2Middle = new Point(1.0, 2.0);
            Edge e21 = new SegmentEdge(p2, e2Middle);
            Edge e22 = new SegmentEdge(e2Middle, p3);

            Point e3Middle = new Point(1.0, 1.0);
            Edge e31 = new SegmentEdge(p3, e3Middle);
            Edge e32 = new SegmentEdge(e3Middle, p1);

            Edge p1Opposite = new SegmentEdge(e1Middle, e3Middle);
            Edge p2Opposite = new SegmentEdge(e1Middle, e2Middle);
            Edge p3Opposite = new SegmentEdge(e2Middle, e3Middle);

            List<AbstractElement> expectedNewElements = new List<AbstractElement>()
            {
                new TriangleElement(e11, e32, p1Opposite),
                new TriangleElement(e12, e21, p2Opposite),
                new TriangleElement(e22, e31, p3Opposite),
                new TriangleElement(p1Opposite, p2Opposite, p3Opposite)
            };

            //Assert.AreEqual(expectedNewElements, afterDivision);
            for(int i=0; i<afterDivision.Count; ++i)
            {
                Assert.AreEqual(expectedNewElements[i], afterDivision[i]);
            }
        }

    }
}
