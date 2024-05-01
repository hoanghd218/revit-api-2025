using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Toolkit.External;

namespace RevitAddIn1.Bai4Parameter.SetMark
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class SetMarkCmd : ExternalCommand
    {
        public override void Execute()
        {
            var columnRfs = UiDocument.Selection.PickObjects(ObjectType.Element, "Select columns");

            var columns = columnRfs.Select(x => Document.GetElement(x)).ToList();



            var markC1 = "C1";

            using (var tx = new Transaction(Document, "Set mark"))
            {
                tx.Start();

                columns.ForEach(x =>
                {
                    x.LookupParameter("Mark").Set(markC1);
                    x.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set("Comment for c1");
                    x.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS).Set("Comment for c1");
                    x.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM).Set(0.2);
                });

                tx.Commit();
            }
        }
    }
}
