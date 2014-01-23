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
    }
}
