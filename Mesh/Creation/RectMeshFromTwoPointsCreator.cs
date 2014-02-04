using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesh.Creation
{
    public class RectMeshFromTwoPointsCreator : IMeshCreator
    {
        public RectMeshFromTwoPointsCreator()
        {
            InitialPoints = new List<Point>();
        }

        public FiniteElementMesh Mesh { get; private set; }

        public List<Point> InitialPoints { get; private set; }

        /// <summary>
        /// dodaj punkt do punktow poczatkowych siatki (i ew. utworz siatke)
        /// </summary>
        /// <param name="p">punkt dodawany do siatki</param>
        /// <returns>czy zostala utworzona siatka po dodaniu tego punktu</returns>
        public bool PutPoint(Point p)
        {
            // jesli istnieje juz siatka, to znaczy ze juz ja wypelniles
            if (Mesh != null)
            {
                return true;
            }

            // dodaj punkt p do punktow poczatkowych
            InitialPoints.Add(p);

            // jesli sa 2 punkty w pktach poczatkowych, utworz siatke
            if(InitialPoints.Count >= 2)
            {
                // utworz siatke z tych punktow
                Mesh = new RectangularMesh(InitialPoints[0], InitialPoints[1]);

                // powiadom, ze siatka jest utworzona
                return true;
            }

            return false;
        }
    }
}
