using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.SelectionFilter;
using RevitAddIn1.Utils;

namespace RevitAddIn1.Bai5EdittingCreating.CopyElement
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CopyTemplateCmd : ExternalCommand
    {
        public override void Execute()
        {

            DocumentUtils.Document = Document;

            var templateDoc = Application.OpenDocumentFile("C:\\Template.rvt");

            var schedule = new FilteredElementCollector(templateDoc).OfClass(typeof(ViewSchedule)).Cast<ViewSchedule>()
                .FirstOrDefault(x => x.Name == "Template Schedule For Column");


            using (var tx = new Transaction(Document, "Move"))
            {
                tx.Start();
                ElementTransformUtils.CopyElements(templateDoc, new List<ElementId>() { schedule.Id }, Document,Transform.Identity, new CopyPasteOptions());

                tx.Commit();
            }


            templateDoc.Close(false);
        }
    }
}
