using Common;
using Mesh;
using Mesh.Creation;
using Mesh.FiniteElement;
using Mesh.FiniteElement.ElementEdge;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeshTest.Creation
{
    [TestClass]
    public class RectMeshFromTwoPointsCreatorTest
    {
        [TestMethod]
        public void TestPutPoint()
        {
            RectMeshFromTwoPointsCreator creator = new RectMeshFromTwoPointsCreator();
            creator.PutPoint(new Point(0.0, 0.0));
        }

        [TestMethod]
        public void TestCreateMeshFromTwoPoints()
        {
            Point p1 = new Point(0.0, 0.0);
            Point p2 = new Point(2.0, 2.0);

            RectMeshFromTwoPointsCreator creator = new RectMeshFromTwoPointsCreator();
            creator.PutPoint(p1);
            creator.PutPoint(p2);

            FiniteElementMesh mesh = creator.Mesh;

            // utworz z tych krawedzi elementy
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

            for (int i = 0; i < expectedElements.Count; ++i)
            {
                Assert.AreEqual(expectedElements[i], mesh.Elements[i]);
            }
        }

        [TestMethod]
        public void TestPutPointAfterMeshCreation()
        {
            Point p1 = new Point(0.0, 0.0);
            Point p2 = new Point(2.0, 2.0);

            RectMeshFromTwoPointsCreator creator = new RectMeshFromTwoPointsCreator();
            creator.PutPoint(p1);
            creator.PutPoint(p2);

            creator.PutPoint(new Point(452.88, 872.76));

            FiniteElementMesh mesh = creator.Mesh;

            // utworz z tych krawedzi elementy
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

            for (int i = 0; i < expectedElements.Count; ++i)
            {
                Assert.AreEqual(expectedElements[i], mesh.Elements[i]);
            }
        }
    }
}
