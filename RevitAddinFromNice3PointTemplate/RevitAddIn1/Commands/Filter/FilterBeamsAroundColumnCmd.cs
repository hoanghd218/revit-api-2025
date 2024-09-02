using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.SelectionFilter;
using RevitAddIn1.Utils;

namespace RevitAddIn1.Commands.Filter
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class FilterBeamsAroundColumnCmd : ExternalCommand
    {
        public override void Execute()
        {

            DocumentUtils.Document = Document;
            var column =
                UiDocument.Selection.PickObject(ObjectType.Element, new ColumnSelectionFilter(), "Select column...").ToElement();

            var bb = column.get_BoundingBox(null);

            Outline myOutLn = new Outline(bb.Min, bb.Max);

            BoundingBoxIntersectsFilter filter = new BoundingBoxIntersectsFilter(myOutLn,30.MmToFeet());

      
            var  beamsAroundColumn = new FilteredElementCollector(Document).OfClass(typeof(FamilyInstance))
                .OfCategory(BuiltInCategory.OST_StructuralFraming).
                WherePasses(filter).ToElements();

            MessageBox.Show(beamsAroundColumn.Count.ToString());

            UiDocument.Selection.SetElementIds(beamsAroundColumn.Select(x=>x.Id).ToList());
        }
    }
}
