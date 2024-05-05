using System.Windows;
using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Bai4Parameter.RenameSheet.View;
using RevitAddIn1.Bai4Parameter.RenameSheet.ViewModel;

namespace RevitAddIn1.Bai4Parameter.RenameSheet
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class RenameSheetCmd : ExternalCommand
    {
        public override void Execute()
        {
            var vm = new RenameSheetViewModel(Document);
            var view = new RenameSheetView(){DataContext = vm};

            vm.RenameSheetView = view;
            view.ShowDialog();
        }
    }
}
