using Autodesk.Revit.UI.Selection;

namespace RevitAddIn1.SelectionFilter
{
    public class CadLinkSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            return elem is ImportInstance;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}
