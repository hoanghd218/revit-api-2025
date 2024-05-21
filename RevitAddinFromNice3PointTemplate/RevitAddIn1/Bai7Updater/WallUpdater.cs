using RevitAddIn1.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAddIn1.Bai7Updater
{
    public class WallUpdater : IUpdater
    {
        static AddInId m_appId;
        static UpdaterId m_updaterId;
        WallType m_wallType = null;

        // constructor takes the AddInId for the add-in associated with this updater
        public WallUpdater(AddInId id)
        {
            m_appId = id;
            m_updaterId = new UpdaterId(m_appId, new Guid("FBFBF6B2-4C06-42d4-97C1-D1B4EB593EFF"));
        }

        public void Execute(UpdaterData data)
        {
            Document doc = data.GetDocument();

            foreach (ElementId addedElemId in data.GetAddedElementIds())
            {
                Wall wall = doc.GetElement(addedElemId) as Wall;
                
                wall.LookupParameter("Comments").Set(wall.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble().FeetToMet().ToString());
            }


            foreach (ElementId modifiedElement in data.GetModifiedElementIds())
            {
                Wall wall = doc.GetElement(modifiedElement) as Wall;

                wall.LookupParameter("Comments").Set(wall.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH).AsDouble().FeetToMet().ToString());
            }
        }

        public string GetAdditionalInformation()
        {
            return "Wall type updater example: updates all newly created walls to a special wall";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.FloorsRoofsStructuralWalls;
        }

        public UpdaterId GetUpdaterId()
        {
            return m_updaterId;
        }

        public string GetUpdaterName()
        {
            return "Wall Type Updater";
        }
    }
}
