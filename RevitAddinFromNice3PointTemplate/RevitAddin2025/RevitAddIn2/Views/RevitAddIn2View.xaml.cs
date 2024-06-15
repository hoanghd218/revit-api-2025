using RevitAddIn2.ViewModels;

namespace RevitAddIn2.Views
{
    public sealed partial class RevitAddIn2View
    {
        public RevitAddIn2View(RevitAddIn2ViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
        }
    }
}