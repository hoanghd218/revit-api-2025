using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
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
    public class PickWallCmd : ExternalCommand
    {
        public override void Execute()
        {

            try
            {
                var reference = UiDocument.Selection.PickObject(ObjectType.Element, new WallSelectionFilter(), "Ban Hay chon Wall nhe!");
                var ele = Document.GetElement(reference);
                MessageBox.Show(ele.Name);
            }
            catch (OperationCanceledException e)
            {
                MessageBox.Show("Ban Da Huy pick doi tuong", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
    }



    public class WallSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            if (elem.Category.Name == "Walls")
            {
                return true;
            }


            return false;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return false;
        }
    }
}