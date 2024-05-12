using BookingApp.DTO;
using BookingApp.Domain.Model;
using BookingApp.Repository;
using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for Notifications.xaml
    /// </summary>
    public partial class Notifications : Window
    {
        public NotificationsVM NotificationsVM { get; set; }
        
        public Notifications( int loggedInUserId)
        {
            InitializeComponent();
            NotificationsVM = new NotificationsVM(loggedInUserId);
            DataContext = NotificationsVM;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }

       
    }
}

