using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MarketTracker.Views.ShareCast
{
    /// <summary>
    /// Interaction logic for ShareCastView.xaml
    /// </summary>
    public partial class ShareCastView : UserControl
    {
        public ShareCastView()
        {
            InitializeComponent();
        }

        private void btSharePrice_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new ShareCastSharePriceView();
        }
    }
}
