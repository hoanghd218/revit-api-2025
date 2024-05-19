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
    public class GetWallSolidCmd : ExternalCommand
    {
        public override void Execute()
        {

            try
            {
                var reference = UiDocument.Selection.PickObject(ObjectType.Element, "Ban Hay chon ele nhe!");
                var ele = Document.GetElement(reference);

                //var opt = new Options();
                //var geometryElement=ele.get_Geometry(opt);

                //foreach (GeometryObject geometryObject in geometryElement)
                //{
                //    if (geometryObject is Solid solid)
                //    {
                //        MessageBox.Show($"Volumn is {solid.Volume} ft3");
                //    }
                //}

                var allSolids = ele.GetAllSolids();
                var allSolidsBySymbol = ele.GetAllSolidsBySymbol(out var tf);

                using (var tx = new Transaction(Document, "Create directshape"))
                {
                    tx.Start();

                    CreateDirectShapeFromSolids(allSolids.Select(x => x.CreateTransformed(Transform.CreateTranslation(XYZ.BasisX * 10.MetToFeet()))).ToList());
                    //CreateDirectShapeFromSolids(allSolidsBySymbol.Select(x=>x.CreateTransformed(tf)).ToList());

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



    public class WallSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if (elem.Category.Name == "Walls")
            {
                return true;
            }


            return false;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}