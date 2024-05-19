using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Bai5EdittingCreating.RealProject.DimensionGrid.View;
using RevitAddIn1.Bai5EdittingCreating.RealProject.DimensionGrid.ViewModel;
using RevitAddIn1.Bai6Geometry.CreatePilesFromCad.View;
using RevitAddIn1.Bai6Geometry.CreatePilesFromCad.ViewModel;
using RevitAddIn1.Utils;

namespace RevitAddIn1.Bai6Geometry.CreatePilesFromCad
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CreatePileFromCadCmd : ExternalCommand
    {
        public override void Execute()
        {
            DocumentUtils.Document = Document;
            var vm = new CreatePileFromCadViewModel(Document, UiDocument);
            var view = new CreatePileFromCadView { DataContext = vm};

            vm.CreatePileFromCadView = view;
            view.ShowDialog();
        }
    }
}
