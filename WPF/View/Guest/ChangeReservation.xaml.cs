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
using System.Windows.Navigation;
namespace BookingApp.WPF.View.Guest
{
    /// <summary>
    /// Interaction logic for ChangeReservation.xaml
    /// </summary>
    public partial class ChangeReservation : Page
    {
        public ChangeReservationVM ChangeReservationVM { get; set; }

        public ChangeReservation(NavigationService navigationService, List<(DateTime, DateTime)> dates, AccommodationReservationDTO accommodationReservation)
        {
            InitializeComponent();
            ChangeReservationVM = new ChangeReservationVM(navigationService,dates,accommodationReservation);
            DataContext = ChangeReservationVM;
        }

    }
}
