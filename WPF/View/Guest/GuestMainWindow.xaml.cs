using BookingApp.WPF.ViewModel.Guest;
using System.Windows;

namespace BookingApp.WPF.View.Guest
{
    public partial class GuestMainWindow : Window
    {
        public GuestMainWindowVM GuestMainWindowVM { get; set; }

        public GuestMainWindow()
        {
            InitializeComponent();
            GuestMainWindowVM = new GuestMainWindowVM();
            DataContext = GuestMainWindowVM;
        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            GuestMainWindowVM.SearchClick();
        }

        private void BookAccommodationClick(object sender, RoutedEventArgs e)
        {
            GuestMainWindowVM.BookAccommodation();
        }

        private void OpenReservationsClick(object sender, RoutedEventArgs e)
        {
            GuestMainWindowVM.OpenReservations();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
