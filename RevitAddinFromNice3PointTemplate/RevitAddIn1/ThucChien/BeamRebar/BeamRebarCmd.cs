using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.ThucChien.BeamRebar.View;
using RevitAddIn1.ThucChien.BeamRebar.ViewModel;
using RevitAddIn1.Utils;

namespace RevitAddIn1.ThucChien.BeamRebar
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class BeamRebarCmd : ExternalCommand
    {
        public override void Execute()
        {
            DocumentUtils.Document = Document;
            var vm = new BeamRebarViewModel(Document, UiDocument);
            var view = new BeamRebarView() { DataContext = vm };

            vm.BeamRebarView = view;
            view.ShowDialog();
        }
    }
}
