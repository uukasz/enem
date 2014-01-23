using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mesh;
using Common;
using Mesh.FiniteElement;
using System.Collections.Generic;
using Mesh.FiniteElement.ElementEdge;

namespace MeshTest
{
    [TestClass]
    public class RectangularMeshTest
    {
        [TestMethod]
        public void TestCreateMeshWithTwoElements()
        {
            Point p1 = new Point(0.0, 0.0);
            Point p2 = new Point(0.0, 2.0);
            Point p3 = new Point(2.0, 2.0);
            Point p4 = new Point(2.0, 0.0);

            Edge e1 = new SegmentEdge(p1, p2);
            Edge e2 = new SegmentEdge(p2, p3);
            Edge e3 = new SegmentEdge(p3, p4);
            Edge e4 = new SegmentEdge(p4, p1);
            Edge eMiddle = new SegmentEdge(p1, p3);

            TriangleElement tElem1 = new TriangleElement(e1, e2, eMiddle);
            TriangleElement tElem2 = new TriangleElement(e3, e4, eMiddle);

            RectangularMesh mesh = new RectangularMesh(tElem1, tElem2);

            AbstractElement[] expected = new AbstractElement[] { tElem1, tElem2 };

            for (int i = 0; i < expected.Length; ++i)
            {
                Assert.AreEqual(expected[i], mesh.Elements[i]);
            }
        }

        [TestMethod]
        public void TestCreateMeshFromTwoPoints()
        {
            Point p1 = new Point(0.0, 0.0);
            Point p2 = new Point(2.0, 2.0);

            RectangularMesh mesh = new RectangularMesh(p1, p2);

            // utworz z tych krawedzi elementy
            Point ep0 = new Point(0.0, 0.0);
            Point ep1 = new Point(0.0, 2.0);
            Point ep2 = new Point(2.0, 2.0);
            Point ep3 = new Point(2.0, 0.0);
            List<AbstractElement>  expectedElements = new List<AbstractElement>()
            {
                new TriangleElement(
                    new SegmentEdge(ep0, ep1),
                    new SegmentEdge(ep1, ep2),
                    new SegmentEdge(ep2, ep0)
                    ),
                new TriangleElement(
                    new SegmentEdge(ep2, ep3),
                    new SegmentEdge(ep3, ep0),
                    new SegmentEdge(ep0, ep2)
                    )
            };

            for (int i = 0; i < expectedElements.Count; ++i)
            {
                Assert.AreEqual(expectedElements[i], mesh.Elements[i]);
            }
        }
    }
}
