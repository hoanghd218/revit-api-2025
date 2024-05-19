using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
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
    public class SplitVolumnForBeamCmd : ExternalCommand
    {
        public override void Execute()
        {

            try
            {
                var reference = UiDocument.Selection.PickObject(ObjectType.Element, "Ban Hay chon ele nhe!");
                var ele = Document.GetElement(reference);

                var allSolids = ele.GetAllSolids();

                using (var tx = new Transaction(Document, "Create directshape"))
                {
                    tx.Start();


                    var newSolids=allSolids.SelectMany(x => x.SplitVolumes()).ToList();

                    foreach (var newSolid in newSolids)
                    {
                        CreateDirectShapeFromSolids(new List<Solid>(){ newSolid });
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