using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Utils;

namespace RevitAddIn1.Bai5EdittingCreating.CreateDimensionForLevels
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CreateDimensionForLevelsCmd : ExternalCommand
    {
        public override void Execute()
        {
            DocumentUtils.Document = Document;

            var levelsRef = UiDocument.Selection.PickObjects(ObjectType.Element, "Select levels");

     

            using (var tx = new Transaction(Document, "Create Dimensions"))
            {
                tx.Start();

                var sk=SketchPlane.Create(Document,
                    Plane.CreateByNormalAndOrigin(Document.ActiveView.ViewDirection, Document.ActiveView.Origin));

                Document.ActiveView.SketchPlane = sk;

                var p = UiDocument.Selection.PickPoint("P");
                var line = Line.CreateBound(p, p.Add(XYZ.BasisZ));
                var ra = new ReferenceArray();
                foreach (var reference in levelsRef)
                {
                    ra.Append(reference);
                }

                Document.Create.NewDimension(ActiveView, line, ra);

                tx.Commit();
            }
        }


    }
}
