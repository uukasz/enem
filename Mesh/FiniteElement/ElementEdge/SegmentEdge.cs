using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mesh.FiniteElement.ElementEdge
{
    /// <summary>
    /// Krawedz w postaci odninka prostego
    /// </summary>
    public class SegmentEdge : Edge
    {
        #region CONSTRUCTORS

        public SegmentEdge(Point p1, Point p2)
            : base(p1, p2)
        {
            Children = new List<Edge>();
            IsDivided = false;
        }

        public SegmentEdge(Edge e)
            : this(e.P1, e.P2)
        { }

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Czy krawedz zostala podzielona
        /// </summary>
        public bool IsDivided { get; private set; }

        /// <summary>
        /// Lista krawedzi powstalych po podziale tej krawedzi
        /// </summary>
        public List<Edge> Children { get; private set; }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// podziel krawedz, zapamietaj dzieci i zapisz ze jest podzielona
        /// </summary>
        /// <param name="n">ilosc powstalych po podziale krawedzi</param>
        /// <returns></returns>
        public override List<Edge> Divide(int n)
        {
            // jesli jest juz podzielona, to nie dziel wiecej
            if (IsDivided)
            {
                return Children;
            }

            // jesli nie jest, to podziel i zapisz wynik
            List<Edge> edges = base.Divide(n);

            foreach (Edge e in edges)
            {
                Children.Add(new SegmentEdge(e));
            }

            IsDivided = true;

            return Children;
        }

        /// <summary>
        /// sprawdza jeszcze typ obiektu, 
        /// jesli to nie jest SegmentEdge, to nie jest rowny
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType().Equals(typeof(SegmentEdge)))
            {
                return base.Equals(obj);
            }
            else
            {
                return false;
            }

        }

        #endregion
    }
}
