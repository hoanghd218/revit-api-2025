using System.IO;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Newtonsoft.Json;
using RevitAddIn1.Bai5EdittingCreating.RealProject.DimensionGrid.Model;
using RevitAddIn1.Bai5EdittingCreating.RealProject.DimensionGrid.View;
using RevitAddIn1.SelectionFilter;
using RevitAddIn1.Utils;

namespace RevitAddIn1.Bai5EdittingCreating.RealProject.DimensionGrid.ViewModel
{
    public class DimensionGridViewModel : ObservableObject
    {
        public List<DimensionType> DimensionTypes { get; set; } = new List<DimensionType>();

        public DimensionType SelectedDimensionType { get; set; }

        public double Distance { get; set; } = 5.0;

        public RelayCommand OkCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }

        private Document doc;
        private UIDocument uiDoc;
        public DimensionGridView DimensionGridView { get; set; }

        private string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                              "\\HocRevitAPI\\DimensionGrid.json";
        public DimensionGridViewModel(Document doc, UIDocument uiDoc)
        {
            this.doc = doc;
            this.uiDoc = uiDoc;
            OkCommand = new RelayCommand(Run);
            CloseCommand = new RelayCommand(Close);

            GetData();
        }

        void GetData()
        {
            DimensionTypes = new FilteredElementCollector(doc).OfClass(typeof(DimensionType)).Cast<DimensionType>()
                .Where(x => x.StyleType == DimensionStyleType.Linear || x.StyleType == DimensionStyleType.LinearFixed)
                .OrderBy(x => x.Name).ToList();


            SelectedDimensionType = DimensionTypes.FirstOrDefault();


            LoadData();

        }

        void Run()
        {
            DimensionGridView.Close();

            var allGrids = new FilteredElementCollector(doc, doc.ActiveView.Id).OfClass(typeof(Grid)).Cast<Grid>()
                .ToList();

            var allGridModels = allGrids.Select(x => new GridModel(x)).ToList();
            using (var tx = new Transaction(doc, "Create dim"))
            {
                tx.Start();

                while (true)
                {

                    Reference rf;
                    try
                    {
                        rf = uiDoc.Selection.PickObject(ObjectType.Element, new GridSelectionFilter(),
                           "Select grid to dim...");
                    }
                    catch (Exception e)
                    {
                        break;
                    }

                    var point = rf.GlobalPoint;
                    var grid = rf.ToElement() as Grid;

                    var selectedGridModel = new GridModel(grid);

                    var gridsToDim = allGridModels.Where(x => x.IsHorizontalGrid == selectedGridModel.IsHorizontalGrid)
                        .ToList();

                    var ra = new ReferenceArray();
                    foreach (var gridModel in gridsToDim)
                    {
                        ra.Append(new Reference(gridModel.Grid));
                    }

                    var vector = XYZ.BasisX;

                    if (selectedGridModel.IsHorizontalGrid)
                    {
                        vector= XYZ.BasisY;
                    }


                    var orderedGrids = gridsToDim.OrderBy(x => x.EP.DotProduct(vector)).ToList();

                    var gridModel1 = orderedGrids.FirstOrDefault();
                    var gridModel2= orderedGrids.LastOrDefault();

                    var line = Line.CreateBound(rf.GlobalPoint, rf.GlobalPoint.Add(vector));

                    doc.Create.NewDimension(doc.ActiveView, line, ra,SelectedDimensionType);


                    var p = rf.GlobalPoint.Add(XYZ.BasisX * Distance.MmToFeet() * doc.ActiveView.Scale);
                    var line2 = Line.CreateBound(p, p.Add(vector));

                    var ra2 = new ReferenceArray();
                    ra2.Append(new Reference(gridModel1.Grid));
                    ra2.Append(new Reference(gridModel2.Grid));
                    doc.Create.NewDimension(doc.ActiveView, line2, ra2, SelectedDimensionType);

                }


                tx.Commit();
            }

            SaveData();
        }
        void Close()
        {

        }


        void SaveData()
        {
            var jsonData = new DimensionGridJsonModel()
            {
                SelectedDimensionTypeName = SelectedDimensionType.Name,
                Distance = Distance
            };

            var jsonString=JsonConvert.SerializeObject(jsonData);

            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                  "\\HocRevitAPI"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                          "\\HocRevitAPI");
            }

            File.WriteAllText(path,jsonString);
        }

        void LoadData()
        {
            if (File.Exists(path))
            {
                var text = File.ReadAllText(path);
                var data = JsonConvert.DeserializeObject<DimensionGridJsonModel>(text);

                if (data!=null)
                {
                    Distance = data.Distance;

                    SelectedDimensionType =
                        DimensionTypes.FirstOrDefault(x => x.Name == data.SelectedDimensionTypeName);
                }
            }
        }
    }
}
