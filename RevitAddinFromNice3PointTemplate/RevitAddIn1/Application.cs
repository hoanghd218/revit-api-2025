using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Commands;

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
        }

        private void CreateRibbon()
        {
            var panel = Application.CreatePanel("Commands", "RevitAddIn1");

            panel.AddPushButton<StartupCommand>("Execute 2")
                .SetImage("/RevitAddIn1;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/RevitAddIn1;component/Resources/Icons/RibbonIcon32.png");
        }
    }
}