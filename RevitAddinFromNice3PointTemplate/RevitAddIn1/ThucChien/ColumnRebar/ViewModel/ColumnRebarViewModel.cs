using System.IO;
using System.Printing;
using System.Windows;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Newtonsoft.Json;
using RevitAddIn1.Bai5EdittingCreating.RealProject.DimensionGrid.Model;
using RevitAddIn1.Bai6Geometry.CreatePilesFromCad.View;
using RevitAddIn1.SelectionFilter;
using RevitAddIn1.ThucChien.ColumnRebar.Model;
using RevitAddIn1.ThucChien.ColumnRebar.View;
using RevitAddIn1.Utils;

namespace RevitAddIn1.ThucChien.ColumnRebar.ViewModel
{
    public class ColumnRebarViewModel : ObservableObject
    {
        public int NumberOfXRebar
        {
            get => _numberOfXRebar;
            set
            {
                if (value == _numberOfXRebar) return;
                _numberOfXRebar = value;
                OnPropertyChanged();
            }
        }

        public int NumberOfYRebar
        {
            get => _numberOfYRebar;
            set
            {
                if (value == _numberOfYRebar) return;
                _numberOfYRebar = value;
                OnPropertyChanged();
            }
        }

        public RebarBarType XDiameter
        {
            get => _xDiameter;
            set
            {
                if (Equals(value, _xDiameter)) return;
                _xDiameter = value;
                OnPropertyChanged();
            }
        }

        public RebarBarType YDiameter
        {
            get => _yDiameter;
            set
            {
                if (Equals(value, _yDiameter)) return;
                _yDiameter = value;
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
        public ColumnRebarView ColumnRebarView { get; set; }

        private string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) +
                              "\\HocRevitAPI\\PileFromCad.json";

        private List<string> _layers = new List<string>();
        private string _selectedLayer;
        private int _numberOfXRebar = 4;
        private int _numberOfYRebar = 4;
        private RebarBarType _xDiameter;
        private RebarBarType _yDiameter;
        private List<RebarBarType> _diameters;
        private int _stirrupSpacing = 200;
        private RebarBarType _stirrupDiameter;

        private ColumnRebarModel columnModel;

        public double Cover { get; set; } = 30.MmToFeet();

        public ColumnRebarViewModel(Document doc, UIDocument uiDoc)
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

            Diameters = new FilteredElementCollector(doc).OfClass(typeof(RebarBarType)).Cast<RebarBarType>()
                .OrderBy(x => x.Name).ToList();


            XDiameter = Diameters.FirstOrDefault(x => x.BarNominalDiameter.FeetToMm() > 20);
            YDiameter = Diameters.FirstOrDefault(x => x.BarNominalDiameter.FeetToMm() > 20);
            StirrupDiameter = Diameters.FirstOrDefault(x => x.BarNominalDiameter.FeetToMm() < 12);

            LoadData();

        }

        void Run()
        {


            ColumnRebarView.Close();

            try
            {
                var column = uiDoc.Selection
                    .PickObject(ObjectType.Element, new ColumnSelectionFilter(), "Select column to create rebar")
                    .ToElement() as FamilyInstance;

                columnModel = new ColumnRebarModel(column);
            }
            catch (Exception e)
            {
                MessageBox.Show("You have aborted the pick operation");
                return;
            }


            using (var tx = new Transaction(doc, "Create piles from autocad"))
            {
                tx.Start();
                CreateXMainRebar();
                CreateYMainRebar();
                CreateStirrup();
                tx.Commit();
            }


            SaveData();
        }


        void CreateStirrup()
        {
            var shape = new FilteredElementCollector(doc).OfClass(typeof(RebarShape)).Cast<RebarShape>()
                .First(x => x.Name == "M_T1");

            var o1 = columnModel.D.Add(columnModel.XVector * Cover).Add(columnModel.YVector * Cover);

            o1 = new XYZ(o1.X, o1.Y, columnModel.BotElevation + Cover + StirrupDiameter.BarNominalDiameter / 2);

            var rebar = Rebar.CreateFromRebarShape(doc, shape, StirrupDiameter, columnModel.Column, o1, columnModel.XVector,
                 columnModel.YVector);

            var shapeDrivenAccessor = rebar.GetShapeDrivenAccessor();
            shapeDrivenAccessor.ScaleToBox(o1, columnModel.XVector * (columnModel.Width - 2 * Cover), columnModel.YVector * (columnModel.Height - 2 * Cover));

            shapeDrivenAccessor.SetLayoutAsMaximumSpacing(StirrupSpacing.MmToFeet(), (columnModel.TopElevation - columnModel.BotElevation) - 2 * Cover - StirrupDiameter.BarNominalDiameter, true, true, true);
        }

