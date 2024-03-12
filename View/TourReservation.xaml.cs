using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Observer;
using BookingApp.Repository;
using System;
using System.Collections;
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

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for TourReservation.xaml
    /// </summary>
    public partial class TourReservation : Window, IObserver
    {
        private TourDTO selectedTour;
        private readonly TourRepository tourRepository;
       
        public TourReservation(TourDTO selectedTour)
        {
            InitializeComponent();
            DataContext = this;

            tourRepository=new TourRepository();
            this.selectedTour = selectedTour;
          
           
        }

        public void Update()
        {
        }

        private void CancelTour(object sender, RoutedEventArgs e)
        {


            this.Close();
        }

        private void ConfirmTourReservation(object sender, RoutedEventArgs e)

        {
            if (!int.TryParse(txtNumberOfPeople.Text, out int numberOfPeople))
            {
                MessageBox.Show("Unesite validan broj ljudi.");
                return;
            }

            Tour tour = tourRepository.GetTourById(selectedTour.Id);
            if (tour == null)
            {

                MessageBox.Show("Greska,nije pronadjena ta tura");
                return;
            }

            if (numberOfPeople > tour.Capacity)
            {


                MessageBox.Show("Nema dovoljno mjesta na turi, mozete da odaberete neku drugu");
            }
            else
            {


                MessageBox.Show("Tura je rezervisana,ima dovoljno mjesta");
                tour.Capacity -= numberOfPeople;
                tourRepository.Update(tour);
                
            }

        }
    }
}
