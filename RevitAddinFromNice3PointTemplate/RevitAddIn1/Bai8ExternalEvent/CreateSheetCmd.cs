using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using OfficeOpenXml;
using RevitAddIn1.Bai8ExternalEvent.View;
using RevitAddIn1.Bai8ExternalEvent.ViewModel;
using RevitAddIn1.Utils;

namespace RevitAddIn1.Bai8ExternalEvent
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CreateSheetCmd2: ExternalCommand
    {
        public override void Execute()
        {
            var externalEvent=DocumentUtils.ExternalEvent;

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var vm = new CreateSheetViewModel2(Document);
            var view = new CreateSheetView2(){DataContext = vm};

            vm.CreateSheetView2 = view;
            view.Show();
        }
    }
}
