using Nice3point.Revit.Toolkit.External;
using RevitAddIn2.Commands;

namespace RevitAddIn2
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
            var panel = Application.CreatePanel("Commands", "RevitAddIn2");

            panel.AddPushButton<StartupCommand>("Execute")
                .SetImage("/RevitAddIn2;component/Resources/Icons/RibbonIcon16.png")
                .SetLargeImage("/RevitAddIn2;component/Resources/Icons/RibbonIcon32.png");
        }
    }
}