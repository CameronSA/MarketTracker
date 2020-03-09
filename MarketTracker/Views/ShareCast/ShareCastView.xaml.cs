namespace MarketTracker.Views.ShareCast
{
    using System.Windows;
    using System.Windows.Controls;
    using MarketTracker.ViewModels;

    /// <summary>
    /// Interaction logic for ShareCastView.xaml
    /// </summary>
    public partial class ShareCastView : UserControl
    {
        public ShareCastView(ApplicationViewModel viewModel)
        {
            InitializeComponent();
            this.ViewModel = viewModel;
        }

        public ApplicationViewModel ViewModel { get; private set; }

        private void btSharePrice_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new ShareCastSharePriceView(this.ViewModel);
        }
    }
}
