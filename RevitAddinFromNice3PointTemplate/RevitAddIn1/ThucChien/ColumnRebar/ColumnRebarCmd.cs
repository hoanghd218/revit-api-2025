using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Bai6Geometry.CreatePilesFromCad.View;
using RevitAddIn1.Bai6Geometry.CreatePilesFromCad.ViewModel;
using RevitAddIn1.ThucChien.ColumnRebar.View;
using RevitAddIn1.ThucChien.ColumnRebar.ViewModel;
using RevitAddIn1.Utils;

namespace RevitAddIn1.ThucChien.ColumnRebar
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class ColumnRebarCmd : ExternalCommand
    {
        public override void Execute()
        {
            DocumentUtils.Document = Document;
            var vm = new ColumnRebarViewModel(Document, UiDocument);
            var view = new ColumnRebarView() { DataContext = vm};

            vm.ColumnRebarView = view;
            view.ShowDialog();
        }
    }
}
