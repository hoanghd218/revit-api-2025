using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.SelectionFilter;
using RevitAddIn1.Utils;

namespace RevitAddIn1.Bai5EdittingCreating.CopyElement
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CopyOneElementCmd : ExternalCommand
    {
        public override void Execute()
        {

            DocumentUtils.Document = Document;

            var column = UiDocument.Selection.PickObject(ObjectType.Element, new ColumnSelectionFilter(),
                "Select column to copy").ToElement();


            using (var tx = new Transaction(Document,"Move"))
            {
                tx.Start();
                ElementTransformUtils.CopyElement(Document,column.Id,new XYZ(10.MetToFeet(),0,0));

                tx.Commit();
            }
        }
    }
}
