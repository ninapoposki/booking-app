using BookingApp.Domain.Model;
using BookingApp.WPF.View.Guide;
using BookingApp.WPF.ViewModel.Guest;
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

namespace BookingApp.WPF.View.Guest
{
    /// <summary>
    /// Interaction logic for GuestMenuBar.xaml
    /// </summary>
    public partial class GuestMenuBar : Window
    {
        public GuestMenuBarVM GuestMenuBarVM { get; set; }

        public GuestMenuBar(string username)
        {
            InitializeComponent();
            GuestMenuBarVM = new GuestMenuBarVM(MainWindowFrame.NavigationService, username);
            DataContext = GuestMenuBarVM;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }
    }
}
