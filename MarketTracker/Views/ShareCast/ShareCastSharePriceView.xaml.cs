using MarketTracker.ViewModels;
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
    /// Interaction logic for ShareCastSharePriceView.xaml
    /// </summary>
    public partial class ShareCastSharePriceView : UserControl
    {
        public ShareCastSharePriceView()
        {
            var viewModel = new ShareCastSharePriceViewModel();
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
