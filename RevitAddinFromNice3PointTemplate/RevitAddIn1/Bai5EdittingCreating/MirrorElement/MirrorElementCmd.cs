using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.SelectionFilter;
using RevitAddIn1.Utils;
using System.Data.Common;

namespace RevitAddIn1.Bai5EdittingCreating.MirrorElement
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class MirrorElementCmd : ExternalCommand
    {
        public override void Execute()
        {

            DocumentUtils.Document = Document;

            var columns = UiDocument.Selection.PickObjects(ObjectType.Element, new ColumnSelectionFilter(),
                "Select column to mirror").Select(x => x.ToElement());


            var modelLine = UiDocument.Selection.PickObject(ObjectType.Element, "Model line").ToElement() as ModelLine;

            var line = modelLine.GeometryCurve as Line;
            var sp = line.GetEndPoint(0);
            var ep = line.GetEndPoint(1);

            var plane = Plane.CreateByThreePoints(sp, ep, sp.Add(XYZ.BasisZ));

            using (var tx = new Transaction(Document,"Mirror"))
            {
                tx.Start();

                ElementTransformUtils.MirrorElements(Document, columns.Select(x=>x.Id).ToList(), plane,false);


                tx.Commit();
            }
        }

    
    }
}
