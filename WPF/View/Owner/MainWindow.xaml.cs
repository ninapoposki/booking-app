using BookingApp.WPF.ViewModel.Owner;
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

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Page
    {
        public MainWindowVM MainWindowVM { get; set; }
        public MainWindow(NavigationService navigation, int loggedInUserId)
        {
            InitializeComponent();
            MainWindowVM = new MainWindowVM(navigation, loggedInUserId);
            DataContext = MainWindowVM;
        }
    }
}