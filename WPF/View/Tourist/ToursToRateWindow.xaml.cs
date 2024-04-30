using BookingApp.WPF.ViewModel.Tourist;
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

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for ToursToRateWindow.xaml
    /// </summary>
    public partial class ToursToRateWindow : Window
    { 
        public ToursToRateVM ToursToRateVM { get; set; }
        public ToursToRateWindow(int userId)
        { 
            ToursToRateVM = new ToursToRateVM(userId);
            InitializeComponent();
            DataContext = ToursToRateVM;
        }

      
    }
}
