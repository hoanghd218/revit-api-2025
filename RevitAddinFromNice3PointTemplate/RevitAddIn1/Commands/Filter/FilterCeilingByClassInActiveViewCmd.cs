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
    public class FilterCeilingByClassInActiveViewCmd : ExternalCommand
    {
        public override void Execute()
        {
            var ceilings = new FilteredElementCollector(Document, ActiveView.Id).OfClass(typeof(Ceiling))
                .ToElements();

            var categories = ceilings.Select(x => x.Category.Name).DistinctBy(x => x).ToList();



            MessageBox.Show(string.Join(",",categories));
            MessageBox.Show(ceilings.Count.ToString());
        }
    }
}
