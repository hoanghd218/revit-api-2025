namespace RevitAddIn1.Bai4Parameter.RenameSheet.Model
{
    public class SheetModel  : ObservableObject
    {
        private string _newSheetName;
        public string OldSheetName { get; set; }

        public string NewSheetName
        {
            get => _newSheetName;
            set => SetProperty(ref _newSheetName, value);
        }

        public ViewSheet ViewSheet { get; set; }

        public SheetModel(ViewSheet viewSheet)
        {
            OldSheetName = viewSheet.Name;
            NewSheetName = viewSheet.Name;

            ViewSheet = viewSheet;
        }
    }
}
