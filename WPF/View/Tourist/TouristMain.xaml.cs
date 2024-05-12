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
    /// Interaction logic for TouristMain.xaml
    /// </summary>
    public partial class TouristMain : Window
    {
        TouristMainVM TouristMainVM { get; set; }
        public TouristMain(string username)
        {
            InitializeComponent();
            DataContext = new TouristMainVM(MainWindowFrame.NavigationService, username);
        }

    
    }
}
