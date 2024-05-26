using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Structure;

namespace RevitAddIn1.Utils
{
    public static class RebarUtils
    {
        public static double BarDiameter(this RebarBarType rebar)
        {
#if REVIT2022_OR_GREATER
            return rebar.BarNominalDiameter;
#else
    return rebar.BarDiameter;
#endif

        }
    }
}
