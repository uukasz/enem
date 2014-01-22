using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mesh.FiniteElement;
using Common;
using System.Collections.Generic;

namespace MeshTest.FiniteElement
{
    [TestClass]
    public class AbstractElementTest
    {
        [TestMethod]
        public void TestDivideAbstractElement()
        {
            Point p1 = new Point(11.11, 11.12);
            Point p2 = new Point(22.21, 22.22);
            Point p3 = new Point(33.31, 33.32);

            Edge e1 = new Edge(p1, p2);
            Edge e2 = new Edge(p2, p3);
            Edge e3 = new Edge(p3, p1);

            AbstractElement element = new AbstractElement(e1, e2, e3);

            List<AbstractElement> divided = element.Divide();

            Assert.AreEqual(element, divided[0]);
        }
    }
}
