using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    /// <summary>
    /// Podstawowa klasa krawedzi
    /// </summary>
    public class Edge
    {
        #region CONSTRUCTORS

        public Edge(Point p1, Point p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public Edge() : this(new Point(), new Point())
        { }

        #endregion

        #region PROPERTIES

        private Point _p1;

        public Point P1
        {
            get { return _p1; }
            set { _p1 = value; }
        }

        private Point _p2;

        public Point P2
        {
            get { return _p2; }
            set { _p2 = value; }
        }

        #endregion

        #region PUBLIC METHODS

        public override string ToString()
        {
            return "[ " + P1 + ", " + P2 + " ]";
        }

        public override bool Equals(object obj)
        {
            Edge edge = obj as Edge;

            if (edge == null) return false;

            return (P1.Equals(edge.P1) && P2.Equals(edge.P2)) 
                || (P1.Equals(edge.P2) && P2.Equals(edge.P1));
        }

        /// <summary>
        /// podziel ta krawedz na n krawedzi
        /// </summary>
        /// <param name="n">ilosc krawedzi po podziale</param>
        /// <returns></returns>
        public virtual List<Edge> Divide(int n)
        {
            List<Edge> dividedEdges = new List<Edge>();

            double dx = (P2.X - P1.X) / (double)n;
            double dy = (P2.Y - P1.Y) / (double)n;

            Point lastPoint = P1;

            for (int i = 0; i < n; ++i)
            {
                Point nextPoint = new Point(lastPoint.X + dx, lastPoint.Y + dy);

                if(nextPoint.Equals(P2))
                {
                    nextPoint = P2;
                }

                Edge nextEdge = new Edge(lastPoint, nextPoint);
                dividedEdges.Add(nextEdge);
                lastPoint = nextPoint;
            }

            return dividedEdges;
        }

        #endregion
    }
}
