using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Structure;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Utils;

namespace RevitAddIn1.Bai5EdittingCreating.CreateBeam
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CreateBeamCmd : ExternalCommand
    {
        public override void Execute()
        {

            DocumentUtils.Document = Document;

            var p1 = UiDocument.Selection.PickPoint("P1");
            var p2 = UiDocument.Selection.PickPoint("P2");

            var curve = Line.CreateBound(p1, p2);
            var beamType = new FilteredElementCollector(Document).OfClass(typeof(FamilySymbol))
                .OfCategory(BuiltInCategory.OST_StructuralFraming)
                .FirstOrDefault(x=>x.Name== "W920X223") as FamilySymbol;
            var level1 = ActiveView.GenLevel;



            using (var tx = new Transaction(Document, "Move"))
            {
                tx.Start();
                // create a new beam
                FamilyInstance instance = Document.Create.NewFamilyInstance(curve, beamType,
                    level1, StructuralType.Beam);

                instance.get_Parameter(BuiltInParameter.Z_OFFSET_VALUE).Set(0.01.MetToFeet());
                tx.Commit();
            }
        }


    }
}
