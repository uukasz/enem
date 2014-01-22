using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Common;
using Mesh.FiniteElement.ElementEdge;

namespace Mesh.FiniteElement
{
    public class TriangleElement : AbstractElement
    {
        #region CONSTRUCTORS

        public TriangleElement(Point p1, Point p2, Point p3)
            : base(p1, p2, p3)
        { }

        public TriangleElement(Edge e1, Edge e2, Edge e3)
            : base(e1, e2, e3)
        { }

        #endregion

        #region PROPERTIES

        public Point P1
        {
            get { return Points[0]; }
            set { Points[0] = value; }
        }

        public Point P2
        {
            get { return Points[1]; }
            set { Points[1] = value; }
        }

        public Point P3
        {
            get { return Points[2]; }
            set { Points[2] = value; }
        }

        public Edge E1
        {
            get { return Edges[0]; }
            set { Edges[0] = value; }
        }

        public Edge E2
        {
            get { return Edges[1]; }
            set { Edges[1] = value; }
        }

        public Edge E3
        {
            get { return Edges[2]; }
            set { Edges[2] = value; }
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// zwraca 4 trojkatne elementy, ktore sa wynikim podzialu tego elementu
        /// </summary>
        /// <returns></returns>
        public override List<AbstractElement> Divide()
        {
            List<AbstractElement> newElements = new List<AbstractElement>();

            // podziel kazda krawedz
            List<Edge> newEdges = E1.Divide(2).Concat(E2.Divide(2)).Concat(E3.Divide(2)).ToList();

            // utworz liste na krawedzie laczace srodki istniejacych krawedzi
            List<Edge> middleEdges = new List<Edge>();

            // dla kazdego wierzcholka tego elementu
            foreach (Point p in Points)
            {
                // znajdz dwie wspoldzielace go krawedzie
                Edge pE1 = null, pE2 = null;
                foreach (Edge e in newEdges)
                {
                    if (e.P1 == p || e.P2 == p)
                    {
                        if (pE1 == null)
                        {
                            pE1 = e;
                        }
                        else
                        {
                            pE2 = e;
                            break;
                        }
                    }
                }

                // usun je z listy nowych krawedzi
                newEdges.Remove(pE1);
                newEdges.Remove(pE2);

                // utworz krawedz naprzeciwko tego wierzcholka
                Edge pOppositeEdge = new SegmentEdge(
                    pE1.P1 == p ? pE1.P2 : pE1.P1,
                    pE2.P1 == p ? pE2.P2 : pE2.P1
                    );

                // dodaj ja do listy krawedzi laczacych srodki
                middleEdges.Add(pOppositeEdge);

                // utworz nowy element z tych trzech krawedzi
                TriangleElement newElement = new TriangleElement(pE1, pE2, pOppositeEdge);

                // dodaj go do listy nowych elementow po podziale
                newElements.Add(newElement);
            }

            // utworz element z krawedzi laczacych srodki pierwotnych krawedzi
            TriangleElement middleElement = new TriangleElement(middleEdges[0], middleEdges[1], middleEdges[2]);

            // dodaj go do listy nowych elementow po podziale
            newElements.Add(middleElement);
            
            return newElements;
        }

        public override bool Equals(object obj)
        {
            TriangleElement element = obj as TriangleElement;
            if (element == null) return false;
            return E1.Equals(element.E1) && E2.Equals(element.E2) && E3.Equals(element.E3);
        }

        #endregion
    }
}
