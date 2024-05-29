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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.View.Guest
{
    /// <summary>
    /// Interaction logic for ReccommendedDates.xaml
    /// </summary>
    public partial class ReccommendedDates : Page
    {
        public ReccommendedDatesVM ReccommendedDatesVM { get; set; }

        public ReccommendedDates(NavigationService navigationService, AccommodationReservationDTO reservationDTO)
        {
            InitializeComponent();
            ReccommendedDatesVM = new ReccommendedDatesVM(navigationService, reservationDTO);
            DataContext = ReccommendedDatesVM;
        }
    }
}
