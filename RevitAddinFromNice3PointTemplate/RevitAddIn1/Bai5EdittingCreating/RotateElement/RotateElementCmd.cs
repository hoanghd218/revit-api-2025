using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.SelectionFilter;
using RevitAddIn1.Utils;
using System.Data.Common;

namespace RevitAddIn1.Bai5EdittingCreating.RotateElement
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class RotateElementCmd : ExternalCommand
    {
        public override void Execute()
        {

            DocumentUtils.Document = Document;

            var columns = UiDocument.Selection.PickObjects(ObjectType.Element, new ColumnSelectionFilter(),
                "Select column to move").Select(x => x.ToElement());


            using (var tx = new Transaction(Document,"Move"))
            {
                tx.Start();

                foreach (var column in columns)
                {
                    var lc = column.Location as LocationPoint;
                    XYZ point1 = new XYZ(lc.Point.X, lc.Point.Y, 0);
                    XYZ point2 = new XYZ(lc.Point.X, lc.Point.Y, 30);
                    Line axis = Line.CreateBound(point1, point2);
                    ElementTransformUtils.RotateElement(Document, column.Id, axis, Math.PI / 3.0);
                }
            

                tx.Commit();
            }
        }

    
    }
}
