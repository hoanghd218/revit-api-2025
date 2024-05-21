using System.Reflection;
using System.Windows;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;

namespace RevitAddIn1
{
    //public class Application : IExternalApplication
    //{
    //    public Result OnStartup(UIControlledApplication application)
    //    {
    //        application.CreateRibbonTab("Revit API 2025");

    //       var panelKienTruc= application.CreateRibbonPanel("Revit API 2025", "Kiến Trúc");
    //        application.CreateRibbonPanel("Revit API 2025", "Kiến Cấu");
    //        application.CreateRibbonPanel("Revit API 2025", "Điện nước");

    //        application.CreateRibbonPanel(Tab.AddIns,"Tét");

    //        var path=Assembly.GetExecutingAssembly().Location;

    //        var pushButtonDataRenameSheet = new PushButtonData("RenameSheet", "Rename Sheet", path,
    //            "RevitAddIn1.Bai4Parameter.RenameSheet.RenameSheetCmd");

    //        pushButtonDataRenameSheet.Image =
    //            new BitmapImage(
    //                new Uri(@"C:\Users\NC\AppData\Roaming\Autodesk\Revit\Addins\2025\RevitAddIn1\Resources\Icons\RibbonIcon16.png"));


    //        pushButtonDataRenameSheet.LargeImage =
    //            new BitmapImage(
    //                new Uri(@"C:\Users\NC\AppData\Roaming\Autodesk\Revit\Addins\2025\RevitAddIn1\Resources\Icons\RibbonIcon32.png"));


    //        var pushButtonDataCreateSheet = new PushButtonData("Create Sheet", "Create Sheet", path,
    //            "RevitAddIn1.Bai5EdittingCreating.CreateSheet.CreateSheetCmd");

    //        pushButtonDataCreateSheet.Image =
    //            new BitmapImage(
    //                new Uri(@"C:\Users\NC\AppData\Roaming\Autodesk\Revit\Addins\2025\RevitAddIn1\Resources\Icons\RibbonIcon16.png"));


    //        pushButtonDataCreateSheet.LargeImage =
    //            new BitmapImage(
    //                new Uri(@"C:\Users\NC\AppData\Roaming\Autodesk\Revit\Addins\2025\RevitAddIn1\Resources\Icons\RibbonIcon32.png"));

    //        pushButtonDataCreateSheet.ToolTip = "Create Sheets by import data from Excel";

    //        var sbd = new PulldownButtonData("View-Sheet", "View-Sheet");
    //        var sb = panelKienTruc.AddItem(sbd) as PulldownButton;
    //        sb.AddPushButton(pushButtonDataRenameSheet);
    //        sb.AddPushButton(pushButtonDataCreateSheet);

    //        //panelKienTruc.AddItem(pushButtonDataRenameSheet);
    //        //panelKienTruc.AddItem(pushButtonDataCreateSheet);

    //        return Result.Succeeded;
    //    }

    //    public Result OnShutdown(UIControlledApplication application)
    //    {
         
    //        return Result.Cancelled;
    //    }
    //}
}