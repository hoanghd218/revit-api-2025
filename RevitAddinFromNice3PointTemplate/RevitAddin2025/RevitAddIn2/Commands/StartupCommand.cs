using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn2.Utils;
using RevitAddIn2.ViewModels;
using RevitAddIn2.Views;

namespace RevitAddIn2.Commands
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
            if (WindowController.Focus<RevitAddIn2View>()) return;

            var viewModel = new RevitAddIn2ViewModel();
            var view = new RevitAddIn2View(viewModel);
            WindowController.Show(view, UiApplication.MainWindowHandle);
        }
    }
}