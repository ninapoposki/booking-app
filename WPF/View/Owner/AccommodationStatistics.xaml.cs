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
    /// Interaction logic for AccommodationStatistics.xaml
    /// </summary>
    public partial class AccommodationStatistics : Window
    {
        public AccommodationStatisticsVM AccommodationStatisticsVM {  get; set; }
        public AccommodationStatistics(AccommodationDTO accommodationDTO)
        {
            InitializeComponent();
            AccommodationStatisticsVM = new AccommodationStatisticsVM(accommodationDTO);
            DataContext = AccommodationStatisticsVM;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
    }
}
