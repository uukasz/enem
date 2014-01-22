using Common;
using Mesh.FiniteElement;
using Mesh.FiniteElement.ElementEdge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mesh
{
    public class RectangularMesh : FiniteElementMesh
    {
        #region CONSTRUCTORS

        /// <summary>
        /// utworzenie siatki z podanych elementow
        /// </summary>
        /// <param name="elements"></param>
        public RectangularMesh(params AbstractElement[] elements)
            : base(elements)
        {

        }

        /// <summary>
        /// utworzenie siatki prostokatnej z podanych punktow
        /// </summary>
        /// <param name="p1">pierwszy wierzcholek prostokata</param>
        /// <param name="p2">przeciwlegly wierzcholek prostokata</param>
        public RectangularMesh(Point p1, Point p2)
        {
            // zamien liste wierzcholkow na wszystkie wierzcholki prostokata
            Point[] newVertices = new Point[]
            {
                p1,
                new Point(p1.X, p2.Y),
                p2,
                new Point(p2.X, p1.Y)
            };

            // utworz z nich krawedzie siatki
            List<Edge> newEdges = new List<Edge>()
            {
                new SegmentEdge(newVertices[0], newVertices[1]),
                new SegmentEdge(newVertices[1], newVertices[2]),
                new SegmentEdge(newVertices[2], newVertices[3]),
                new SegmentEdge(newVertices[3], newVertices[0])
            };

            Edge middleEdge = new SegmentEdge(newVertices[0], newVertices[2]);
            newEdges.Add(middleEdge);

            // utworz z tych krawedzi elementy
            Elements = new List<AbstractElement>()
            {
                new TriangleElement(newEdges[0], newEdges[1], middleEdge),
                new TriangleElement(newEdges[2], newEdges[3], middleEdge)
            };
        }

        #endregion

    }
}
