using BookingApp.DTO;
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
    /// Interaction logic for ChangeReservation.xaml
    /// </summary>
    public partial class ChangeReservation : Window
    {
        public ChangeReservationVM ChangeReservationVM { get; set; }

        public ChangeReservation(List<(DateTime, DateTime)> dates, AccommodationReservationDTO accommodationReservation)
        {
            InitializeComponent();
            ChangeReservationVM = new ChangeReservationVM(dates,accommodationReservation);
            DataContext = ChangeReservationVM;
        }

        private void SendRequestClick(object sender, RoutedEventArgs e)
        {
            ChangeReservationVM.SendRequestClick();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
