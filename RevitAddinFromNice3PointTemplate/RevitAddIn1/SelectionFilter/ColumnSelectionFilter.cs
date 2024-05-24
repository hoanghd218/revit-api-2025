using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI.Selection;

namespace RevitAddIn1.SelectionFilter
{
    public class ColumnSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
#if REVIT2024_OR_GREATER
            return elem.Category?.Id.Value == -2001330;
#else
            return elem.Category?.Id.IntegerValue == -2001330;
#endif
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}
