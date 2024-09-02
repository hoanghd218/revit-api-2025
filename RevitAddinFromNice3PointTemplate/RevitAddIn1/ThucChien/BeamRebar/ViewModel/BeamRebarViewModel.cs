using System.IO;
using System.Windows;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Newtonsoft.Json;
using RevitAddIn1.Bai5EdittingCreating.RealProject.DimensionGrid.Model;
using RevitAddIn1.SelectionFilter;
using RevitAddIn1.ThucChien.BeamRebar.Model;
using RevitAddIn1.ThucChien.BeamRebar.View;
using RevitAddIn1.ThucChien.ColumnRebar.Model;
using RevitAddIn1.ThucChien.ColumnRebar.View;
using RevitAddIn1.Utils;

namespace RevitAddIn1.ThucChien.BeamRebar.ViewModel
{
    public class BeamRebarViewModel : ObservableObject
    {
        public int SoLuongThepChinh
        {
            get => _soLuongThepChinh;
            set
            {
                if (value == _soLuongThepChinh) return;
                _soLuongThepChinh = value;
                OnPropertyChanged();
            }
        }


        public RebarBarType DuongKinhThepChinh
        {
            get => _duongKinhThepChinh;
            set
            {
                if (Equals(value, _duongKinhThepChinh)) return;
                _duongKinhThepChinh = value;
                OnPropertyChanged();
            }
        }


        public int SoLuongThepGiaCuongLopTren
        {
            get => _soLuongThepGiaCuongLopTren;
            set
            {
                if (value == _soLuongThepGiaCuongLopTren) return;
                _soLuongThepGiaCuongLopTren = value;
                OnPropertyChanged();
            }
        }


        public RebarBarType DuongKinhThepGiaCuongLopTren
        {
            get => _duongKinhThepGiaCuongLopTren;
            set
            {
                if (Equals(value, _duongKinhThepGiaCuongLopTren)) return;
                _duongKinhThepGiaCuongLopTren = value;
                OnPropertyChanged();
            }
        }


        public int SoLuongThepGiaCuongLopDuoi
        {
            get => _soLuongThepGiaCuongLopDuoi;
            set
            {
                if (value == _soLuongThepGiaCuongLopDuoi) return;
                _soLuongThepGiaCuongLopDuoi = value;
                OnPropertyChanged();
            }
        }


        public RebarBarType DuongKinhThepGiaCuongLopDuoi
        {
            get => _duongKinhThepGiaCuongLopDuoi;
            set
            {
                if (Equals(value, _duongKinhThepGiaCuongLopDuoi)) return;
                _duongKinhThepGiaCuongLopDuoi = value;
                OnPropertyChanged();
            }
        }


        public List<RebarBarType> Diameters
        {
            get => _diameters;
            set
            {
                if (Equals(value, _diameters)) return;
                _diameters = value;
                OnPropertyChanged();
            }
        }


        public int StirrupSpacing
        {
            get => _stirrupSpacing;
            set
            {
                if (value == _stirrupSpacing) return;
                _stirrupSpacing = value;
                OnPropertyChanged();
            }
        }

        public RebarBarType StirrupDiameter
        {
            get => _stirrupDiameter;
            set
            {
                if (Equals(value, _stirrupDiameter)) return;
                _stirrupDiameter = value;
                OnPropertyChanged();
            }
        }

        public int D { get; set; } = 20;

        public RelayCommand OkCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }
        public RelayCommand LoadCadCommand { get; set; }

        private Document doc;
        private UIDocument uiDoc;
        public BeamRebarView BeamRebarView { get; set; }

        private string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                              "\\HocRevitAPI\\BeamRebar.json";

        private List<string> _layers = new List<string>();
        private string _selectedLayer;

        private List<RebarBarType> _diameters;
        private int _stirrupSpacing = 200;
        private RebarBarType _stirrupDiameter;

        private BeamRebarModel beamRebarModel;
        private int _soLuongThepChinh=4;
        private RebarBarType _duongKinhThepChinh;
        private int _soLuongThepGiaCuongLopTren = 2;
        private RebarBarType _duongKinhThepGiaCuongLopTren;
        private int _soLuongThepGiaCuongLopDuoi = 2;
        private RebarBarType _duongKinhThepGiaCuongLopDuoi;

        public double Cover { get; set; } = 30.MmToFeet();

        public BeamRebarViewModel(Document doc, UIDocument uiDoc)
        {
            this.doc = doc;
            this.uiDoc = uiDoc;
            OkCommand = new RelayCommand(Run);
            CloseCommand = new RelayCommand(Close);

            GetData();
        }

        void GetData()
        {

            Diameters = new FilteredElementCollector(doc).OfClass(typeof(RebarBarType)).Cast<RebarBarType>()
                .OrderBy(x => x.Name).ToList();



            DuongKinhThepChinh = Diameters.FirstOrDefault(x => x.BarDiameter().FeetToMm() > 20);
            DuongKinhThepGiaCuongLopTren = Diameters.FirstOrDefault(x => x.BarDiameter().FeetToMm() > 15);
            DuongKinhThepGiaCuongLopDuoi = Diameters.FirstOrDefault(x => x.BarDiameter().FeetToMm() > 15);
            StirrupDiameter = Diameters.FirstOrDefault(x => x.BarDiameter().FeetToMm() < 12);

            LoadData();

        }

