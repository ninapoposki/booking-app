using BookingApp.DTO;
using BookingApp.Domain.Model;
using BookingApp.Observer;
using BookingApp.Repository;
using System;
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
using BookingApp.Services;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Owner
{
    /// summary>
    /// Interaction logic for OwnerMainWindow.xaml
    /// </summary>
    public partial class OwnerMainWindow : Window
    {
        
        public OwnerMainWindowVM OwnerMainWindowVM { get; set; }
        
        public OwnerMainWindow(string username)
        {
            InitializeComponent();
            OwnerMainWindowVM = new OwnerMainWindowVM(MainWindowFrame.NavigationService, username);
            DataContext = OwnerMainWindowVM;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            

        }
       /* private void StudentViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            OwnersAccommodationVM ownersAccommodationVM = new OwnersAccommodationVM();



            OwnersAccommodationVM.DataContext = ownersAccommodationVM;
        }*/
    }
        /*
        private void AddAccommodationClick(object sender, RoutedEventArgs e)
        {
            OwnerMainWindowVM.AddAccommodationClick();
        }
      
        private void GradeGuestClick(object sender, RoutedEventArgs e)
        {
            OwnerMainWindowVM.GradeGuestClick();
        }
        private void NotificationsClick(object sender, RoutedEventArgs e)
        {
           OwnerMainWindowVM.NotificationsClick();
        }
        private void AccommodationsClick(object sender, RoutedEventArgs e)
        {
            OwnerMainWindowVM.AccommodationsClick();
        }

        private void ReservationsClick(object sender, RoutedEventArgs e)
        {
            OwnerMainWindowVM.ReservationsClick();
        }
        private void MyGradesClick(object sender, RoutedEventArgs e)
        {
            OwnerMainWindowVM.MyGradesClick();
        }
        private void RequestsClick(object sender, RoutedEventArgs e)
        {
            OwnerMainWindowVM.RequestsClick();
        }*/
    
}
