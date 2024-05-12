using BookingApp.WPF.ViewModel.Guest;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Controls;
using BookingApp.DTO;

namespace BookingApp.WPF.View.Guest
{
    public partial class MyReservationsWindow : Page
    {
        public MyReservationsVM MyReservationsVM { get; set; }

        public MyReservationsWindow(NavigationService navigationService,GuestDTO guestDTO)
        {
            InitializeComponent();
            MyReservationsVM = new MyReservationsVM(navigationService,guestDTO);
            DataContext = MyReservationsVM;
        }
    }
}
