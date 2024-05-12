using BookingApp.WPF.ViewModel.Guest;
using System.Windows.Navigation;
using System.Windows.Controls;
using System.Windows;
using BookingApp.DTO;

namespace BookingApp.WPF.View.Guest
{
    public partial class GuestMainWindow : Page
    {
        public GuestMainWindowVM GuestMainWindowVM { get; set; }

        public GuestMainWindow(NavigationService navigationService,GuestDTO guestDTO)
        {
            InitializeComponent();
            GuestMainWindowVM = new GuestMainWindowVM(navigationService,guestDTO);
            DataContext = GuestMainWindowVM;

        }
    }
}
