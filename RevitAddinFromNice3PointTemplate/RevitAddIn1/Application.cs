using Nice3point.Revit.Toolkit.External;
using OfficeOpenXml;
using RevitAddIn1.Commands;
using OfficeOpenXml;
using RevitAddIn1.Bai4Parameter.RenameSheet;
using RevitAddIn1.Bai5EdittingCreating.CreateSheet;
using Autodesk.Revit.UI;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System;
using System.Security.Policy;
using RevitAddIn1.Bai4Parameter.ColumnBeamSlabConcreteVolume;
using RevitAddIn1.Bai4Parameter.SetMark;
using RevitAddIn1.Bai5EdittingCreating.CopyElement;

namespace RevitAddIn1
{
    /// <summary>
    ///     Application entry point
    /// </summary>
    [UsedImplicitly]
    public class Application : ExternalApplication
    {
        public override void OnStartup()
        {
            CreateRibbon();


            // if you have a commercial license
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private void CreateRibbon()
        {
            var panel = Application.CreatePanel( "Common Tools", "Revit 2025 API");

            var sb = panel.AddSplitButton("View", "View Tool");
            sb.AddPushButton<CreateSheetCmd>("Create Sheet")
                .SetImage("/RevitAddIn1;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/RevitAddIn1;component/Resources/Icons/RibbonIcon32.png");

            sb.AddPushButton<RenameSheetCmd>("Rename Sheet")
                .SetImage("/RevitAddIn1;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/RevitAddIn1;component/Resources/Icons/RibbonIcon32.png");

            panel.AddStackedItems(
                CreatePushButtonData<ColumnBeamSlabConcreteVolumeCmd>("Volume",
                    "/RevitAddIn1;component/Resources/Icons/RibbonIcon32.png",
                    "/RevitAddIn1;component/Resources/Icons/RibbonIcon16.png"),
                
                CreatePushButtonData<SetMarkCmd>("Set Mark", "/RevitAddIn1;component/Resources/Icons/RibbonIcon32.png",
                    "/RevitAddIn1;component/Resources/Icons/RibbonIcon16.png"),


                CreatePushButtonData<CopyOneElementCmd>("Copy One", "/RevitAddIn1;component/Resources/Icons/RibbonIcon32.png",
                    "/RevitAddIn1;component/Resources/Icons/RibbonIcon16.png")

            );
        }



        PushButtonData CreatePushButtonData<TCommand>(string buttonText,string uri32,string uri16)
            where TCommand : IExternalCommand, new()
        {
            var command = typeof(TCommand);
            var pushButtonData = new PushButtonData(command.FullName, buttonText, Assembly.GetAssembly(command)!.Location, command.FullName);


            pushButtonData.Image = new BitmapImage(new Uri(uri16, UriKind.RelativeOrAbsolute));
            pushButtonData.LargeImage = new BitmapImage(new Uri(uri32, UriKind.RelativeOrAbsolute));


            return pushButtonData;
        }
    }
}