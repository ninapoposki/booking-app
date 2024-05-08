using BookingApp.WPF.ViewModel.Guest;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Controls;


namespace BookingApp.WPF.View.Guest
{
    public partial class MyReservationsWindow : Page
    {
        public MyReservationsVM MyReservationsVM { get; set; }

        public MyReservationsWindow(NavigationService navigationService)
        {
            InitializeComponent();
            MyReservationsVM = new MyReservationsVM(navigationService);
            DataContext = MyReservationsVM;
        }
    }
}
