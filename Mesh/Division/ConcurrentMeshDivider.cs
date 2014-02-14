using Mesh.FiniteElement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesh.Division
{
    public class ConcurrentMeshDivider : IMeshDivider
    {
        /// <summary>
        /// Podział elementów:
        /// - utworzenie tablicy sąsiedztwa elementów
        /// - pokolorowanie elementów tak, żeby żadne dwa kolory ze sobą nie sąsiadowały
        /// - elementy o tym samym kolorze można dzielić równolegle 
        /// - podział równoległy każdej listy elementów o tym samym kolorze
        /// </summary>
        /// <param name="elements">elementy do podziału</param>
        /// <returns>elementy po podziale</returns>
        public List<AbstractElement> DivideMesh(List<AbstractElement> elements)
        {
            if (elements == null)
            {
                throw new ArgumentNullException("elements");
            }

            List<AbstractElement> returnedElements = new List<AbstractElement>();


            return returnedElements;
        }

        private Dictionary<AbstractElement, List<AbstractElement>> getNeighboursGraph(List<AbstractElement> elements)
        {
            Dictionary<AbstractElement, List<AbstractElement>> graph = new Dictionary<AbstractElement, List<AbstractElement>>();

            return graph;
        }

        public event PercentCompletedHandler PercentCompleted;


        public double PercentCompletedDelta { get; private set; }
    }
}
