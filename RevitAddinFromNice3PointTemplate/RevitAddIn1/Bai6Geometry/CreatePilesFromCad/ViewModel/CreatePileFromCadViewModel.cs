using System.IO;
using System.Text.Json;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using RevitAddIn1.Bai5EdittingCreating.RealProject.DimensionGrid.Model;
using RevitAddIn1.Bai6Geometry.CreatePilesFromCad.Model;
using RevitAddIn1.Bai6Geometry.CreatePilesFromCad.View;
using RevitAddIn1.SelectionFilter;
using RevitAddIn1.Utils;

namespace RevitAddIn1.Bai6Geometry.CreatePilesFromCad.ViewModel
{
    public class CreatePileFromCadViewModel : ObservableObject
    {

        public List<CadCurveModel> CadCurveModels { get; set; } = new List<CadCurveModel>();

        public List<string> Layers
        {
            get => _layers;
            set
            {
                if (Equals(value, _layers)) return;
                _layers = value;
                OnPropertyChanged();
            }
        }

        public string SelectedLayer
        {
            get => _selectedLayer;
            set
            {
                if (value == _selectedLayer) return;
                _selectedLayer = value;
                OnPropertyChanged();
            }
        }

        public List<FamilySymbol> PileSymbols { get; set; } = new List<FamilySymbol>();
        public FamilySymbol SelectedPileSymbol { get; set; }
        public RelayCommand OkCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand LoadCadCommand { get; set; }

        private Document doc;
        private UIDocument uiDoc;
        public CreatePileFromCadView CreatePileFromCadView { get; set; }

        private string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                              "\\HocRevitAPI\\PileFromCad.json";

        private List<string> _layers = new List<string>();
        private string _selectedLayer;

        public CreatePileFromCadViewModel(Document doc, UIDocument uiDoc)
        {
            this.doc = doc;
            this.uiDoc = uiDoc;
            OkCommand = new RelayCommand(Run);
            CloseCommand = new RelayCommand(Close);
            LoadCadCommand = new RelayCommand(LoadCad);

            GetData();
        }

        void GetData()
        {
            PileSymbols = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol))
                .OfCategory(BuiltInCategory.OST_StructuralFoundation)
                .Cast<FamilySymbol>()
                .OrderBy(x => x.Name).ToList();

            SelectedPileSymbol = PileSymbols.FirstOrDefault();

            LoadData();

        }

        void Run()
        {
            CreatePileFromCadView.Close();

            var cadPileCurves = CadCurveModels.Where(x => x.Layer == SelectedLayer).ToList();


            using (var tx= new Transaction(doc,"Create piles from autocad"))
            {
                tx.Start();
                foreach (var cadPileCurve in cadPileCurves)
                {
                    var arc = cadPileCurve.Curve as Arc;

                    if (arc != null)
                    {
                        var center = arc.Center;
                        var radius = arc.Radius;


                        doc.Create.NewFamilyInstance(center, SelectedPileSymbol, doc.ActiveView.GenLevel,
                            StructuralType.Footing);
                    }
                }

                tx.Commit();
            }
     

            SaveData();
        }

        void LoadCad()
        {
            CreatePileFromCadView.Hide();
            var cadLink = uiDoc.Selection.PickObject(ObjectType.Element, new CadLinkSelectionFilter(), "Select Cad Link").ToElement();


            var allArcs = new List<Arc>();
            var geometryElement = cadLink.get_Geometry(new Options());
            foreach (var geoObj in geometryElement)
            {
                if (geoObj is GeometryInstance geometryInstance)
                {
                    var ge = geometryInstance.GetInstanceGeometry();
                    var arcs = ge.Where(x => x is Arc).Cast<Arc>().ToList();
                    allArcs.AddRange(arcs);
                }
            }

            CadCurveModels = allArcs.Select(x => new CadCurveModel(x)).ToList();

            Layers = CadCurveModels.Select(x => x.Layer).DistinctBy(x => x).OrderBy(x => x).ToList();

            SelectedLayer = Layers.FirstOrDefault();

            CreatePileFromCadView.ShowDialog();
        }
        void Close()
        {

        }

        void SaveData()
        {
            var jsonData = new DimensionGridJsonModel()
            {

            };

            var jsonString = JsonSerializer.Serialize(jsonData);

            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                  "\\HocRevitAPI"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                                          "\\HocRevitAPI");
            }

            File.WriteAllText(path, jsonString);
        }

        void LoadData()
        {
            if (File.Exists(path))
            {
                var text = File.ReadAllText(path);
                var data = JsonSerializer.Deserialize<DimensionGridJsonModel>(text);

                if (data != null)
                {

                }
            }
        }
    }
}
