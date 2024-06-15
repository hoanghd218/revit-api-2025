namespace RevitAddIn1.Bai8ExternalEvent.Model
{
    public class CreateSheetModel2
    {
        public string SheetNumber
        {
            get;
            set;
        }

        public string SheetName { get; set; }
        public string DrawnBy { get; set; }
        public string CheckedBy { get; set; }

        public bool AlreadyExist { get; set; } = true;

        public CreateSheetModel2()
        {

        }
    }
}
