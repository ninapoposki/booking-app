using BookingApp.DTO;
using BookingApp.WPF.ViewModel.Owner;
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

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for AvailableDates.xaml
    /// </summary>
    public partial class AvailableDates : Window
    {
        public List<(DateTime, DateTime)> dates1;
        public AvailableDatesVM AvailableDatesVM { get; set; }
        public AvailableDates(List<(DateTime, DateTime)> dates, AccommodationReservationDTO accommodationReservationDTO)
        {
            InitializeComponent();
            AvailableDatesVM = new AvailableDatesVM(dates, accommodationReservationDTO);
            List<(DateTime, DateTime)> dates1 = dates;
            DataContext = AvailableDatesVM;
        }
    }
}
