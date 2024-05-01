using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Extensions;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.ViewModels;
using RevitAddIn1.Views;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace RevitAddIn1.Commands.Selection
{
    /// <summary>
    ///     External command entry point invoked from the Revit interface
    /// </summary>
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class PickObjectsCmd : ExternalCommand
    {
        public override void Execute()
        {

            try
            {
                var references = UiDocument.Selection.PickObjects(ObjectType.Element, new WallSelectionFilter(), "Ban Hay chon Wall nhe!");
                var eles = references.Select(x => Document.GetElement(x)).ToList();
                MessageBox.Show(string.Join(",", eles.Select(x => x.Id.ToString())));

                var totalLengthInMm = eles.Sum(x => x.LookupParameter("Length").AsDouble() * 304.8);

                MessageBox.Show($"Tong chieu dai Wall la : {totalLengthInMm} mm", "Infor");
            }
            catch (OperationCanceledException e)
            {
                MessageBox.Show("Ban Da Huy pick doi tuong", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
    }
}