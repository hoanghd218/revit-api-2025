using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Structure;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Utils;

namespace RevitAddIn1.Bai5EdittingCreating.CreateGrid
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CreateGridCmd : ExternalCommand
    {
        public override void Execute()
        {

            DocumentUtils.Document = Document;

            var p1 = UiDocument.Selection.PickPoint("P1");
            var p2 = UiDocument.Selection.PickPoint("P2");


            var line = Line.CreateBound(p1, p2);

            using (var tx = new Transaction(Document, "Move"))
            {
                tx.Start();
                Grid.Create(Document, line);
                tx.Commit();
            }
        }


    }
}
