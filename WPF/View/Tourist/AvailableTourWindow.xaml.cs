using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for AvailableTourWindow.xaml
    /// </summary>
    public partial class AvailableTourWindow : Window
    {
        public TourDTO SelectedTour { get; set; }
        
        public ObservableCollection<TourDTO> AvailableTours { get; set; }

        public AvailableTourWindow(List<TourDTO> availableTours)
        {
            
            InitializeComponent();
            AvailableTours = new ObservableCollection<TourDTO>(availableTours);
            DataContext = this;
        }


        private void BookTour(object sender, RoutedEventArgs e)
        {

            if (SelectedTour == null)
            {


                MessageBox.Show("Molimo Vas da odaberete turu koju želite da rezervižšete.");

                return;
            }
            else
            {
                TourReservationWindow tourReservationWindow = new TourReservationWindow(SelectedTour);
                tourReservationWindow.Show();

            }


        }

        private void CancelTour(object sender, RoutedEventArgs e)
        {

            this.Close();
        }
    }
}
