namespace MarketTracker.Views
{
    using MarketTracker.ViewModels;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for FtseIndexView.xaml
    /// </summary>
    public partial class FtseIndexView 
    {
        public FtseIndexView(ApplicationViewModel applicationViewModel)
        {
            var viewModel = new FtseIndexViewModel(applicationViewModel);
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
