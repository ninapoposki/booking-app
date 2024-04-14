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
    /// Interaction logic for GuestNotifications.xaml
    /// </summary>
    public partial class GuestNotifications : Window
    {
        public GuestNotificationsVM GuestNotificationsVM { get; set; }

        public GuestNotifications()
        {
            InitializeComponent();
            GuestNotificationsVM = new GuestNotificationsVM();
            DataContext = GuestNotificationsVM;
        }
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
