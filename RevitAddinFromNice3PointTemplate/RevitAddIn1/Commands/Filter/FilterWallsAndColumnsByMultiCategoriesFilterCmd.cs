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
    public class FilterWallsAndColumnsByMultiCategoriesFilterCmd : ExternalCommand
    {
        public override void Execute()
        {


            var wallAndColumnCategoryFilter = new ElementMulticategoryFilter(new List<BuiltInCategory>()
            {
                BuiltInCategory.OST_Columns,
                BuiltInCategory.OST_Walls
            });

            var wallsAndColumns = new FilteredElementCollector(Document, ActiveView.Id)
                .WherePasses(wallAndColumnCategoryFilter)
                .WhereElementIsNotElementType()
                .ToElements();

            MessageBox.Show(wallsAndColumns.Count.ToString());
        }
    }
}
