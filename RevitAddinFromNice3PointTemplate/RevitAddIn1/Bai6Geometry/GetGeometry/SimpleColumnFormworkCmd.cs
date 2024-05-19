using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using MoreLinq;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Utils;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace RevitAddIn1.Bai6Geometry.GetGeometry
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class SimpleColumnFormworkCmd : ExternalCommand
    {
        public override void Execute()
        {
            DocumentUtils.Document = Document;

            var column = UiDocument.Selection.PickObject(ObjectType.Element, "Select column").ToElement();

            var columnBb = column.get_BoundingBox(null);

            // Create a Outline, uses a minimum and maximum XYZ point to initialize the outline. 
            Outline myOutLn = new Outline(columnBb.Min, columnBb.Max);

            // Create a BoundingBoxIntersects filter with this Outline
            BoundingBoxIntersectsFilter filter = new BoundingBoxIntersectsFilter(myOutLn,20.MmToFeet());

            var beams = new FilteredElementCollector(Document).OfClass(typeof(FamilyInstance))
                .OfCategory(BuiltInCategory.OST_StructuralFraming).WherePasses(filter).ToList();


            var beamSolids = beams.SelectMany(x => x.GetAllSolids()).ToList();
            var allSolids = column.GetAllSolids();
            try
            {
                using (var tx = new Transaction(Document, "Create directshape"))
                {
                    tx.Start();

                    foreach (var solid in allSolids)
                    {
                        var faces = solid.Faces.Flatten().Where(x=>x is PlanarFace).Cast<PlanarFace>().ToList();

                        var verticalFaces = faces.Where(x => x.FaceNormal.IsPerpendicular(XYZ.BasisZ)).ToList();


                        foreach (var sideFace in verticalFaces)
                        {
                            var area= sideFace.Area;
                            var sideSolid = GeometryCreationUtilities.CreateExtrusionGeometry(
                                new List<CurveLoop>() { sideFace.GetEdgesAsCurveLoops().FirstOrDefault() },
                                sideFace.FaceNormal, 10.MmToFeet());

                            beamSolids.ForEach(x=>BooleanOperationsUtils.ExecuteBooleanOperationModifyingOriginalSolid(sideSolid,x,BooleanOperationsType.Difference));

                            CreateDirectShapeFromSolids(new List<Solid>() { sideSolid });
                        }
                    }
          
                    tx.Commit();
                }
            }
            catch (OperationCanceledException e)
            {
                MessageBox.Show("Ban Da Huy pick doi tuong", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private DirectShape CreateDirectShapeFromSolids(List<Solid> solids)
        {
            var ds = DirectShape.CreateElement(Document, new ElementId(BuiltInCategory.OST_GenericModel));

            ds.SetShape(new List<GeometryObject>(solids));

            return ds;
        }
    }



   
}