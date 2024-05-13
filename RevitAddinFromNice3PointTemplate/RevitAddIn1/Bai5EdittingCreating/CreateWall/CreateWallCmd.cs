using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Analysis;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.SelectionFilter;
using RevitAddIn1.Utils;

namespace RevitAddIn1.Bai5EdittingCreating.CreateWall
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CreateWallCmd : ExternalCommand
    {
        public override void Execute()
        {

            DocumentUtils.Document = Document;

            var p1 = UiDocument.Selection.PickPoint("P1");
            var p2 = UiDocument.Selection.PickPoint("P2");

            var curve = Line.CreateBound(p1, p2);
            var wallType200 = new FilteredElementCollector(Document).OfClass(typeof(WallType))
                .FirstOrDefault(x => x.Name == "Generic - 200mm");
            var level1 = ActiveView.GenLevel;



            using (var tx = new Transaction(Document, "Move"))
            {
                tx.Start();

                Wall.Create(Document, curve, wallType200.Id, level1.Id, 3.MetToFeet(), 0.01.MetToFeet(), true, false);

                tx.Commit();
            }
        }


    }
}
