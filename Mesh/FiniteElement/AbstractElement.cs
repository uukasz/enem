using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesh.FiniteElement
{
    public class AbstractElement
    {
        #region CONSTRUCTORS

        public AbstractElement(params Point[] points)
        {
            RecreateEdges(points);
        }

        public AbstractElement(params Edge[] edges)
        {
            Edges = new List<Edge>(edges);
        }

        #endregion

        #region PROPERTIES

        /// <summary>
        /// zbior punktow, ktore zawieraja sie w krawedziach elementu
        /// </summary>
        public List<Point> Points
        {
            get 
            {
                var points = Edges.Select(e => e.P1).Concat(Edges.Select(e => e.P2)).Distinct();
                return points.ToList(); 
            }
        }

        private List<Edge> _edges;

        /// <summary>
        /// lista krawedzi elementu
        /// </summary>
        public List<Edge> Edges
        {
            get { return _edges; }
            set { _edges = value; }
        }

        #endregion

        #region PRIVATE METHODS

        /// <summary>
        /// odtwarza liste krawedzi z podanych punktow
        /// </summary>
        private void RecreateEdges(Point[] points)
        {
            _edges = new List<Edge>();
            foreach (Point p1 in points)
            {
                foreach (Point p2 in points)
                {
                    if (p1 != p2)
                    {
                        if (!Edges.Exists(edge => 
                            (edge.P1 == p1 && edge.P2 == p2) ||
                            (edge.P1 == p2 && edge.P2 == p1)))
                        {
                            Edges.Add(new Edge(p1, p2));
                        }
                    }
                }
            }
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// wyswietla liste punktow oddzielonych przecinkami
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            int pointsCount = Points.Count;

            StringBuilder elementString = new StringBuilder();
            elementString.Append("< ");

            for (int i = 0; i < pointsCount; ++i)
            {
                elementString.Append(Points[i]);
                if(i < pointsCount-1)
                {
                    elementString.Append(", ");
                }
            }

            elementString.Append(" >");

            return elementString.ToString();
        }

        /// <summary>
        /// wyswietla liste krawedzi oddzielonych przecinkami
        /// </summary>
        /// <returns></returns>
        public string PrintEdges()
        {
            int edgesCount = Edges.Count;

            StringBuilder elementString = new StringBuilder();
            elementString.Append("< ");

            for (int i = 0; i < edgesCount; ++i)
            {
                elementString.Append(Edges[i]);
                if (i < edgesCount - 1)
                {
                    elementString.Append(", ");
                }
            }

            elementString.Append(" >");

            return elementString.ToString();
        }

        /// <summary>
        /// podzial elementu (domyslnie zwraca siebie samego)
        /// </summary>
        /// <returns></returns>
        public virtual List<AbstractElement> Divide()
        {
            return new List<AbstractElement>() { this };
        }

        #endregion
    }
}
