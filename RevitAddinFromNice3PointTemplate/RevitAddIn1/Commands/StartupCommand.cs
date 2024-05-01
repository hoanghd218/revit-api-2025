using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.ViewModels;
using RevitAddIn1.Views;

namespace RevitAddIn1.Commands
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class StartupCommand : ExternalCommand
    {
        public override void Execute()
        {
            var a = 1;
            var b = 2;

            var c = a + b;


            var viewModel = new RevitAddIn1ViewModel();
            var view = new RevitAddIn1View(viewModel);
            view.ShowDialog();
        }
    }
}