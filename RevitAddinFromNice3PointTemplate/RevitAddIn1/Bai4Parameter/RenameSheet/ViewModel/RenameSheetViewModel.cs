using RevitAddIn1.Bai4Parameter.RenameSheet.Model;
using RevitAddIn1.Bai4Parameter.RenameSheet.View;

namespace RevitAddIn1.Bai4Parameter.RenameSheet.ViewModel
{
    public class RenameSheetViewModel : ObservableObject
    {
        public RenameSheetView RenameSheetView { get; set; }
        private List<SheetModel> _sheetModels = new List<SheetModel>();
        private string _find = "";
        private string _replace = "";

        public List<SheetModel> SheetModels
        {
            get => _sheetModels;
            set
            {
                if (Equals(value, _sheetModels)) return;
                _sheetModels = value;
                OnPropertyChanged();
            }
        }


        public string Find
        {
            get => _find;
            set
            {
                if (value == _find) return;
                _find = value;
                OnPropertyChanged();
            }
        }

        public string Replace
        {
            get => _replace;
            set
            {
                if (value == _replace) return;
                _replace = value;


                foreach (var sheetModel in SheetModels)
                {
                    sheetModel.NewSheetName = sheetModel.OldSheetName.Replace(Find, Replace);
                }

                OnPropertyChanged();
            }
        }

        public RelayCommand OkCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }

        private Document doc;
        public RenameSheetViewModel(Document doc)
        {
            this.doc = doc;
            OkCommand = new RelayCommand(Run);
            CloseCommand = new RelayCommand(Close);

            GetData();
        }

        void GetData()
        {
            SheetModels = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet)).Cast<ViewSheet>().Select(x=> new SheetModel(x)).ToList();
        }

        void Run()
        {
            RenameSheetView.Close();

            using (var tx= new Transaction(doc,"rename sheet"))
            {
                tx.Start();


                foreach (var sheetModel in SheetModels)
                {
                    if (sheetModel.NewSheetName!= sheetModel.OldSheetName)
                    {
                        sheetModel.ViewSheet.Name = sheetModel.NewSheetName;
                    }
                }

                tx.Commit();
            }
        }

        void Close()
        {

        }
    }
}
