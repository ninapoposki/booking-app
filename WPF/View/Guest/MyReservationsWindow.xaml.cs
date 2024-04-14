using BookingApp.WPF.ViewModel.Guest;
using System.Windows;

namespace BookingApp.WPF.View.Guest
{
    public partial class MyReservationsWindow : Window
    {
        public MyReservationsVM MyReservationsVM { get; set; }

        public MyReservationsWindow()
        {
            InitializeComponent();
            MyReservationsVM = new MyReservationsVM();
            DataContext = MyReservationsVM;
        }

        private void RateAccommodationClick(object sender, RoutedEventArgs e)
        {
            MyReservationsVM.RateAccommodationClick();
        }

        private void CancelReservationClick(object sender, RoutedEventArgs e)
        {
            MyReservationsVM.CancelReservationClick();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
