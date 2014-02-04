using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mesh.Creation
{
    public interface IMeshCreator
    {
        FiniteElementMesh Mesh { get; }
        bool PutPoint(Point p);
    }
}
