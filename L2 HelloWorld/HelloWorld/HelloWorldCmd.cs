using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace HelloWorld
{
    [Transaction(TransactionMode.Manual)]
    public class HelloWorldCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {



            MessageBox.Show("Hello world, I'm learning Revit API 2025", "Revit api 2025", MessageBoxButton.OK, MessageBoxImage.Information);
            return Result.Succeeded;
        }
    }
}
