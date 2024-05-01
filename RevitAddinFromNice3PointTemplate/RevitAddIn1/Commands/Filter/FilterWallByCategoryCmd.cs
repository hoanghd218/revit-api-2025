using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;

namespace RevitAddIn1.Commands.Filter
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class FilterWallByCategoryCmd : ExternalCommand
    {
        public override void Execute()
        {
            var wallsByCategory = new FilteredElementCollector(Document).OfCategory(BuiltInCategory.OST_Walls)
                .ToElements();

            MessageBox.Show(wallsByCategory.Count.ToString());
        }
    }
}
