using Mesh.FiniteElement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesh
{
    public class FiniteElementMesh
    {
        #region CONSTRUCTORS

        public FiniteElementMesh(params AbstractElement[] elements)
        {
            Elements = new List<AbstractElement>(elements);
        }

        #endregion

        #region PROPERTIES

        public List<AbstractElement> Elements
        {
            get;
            set;
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// podzial wszystkich elementow w siatce
        /// </summary>
        public virtual void DivideElements()
        {
            List<AbstractElement> newElements = new List<AbstractElement>();
            foreach (AbstractElement element in Elements)
            {
                newElements.AddRange(element.Divide());
            }

            Elements = newElements;
        }

        #endregion
    }
}
