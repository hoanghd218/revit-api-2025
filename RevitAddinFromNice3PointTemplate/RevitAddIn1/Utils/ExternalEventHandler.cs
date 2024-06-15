using Autodesk.Revit.UI;

namespace RevitAddIn1.Utils
{
    public class ExternalEventHandler : IExternalEventHandler
    {
        protected static Action Action;

        protected static ExternalEventHandler instance { get; set; }

        public static ExternalEventHandler Instance
        {
            get
            {
                if (instance == null)
                    instance = new ExternalEventHandler();
                return instance;
            }
        }

        protected static ExternalEvent create { get; set; }

        public ExternalEvent Create()
        {
            if (create == null)
                create = ExternalEvent.Create((IExternalEventHandler)Instance);
            return create;
        }

        public void SetAction(Action parameter) => Action = parameter;

        public async void Run()
        {
            int num = (int)create.Raise();
            while (create.IsPending)
                await Task.Delay(10);
        }

        public void Execute(UIApplication app)
        {
            if (app.ActiveUIDocument == null)
            {
                int num = (int)TaskDialog.Show("Notification", " no document, nothing to do");
            }
            else
                Action();
        }

        public string GetName() => "BIMSpeedTools";
    }
}
