using Mesh;
using Mesh.FiniteElement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mesh.Division
{
    public interface IMeshDivider
    {
        List<AbstractElement> DivideMesh(List<AbstractElement> elements);
    }
}
