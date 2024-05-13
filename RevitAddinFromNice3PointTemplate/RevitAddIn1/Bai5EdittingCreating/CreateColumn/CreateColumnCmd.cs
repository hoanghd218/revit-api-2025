using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Structure;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Utils;

namespace RevitAddIn1.Bai5EdittingCreating.CreateColumn
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CreateColumnCmd : ExternalCommand
    {
        public override void Execute()
        {

            DocumentUtils.Document = Document;

            var p1 = UiDocument.Selection.PickPoint("P1");
 
            var columnType = new FilteredElementCollector(Document).OfClass(typeof(FamilySymbol))
                .OfCategory(BuiltInCategory.OST_StructuralColumns)
                .FirstOrDefault(x=>x.Name== "600 x 750mm") as FamilySymbol;

            var level0 = ActiveView.GenLevel;
            var level1 = new FilteredElementCollector(Document).OfClass(typeof(Level))
                .FirstOrDefault(x => x.Name == "Level 1");

            using (var tx = new Transaction(Document, "Move"))
            {
                tx.Start();
                // create a new beam
                FamilyInstance instance = Document.Create.NewFamilyInstance(p1, columnType,
                    level0, StructuralType.Column);

                instance.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM).Set(level1.Id);
                instance.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_OFFSET_PARAM).Set(0.0);
                instance.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM).Set(0.01.MetToFeet());
                tx.Commit();
            }
        }


    }
}