        void Run()
        {


            BeamRebarView.Close();

            try
            {
                var beam = uiDoc.Selection
                    .PickObject(ObjectType.Element, new BeamSelectionFilter(), "Select beam to create rebar")
                    .ToElement() as FamilyInstance;


                var columns = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                    .OfCategory(BuiltInCategory.OST_StructuralColumns).Cast<FamilyInstance>().ToList();

                beamRebarModel = new BeamRebarModel(beam, columns);
            }
            catch (Exception e)
            {
                MessageBox.Show("You have aborted the pick operation");
                return;
            }


            using (var tx = new Transaction(doc, "Create piles from autocad"))
            {
                tx.Start();
                CreateStirrup();
                CreateMainTopRebar();
                CreateMainBotRebar();

                tx.Commit();
            }


            SaveData();
        }


        void CreateMainTopRebar()
        {
            //Top Bar

            var p1 = beamRebarModel.StartPointTop
                .Add(XYZ.BasisZ * -(Cover + StirrupDiameter.BarNominalDiameter +
                                    DuongKinhThepChinh.BarNominalDiameter / 2))
                .Add(beamRebarModel.XVector * (Cover + StirrupDiameter.BarNominalDiameter +
                                                DuongKinhThepChinh.BarNominalDiameter / 2));

            var p0 = p1.Add(-beamRebarModel.Direction * 30 * DuongKinhThepChinh.BarNominalDiameter);


            var p2 = p1.Add(beamRebarModel.Direction *
                            (beamRebarModel.Length + 30 * DuongKinhThepChinh.BarNominalDiameter));



            var curve = Line.CreateBound(p0, p2);

            var rebar = Rebar.CreateFromCurves(doc, RebarStyle.Standard, DuongKinhThepChinh, null, null, beamRebarModel.Beam,
                beamRebarModel.XVector, new List<Curve>() { curve }, RebarHookOrientation.Left, RebarHookOrientation.Left,
                true, true);

            var spacing = (beamRebarModel.B - (2 * Cover + 2 * StirrupDiameter.BarNominalDiameter +
                                              DuongKinhThepChinh.BarNominalDiameter))/(SoLuongThepChinh-1);


            for (int i = 1; i < SoLuongThepChinh; i++)
            {
                ElementTransformUtils.CopyElement(doc, rebar.Id, beamRebarModel.XVector * spacing*i);
            }
            

        
        }


        void CreateMainBotRebar()
        {
            var p1 = beamRebarModel.StartPointTop
                .Add(-XYZ.BasisZ * (beamRebarModel.H-(Cover + StirrupDiameter.BarNominalDiameter +
                                    DuongKinhThepChinh.BarNominalDiameter / 2)))

                .Add(beamRebarModel.XVector * (Cover + StirrupDiameter.BarNominalDiameter +
                                               DuongKinhThepChinh.BarNominalDiameter / 2));

            var p0 = p1.Add(-beamRebarModel.Direction * 30 * DuongKinhThepChinh.BarNominalDiameter);
            var p2 = p1.Add(beamRebarModel.Direction *
                            (beamRebarModel.Length + 30 * DuongKinhThepChinh.BarNominalDiameter));

            var curve = Line.CreateBound(p0, p2);

            var rebar = Rebar.CreateFromCurves(doc, RebarStyle.Standard, DuongKinhThepChinh, null, null, beamRebarModel.Beam,
                beamRebarModel.XVector, new List<Curve>() { curve }, RebarHookOrientation.Left, RebarHookOrientation.Left,
                true, true);

            var spacing = (beamRebarModel.B - (2 * Cover + 2 * StirrupDiameter.BarNominalDiameter +
                                               DuongKinhThepChinh.BarNominalDiameter)) / (SoLuongThepChinh - 1);


            for (int i = 1; i < SoLuongThepChinh; i++)
            {
                ElementTransformUtils.CopyElement(doc, rebar.Id, beamRebarModel.XVector * spacing * i);
            }



        }

        void CreateStirrup()
        {
            var shape = new FilteredElementCollector(doc).OfClass(typeof(RebarShape)).Cast<RebarShape>()
                .First(x => x.Name == "M_T1");


            var o1 = beamRebarModel.StartPointTop
                .Add(beamRebarModel.Direction * 50.MmToFeet())
                .Add(XYZ.BasisZ*- (beamRebarModel.H-Cover))
                .Add(beamRebarModel.XVector*Cover)
                ;


            var rebar = Rebar.CreateFromRebarShape(doc, shape, StirrupDiameter,beamRebarModel.Beam, o1, beamRebarModel.XVector,
                beamRebarModel.ZVector);



            var shapeDrivenAccessor = rebar.GetShapeDrivenAccessor();
            shapeDrivenAccessor.ScaleToBox(o1, beamRebarModel.XVector * (beamRebarModel.B - 2 * Cover), beamRebarModel.ZVector * (beamRebarModel.H - 2 * Cover));


            var normalSide= rebar.GetShapeDrivenAccessor().Normal.DotProduct(beamRebarModel.Direction) > 0;
            shapeDrivenAccessor.SetLayoutAsMaximumSpacing(StirrupSpacing.MmToFeet(), (beamRebarModel.StartPointTop - beamRebarModel.EndPointTop).GetLength() - 100.MmToFeet(), normalSide, true, true);

     

            doc.Regenerate();
        }

 
        void Close()
        {

        }

        void SaveData()
        {
            var jsonData = new DimensionGridJsonModel()
            {

            };

            var jsonString = JsonConvert.SerializeObject(jsonData);

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
                var data = JsonConvert.DeserializeObject<DimensionGridJsonModel>(text);

                if (data != null)
                {

                }
            }
        }
    }
}
