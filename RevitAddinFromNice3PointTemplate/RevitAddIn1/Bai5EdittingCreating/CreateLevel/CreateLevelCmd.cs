using Autodesk.Revit.Attributes;
using Nice3point.Revit.Toolkit.External;
using RevitAddIn1.Utils;

namespace RevitAddIn1.Bai5EdittingCreating.CreateLevel
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class CreateLevelCmd : ExternalCommand
    {
        public override void Execute()
        {

            DocumentUtils.Document = Document;

            using (var tx = new Transaction(Document, "Move"))
            {
                tx.Start();
              
                CreateLevel(Document, 3.MetToFeet());
                CreateLevel(Document, 6.MetToFeet());
                CreateLevel(Document, 9.MetToFeet());
                tx.Commit();
            }
        }

        Level CreateLevel(Document document, double elevation)
        {
            // Create a new level at the specified elevation
            Level level = Level.Create(document, elevation);

            if (level == null)
            {
                throw new Exception("Failed to create level.");
            }

            return level;
        }
    }
}
