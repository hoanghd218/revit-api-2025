using Autodesk.Revit.UI;

namespace RevitAddIn1.Utils
{
    public static class DocumentUtils
    {
        private static ExternalEvent externalEvent;
        private static ExternalEventHandler externalEventHandler;
        public static ExternalEvent ExternalEvent
        {
            get
            {
                if (externalEvent == null)
                    externalEvent = ExternalEvent.Create(ExternalEventHandler);
                return externalEvent;
            }
            set => externalEvent = value;
        }

        public static ExternalEventHandler ExternalEventHandler
        {
            get
            {
                if (externalEventHandler == null)
                    externalEventHandler = new ExternalEventHandler();
                return externalEventHandler;
            }
            set => externalEventHandler = value;
        }

        public static Document Document;
        public static Element ToElement(this Reference rf) => Document.GetElement(rf);
        public static Element ToElement(this ElementId id) => Document.GetElement(id);
    }
}
