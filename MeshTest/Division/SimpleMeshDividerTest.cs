using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mesh;
using Common;
using Mesh.Division;
using System.Collections.Generic;
using Mesh.FiniteElement;
using Mesh.FiniteElement.ElementEdge;

namespace MeshTest.Division
{
    [TestClass]
    public class SimpleMeshDividerTest
    {
        [TestMethod]
        public void TestSimpleMeshDivide()
        {
            FiniteElementMesh mesh = new RectangularMesh(new Point(0.0, 0.0), new Point(2.0, 2.0));

            IMeshDivider divider = new SimpleMeshDivider();
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestDivideNullElements()
        {
            List<AbstractElement> elements = null;
            IMeshDivider divider = new SimpleMeshDivider();
            divider.DivideMesh(elements);
        }

        [TestMethod]
        public void TestRaisePercentCompleted()
        {
            FiniteElementMesh mesh = new RectangularMesh(new Point(0.0, 0.0), new Point(2.0, 2.0));

            IMeshDivider divider = new SimpleMeshDivider();

            List<double> expectedPercentages = new List<double>(new double[]{0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1.0});
            List<double> percentagesCompleted = new List<double>();

            mesh.Elements = divider.DivideMesh(mesh.Elements);
            mesh.Elements = divider.DivideMesh(mesh.Elements);
            mesh.Elements = divider.DivideMesh(mesh.Elements);
            mesh.Elements = divider.DivideMesh(mesh.Elements);

            // podepnij pod event procentu zakonczonych podzialow lambde dodajaca kazdy procent do listy procentow
            divider.PercentCompleted +=
                (percent) =>
                {
                    percentagesCompleted.Add(percent);
                };

            mesh.Elements = divider.DivideMesh(mesh.Elements);

            Assert.AreEqual(expectedPercentages.Count, percentagesCompleted.Count);

            for (int i = 0; i < percentagesCompleted.Count; ++i)
            {
                int actual = (int)Math.Floor(10.0 * percentagesCompleted[i]);
                int expected = (int)Math.Floor(10.0 * expectedPercentages[i]);
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void TestRaisePercentCompletedWithTwoElements()
        {
            FiniteElementMesh mesh = new RectangularMesh(new Point(0.0, 0.0), new Point(2.0, 2.0));

            IMeshDivider divider = new SimpleMeshDivider();

            List<double> expectedPercentages = new List<double>(new double[] { 0.5, 1.0 });
            List<double> percentagesCompleted = new List<double>();

            // podepnij pod event procentu zakonczonych podzialow lambde dodajaca kazdy procent do listy procentow
            divider.PercentCompleted +=
                (percent) =>
                {
                    percentagesCompleted.Add(percent);
                };

            mesh.Elements = divider.DivideMesh(mesh.Elements);

            Assert.AreEqual(2, percentagesCompleted.Count);

            for (int i = 0; i < percentagesCompleted.Count; ++i )
            {
                Assert.AreEqual(expectedPercentages[i], percentagesCompleted[i]);
            }
        }
    }
}
