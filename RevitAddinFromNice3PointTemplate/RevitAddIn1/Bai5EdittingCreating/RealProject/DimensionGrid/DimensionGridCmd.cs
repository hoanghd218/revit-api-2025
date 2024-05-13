using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Bai4Parameter.RenameSheet.View;
using RevitAddIn1.Bai4Parameter.RenameSheet.ViewModel;
using RevitAddIn1.Bai5EdittingCreating.RealProject.DimensionGrid.View;
using RevitAddIn1.Bai5EdittingCreating.RealProject.DimensionGrid.ViewModel;
using RevitAddIn1.Utils;

namespace RevitAddIn1.Bai5EdittingCreating.RealProject.DimensionGrid
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class DimensionGridCmd : ExternalCommand
    {
        public override void Execute()
        {
            DocumentUtils.Document = Document;
            var vm = new DimensionGridViewModel(Document, UiDocument);
            var view = new DimensionGridView(){DataContext = vm};

            vm.DimensionGridView = view;
            view.ShowDialog();
        }
    }
}
