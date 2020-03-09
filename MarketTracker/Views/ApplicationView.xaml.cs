namespace MarketTracker.Views
{
    using System.Windows;
    using MahApps.Metro.Controls;
    using MarketTracker.ViewModels;
    using MarketTracker.Views.ShareCast;
    using MarketTracker.WebDriver;

    /// <summary>
    /// Interaction logic for ApplicationView.xaml
    /// </summary>
    public partial class ApplicationView : MetroWindow
    {
        private ApplicationViewModel viewModel;

        public ApplicationView()
        {
            WebDriverData.PopulateProperties();
            var viewModel = new ApplicationViewModel();
            this.viewModel = viewModel;
            InitializeComponent();
            this.DataContext = viewModel;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ContentArea.Content = new HomeView();
        }

        private void BtHome_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new HomeView();
        }

        private void BtFtseIndex_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new FtseIndexView(this.viewModel);
        }

        private void BtShareCast_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new ShareCastView(this.viewModel);
        }
    }
}
