using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB.Structure;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Utils;

namespace RevitAddIn1.Bai7Updater
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class WallUpdaterCmd : ExternalCommand
    {
        public override void Execute()
        {
            WallUpdater updater = new WallUpdater(UiApplication.ActiveAddInId);
            UpdaterRegistry.RegisterUpdater(updater);

            // Change Scope = any Wall element
            ElementClassFilter wallFilter = new ElementClassFilter(typeof(Wall));

            // Change type = element addition
            UpdaterRegistry.AddTrigger(updater.GetUpdaterId(), wallFilter,
                Element.GetChangeTypeElementAddition());

            UpdaterRegistry.AddTrigger(updater.GetUpdaterId(), wallFilter,
                Element.GetChangeTypeGeometry());
        }
    }
}
