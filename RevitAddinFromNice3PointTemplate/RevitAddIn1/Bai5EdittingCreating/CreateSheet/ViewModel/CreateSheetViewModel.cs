using Microsoft.Win32;
using RevitAddIn1.Bai5EdittingCreating.CreateSheet.Model;
using RevitAddIn1.Bai5EdittingCreating.CreateSheet.View;
using System.IO;
using System.Windows;
using OfficeOpenXml;

namespace RevitAddIn1.Bai5EdittingCreating.CreateSheet.ViewModel
{
    public class CreateSheetViewModel : ObservableObject
    {
        public CreateSheetView CreateSheetView { get; set; }


        public List<CreateSheetModel> CreateSheetModels
        {
            get => _createSheetModels;
            set
            {
                if (Equals(value, _createSheetModels)) return;
                _createSheetModels = value;
                OnPropertyChanged();
            }
        }


        private List<ViewSheet> existingSheets = new List<ViewSheet>();
        public RelayCommand ImportCommand { get; set; }
        public RelayCommand OkCommand { get; set; }
        public RelayCommand CloseCommand { get; set; }

        private Document doc;
        private List<CreateSheetModel> _createSheetModels = new List<CreateSheetModel>();

        public CreateSheetViewModel(Document doc)
        {
            this.doc = doc;
            OkCommand = new RelayCommand(Run);
            CloseCommand = new RelayCommand(Close);
            ImportCommand = new RelayCommand(Import);
            GetData();
        }

        void GetData()
        {
            existingSheets = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet)).Cast<ViewSheet>().ToList();
        }

        void Run()
        {
            if (CreateSheetView.SheetDataGrid.SelectedItems.Count <1)
            {
                MessageBox.Show("Please Select Atleast one sheet to create!", "Warning", MessageBoxButton.OK);

                return;
            }

            CreateSheetView.Close();



            var sheetsCreated = new List<CreateSheetModel>();
            using (var tx = new TransactionGroup(doc, "rename sheet"))
            {
                tx.Start();
                foreach (var createSheetModel in CreateSheetView.SheetDataGrid.SelectedItems.Cast<CreateSheetModel>())
                {

                    using (var t = new Transaction(doc, "Create Sheet"))
                    {
                        t.Start();

                        try
                        {
                            var vs = ViewSheet.Create(doc, ElementId.InvalidElementId);
                            vs.SheetNumber = createSheetModel.SheetNumber;
                            vs.Name = createSheetModel.SheetName;
                            t.Commit();
                        }
                        catch (Exception e)
                        {
                            sheetsCreated.Add(createSheetModel);
                            t.RollBack();
                        }
                    }
                }

                tx.Commit();
            }


            if (sheetsCreated.Any())
            {
                MessageBox.Show("Can not create sheets for the following sheet numbers :" + Environment.NewLine +
                                string.Join(",", sheetsCreated.Select(x => x.SheetNumber)),"Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        void Import()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                // Set filter options and filter index
                Filter = "Excel Files|*.xlsx;*.xls",
                FilterIndex = 1,
                Multiselect = false
            };

            // Call the ShowDialog method to show the dialog box
            if (openFileDialog.ShowDialog() == true)
            {
                // Get the path of the selected file
                string filePath = openFileDialog.FileName;

                // Read the Excel file
                try
                {
                    using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
                    {
                        // Get the first worksheet in the workbook
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                        // Iterate through the rows and columns
                        for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                        {
                            var sheetNumber = worksheet.Cells[$"A{row}"].Value?.ToString();
                            var sheetName = worksheet.Cells[$"B{row}"].Value?.ToString();
                            var drawnBy = worksheet.Cells[$"C{row}"].Value?.ToString();
                            var checkedBy = worksheet.Cells[$"D{row}"].Value?.ToString();

                            if (!string.IsNullOrWhiteSpace(sheetNumber) && !string.IsNullOrWhiteSpace(sheetName))
                            {
                                var createSheetModel = new CreateSheetModel()
                                {
                                    SheetNumber = sheetNumber,
                                    SheetName = sheetName,
                                    DrawnBy = drawnBy,
                                    CheckedBy = checkedBy
                                };

                                CreateSheetModels.Add(createSheetModel);
                            }
                        }
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while reading the Excel file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }


                CreateSheetModels = CreateSheetModels.OrderBy(x => x.SheetNumber).ToList();
            }
        }

        void Close()
        {

        }
    }
}