        void CreateXMainRebar()
        {

            var spacing2Rebars = (columnModel.Width - 2 * Cover - 2 * StirrupDiameter.BarNominalDiameter -
                                 XDiameter.BarNominalDiameter) / (NumberOfXRebar - 1);


            //Top Layers

            var topRebars = new List<Rebar>();
            for (int i = 0; i < NumberOfXRebar; i++)
            {

                var o2 = columnModel.A.Add(columnModel.XVector *
                                           (Cover + StirrupDiameter.BarNominalDiameter + XDiameter.BarNominalDiameter / 2))
                    .Add(-columnModel.YVector *
                         (Cover + StirrupDiameter.BarNominalDiameter + XDiameter.BarNominalDiameter / 2));

                o2 = new XYZ(o2.X, o2.Y, columnModel.BotElevation);


                o2 = o2.Add(columnModel.XVector * i * spacing2Rebars);

                var columnHeight = columnModel.TopElevation - columnModel.BotElevation;
                var line20 = Line.CreateBound(o2, o2.Add(XYZ.BasisZ * (columnHeight + 20 * XDiameter.BarNominalDiameter)));
                var line30 = Line.CreateBound(o2, o2.Add(XYZ.BasisZ * (columnHeight + 30 * XDiameter.BarNominalDiameter)));


                if (i % 2 == 0)
                {
                    var rebar = Rebar.CreateFromCurves(doc, RebarStyle.Standard, XDiameter, null, null, columnModel.Column,
                         columnModel.XVector, new List<Curve>() { line20 }, RebarHookOrientation.Left, RebarHookOrientation.Left,
                         true, true);

                    topRebars.Add(rebar);
                }
                else
                {
                    var rebar = Rebar.CreateFromCurves(doc, RebarStyle.Standard, XDiameter, null, null, columnModel.Column,
                         columnModel.XVector, new List<Curve>() { line30 }, RebarHookOrientation.Left, RebarHookOrientation.Left,
                         true, true);

                    topRebars.Add(rebar);
                }

            }


            ElementTransformUtils.CopyElements(doc, topRebars.Select(x => x.Id).ToList(), columnModel.YVector * -1 *
                (columnModel.Height - 2 * Cover - 2 * StirrupDiameter.BarNominalDiameter -
                 XDiameter.BarNominalDiameter));


   

        }

        void CreateYMainRebar()
        {

            var spacing2Rebars = (columnModel.Height - 2 * Cover - 2 * StirrupDiameter.BarNominalDiameter -
                                 XDiameter.BarNominalDiameter) / (NumberOfYRebar - 1);


            //Left Layers

            var topRebars = new List<Rebar>();
   
            if (NumberOfYRebar > 2)
            {
                for (int i = 0; i < NumberOfYRebar; i++)
                {
                    if (i == 0 || i == NumberOfYRebar - 1)
                    {
                        continue;
                    }


                    var o2 = columnModel.A.Add(columnModel.XVector *
                                               (Cover + StirrupDiameter.BarNominalDiameter + XDiameter.BarNominalDiameter / 2))
                        .Add(-columnModel.YVector *
                             (Cover + StirrupDiameter.BarNominalDiameter + XDiameter.BarNominalDiameter / 2));

                    o2 = new XYZ(o2.X, o2.Y, columnModel.BotElevation);


                    o2 = o2.Add(columnModel.YVector*-1 * i * spacing2Rebars);

                    var columnHeight = columnModel.TopElevation - columnModel.BotElevation;
                    var line20 = Line.CreateBound(o2, o2.Add(XYZ.BasisZ * (columnHeight + 20 * XDiameter.BarNominalDiameter)));
                    var line30 = Line.CreateBound(o2, o2.Add(XYZ.BasisZ * (columnHeight + 30 * XDiameter.BarNominalDiameter)));


                    if (i % 2 == 0)
                    {
                        var rebar = Rebar.CreateFromCurves(doc, RebarStyle.Standard, XDiameter, null, null, columnModel.Column,
                            columnModel.XVector, new List<Curve>() { line20 }, RebarHookOrientation.Left, RebarHookOrientation.Left,
                            true, true);

                        topRebars.Add(rebar);
                    }
                    else
                    {
                        var rebar = Rebar.CreateFromCurves(doc, RebarStyle.Standard, XDiameter, null, null, columnModel.Column,
                            columnModel.XVector, new List<Curve>() { line30 }, RebarHookOrientation.Left, RebarHookOrientation.Left,
                            true, true);

                        topRebars.Add(rebar);
                    }
                }
            }

            ElementTransformUtils.CopyElements(doc, topRebars.Select(x => x.Id).ToList(), columnModel.XVector  *
                (columnModel.Width - 2 * Cover - 2 * StirrupDiameter.BarNominalDiameter -
                 XDiameter.BarNominalDiameter));

        }
        void LoadCad()
        {

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
