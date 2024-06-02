using System.Windows;
using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Utils;

namespace RevitAddIn1.Bai4Parameter.ColumnBeamSlabConcreteVolume
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class ColumnBeamSlabConcreteVolumeCmd : ExternalCommand
    {
        public override void Execute()
        {
            if (!LicenseUtils.IsLicenseValid())
            {
                return;
            }

            var columnBeamSlabsFilter = new ElementMulticategoryFilter(new List<BuiltInCategory>()
            {
                BuiltInCategory.OST_StructuralColumns,
                BuiltInCategory.OST_Floors,
                BuiltInCategory.OST_StructuralFraming,
            });

            var columnBeamSlabs = new FilteredElementCollector(Document, ActiveView.Id)
                .WherePasses(columnBeamSlabsFilter)
                .WhereElementIsNotElementType()
                .ToElements();

            MessageBox.Show(columnBeamSlabs.Count.ToString());

            var totalVolume = columnBeamSlabs.Sum(x => x.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble() * 0.02831685);
            MessageBox.Show($"Total volume is {totalVolume} m3" );
        }
    }
}
