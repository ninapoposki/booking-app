using BookingApp.WPF.ViewModel.Guide;
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
using System.Windows.Shapes;

namespace BookingApp.WPF.View.Guide
{
    /// <summary>
    /// Interaction logic for UpcomigTours.xaml
    /// </summary>
    public partial class UpcomigTours : Window
    {
        private UpcomingToursVM UpcomingToursVM {  get; set; }
        public UpcomigTours()
        {
            InitializeComponent();
            UpcomingToursVM = new UpcomingToursVM();
            DataContext = UpcomingToursVM;
        }
        private void CancelTourClick(object sender, RoutedEventArgs e)
        {
            UpcomingToursVM.CancelTourClick();
        }
    }
}
