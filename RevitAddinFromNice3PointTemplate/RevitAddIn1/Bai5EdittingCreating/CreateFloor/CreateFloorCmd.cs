using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Structure;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Utils;

namespace RevitAddIn1.Bai5EdittingCreating.CreateFloor
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CreateFloorCmd : ExternalCommand
    {
        public override void Execute()
        {

            DocumentUtils.Document = Document;


            Document doc = Document;

            // Get the FloorType
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> elements = collector.OfClass(typeof(FloorType)).ToElements();
            FloorType floorType = elements.FirstOrDefault() as FloorType;

            // Define the boundary curve(s) for the floor (for simplicity, using a rectangle)
            XYZ[] points = new XYZ[4];

            points[0] = new XYZ(0, 0, 10);
            points[1] = new XYZ(10, 0, 10);
            points[2] = new XYZ(10, 10, 10);
            points[3] = new XYZ(0, 10, 10);

            Curve ab = Line.CreateBound(points[0], points[1]);
            Curve bc = Line.CreateBound(points[1], points[2]);
            Curve cd = Line.CreateBound(points[2], points[3]);
            Curve da = Line.CreateBound(points[3], points[0]);

            var curveLoop = new CurveLoop();
            curveLoop.Append(ab);
            curveLoop.Append(bc);
            curveLoop.Append(cd);
            curveLoop.Append(da);

            // Create the floor
            Transaction transaction = new Transaction(doc, "Create Floor");
            transaction.Start();

            Floor.Create(doc, new List<CurveLoop>(){curveLoop}, floorType.Id, ActiveView.GenLevel.Id);

            transaction.Commit();


        }
    }
}
