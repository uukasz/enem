using Mesh.FiniteElement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mesh.Division
{
    public class SimpleMeshDivider : IMeshDivider
    {
        /// <summary>
        /// dzieli wszystkiepodane elementy zwraca liste tych 
        /// elementow po podziale
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public List<AbstractElement> DivideMesh(List<AbstractElement> elements)
        {
            if (elements == null)
            {
                throw new ArgumentNullException("elements");
            }

            List<AbstractElement> newElements = new List<AbstractElement>();
            foreach (AbstractElement element in elements)
            {
                newElements.AddRange(element.Divide());
            }

            return newElements;
        }
    }
}
