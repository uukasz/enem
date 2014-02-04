using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mesh;
using Mesh.Division;
using Common;
using System.Collections.Generic;
using Mesh.FiniteElement;
using Mesh.FiniteElement.ElementEdge;

namespace MeshTest.Division
{
    [TestClass]
    public class ConcurrentMeshDividerTest
    {
        [TestMethod]
        public void TestDivide()
        {
            FiniteElementMesh mesh = new RectangularMesh(new Point(0.0, 0.0), new Point(2.0, 2.0));

            IMeshDivider divider = new ConcurrentMeshDivider();
            mesh.Elements = divider.DivideMesh(mesh.Elements);

            // przygotuj dane do porownania
            Point ep0 = new Point(0.0, 0.0);
            Point ep1 = new Point(0.0, 2.0);
            Point ep2 = new Point(2.0, 2.0);
            Point ep3 = new Point(2.0, 0.0);
            List<AbstractElement> expectedElements = new List<AbstractElement>()
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

            List<AbstractElement> expectedElementsAfterDivision = new List<AbstractElement>();
            foreach (AbstractElement e in expectedElements)
            {
                expectedElementsAfterDivision.AddRange(e.Divide());
            }

            // porownaj ilosc elementow i ich wartosci

            Assert.AreEqual(expectedElementsAfterDivision.Count, mesh.Elements.Count);

            for (int i = 0; i < expectedElements.Count; ++i)
            {
                Assert.AreEqual(expectedElementsAfterDivision[i], mesh.Elements[i]);
            }
        }
    }
}
