using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using OfficeOpenXml;
using RevitAddIn1.Bai4Parameter.RenameSheet.View;
using RevitAddIn1.Bai4Parameter.RenameSheet.ViewModel;
using RevitAddIn1.Bai5EdittingCreating.CreateSheet.View;
using RevitAddIn1.Bai5EdittingCreating.CreateSheet.ViewModel;

namespace RevitAddIn1.Bai5EdittingCreating.CreateSheet
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CreateSheetCmd : ExternalCommand
    {
        public override void Execute()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var vm = new CreateSheetViewModel(Document);
            var view = new CreateSheetView(){DataContext = vm};

            vm.CreateSheetView = view;
            view.ShowDialog();
        }
    }
}
