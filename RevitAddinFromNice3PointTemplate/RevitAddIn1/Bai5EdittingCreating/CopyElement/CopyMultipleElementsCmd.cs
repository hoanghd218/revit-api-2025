using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.SelectionFilter;
using RevitAddIn1.Utils;

namespace RevitAddIn1.Bai5EdittingCreating.CopyElement
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CopyMultipleElementsCmd : ExternalCommand
    {
        public override void Execute()
        {

            DocumentUtils.Document = Document;

            var columns = UiDocument.Selection.PickObjects(ObjectType.Element, new ColumnSelectionFilter(),
                "Select columns to copy").Select(x=>x.ToElement()).ToList();


            using (var tx = new Transaction(Document,"Move"))
            {
                tx.Start();
                ElementTransformUtils.CopyElements(Document,columns.Select(x=>x.Id).ToList(),new XYZ(10.MetToFeet(),0,0));

                tx.Commit();
            }
        }
    }
}
