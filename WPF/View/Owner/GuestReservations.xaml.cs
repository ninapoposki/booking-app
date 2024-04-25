using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookingApp.Observer;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BookingApp.Repository;
using BookingApp.DTO;
using System.Collections.ObjectModel;
using BookingApp.Domain.Model;
using BookingApp.WPF.View.Owner;
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for GuestReservations.xaml
    /// </summary>
    public partial class GuestReservations : Window
    {
        public GuestReservationsVM GuestReservationsVM { get; set; }
        public GuestReservations(int loggedInUserId)
        {
            InitializeComponent();
            GuestReservationsVM = new GuestReservationsVM(loggedInUserId);
            DataContext = GuestReservationsVM;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public void GuestDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
                GuestReservationsVM.GuestDataGridSelectionChanged();
        }
        private void GradeGuestClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            AccommodationReservationDTO reservation = (AccommodationReservationDTO)clickedButton.DataContext;

            GuestReservationsVM.GradeGuestClick(reservation);
        }
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}