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
    public class FilterColumnByCategoryAndClassCmd : ExternalCommand
    {
        public override void Execute()
        {
            var columnsByCategoryAndClass = new FilteredElementCollector(Document, ActiveView.Id)
                .OfCategory(BuiltInCategory.OST_Columns)
                .OfClass(typeof(FamilyInstance))
                .ToElements();

            MessageBox.Show(columnsByCategoryAndClass.Count.ToString());
        }
    }
}
