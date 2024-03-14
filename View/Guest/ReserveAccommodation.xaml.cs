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
using BookingApp.Model;

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for ReserveAccommodation.xaml
    /// </summary>
    public partial class ReserveAccommodation : Window
    {
        public AccommodationReservationDTO accommodationReservation;
        public AccommodationReservation accommRes;
        public ObservableCollection<AccommodationReservationDTO> accommodationReservations;
        public AccommodationReservationRepository accommodationReservationRepository=new AccommodationReservationRepository();
        public Accommodation selectedAccommodation;
        public ReserveAccommodation(AccommodationDTO accommodation)
        {
            InitializeComponent();
            DataContext = this;
            accommodationReservation=new AccommodationReservationDTO();
        }
        private void TryToBookButton(object sender, RoutedEventArgs e)
        {
            if (!accommodationReservationRepository.isReservationValid(accommRes)) //kontam da mora u dto
            {
                accommodationReservationRepository.makeReservation(selectedAccommodation); 

            }
            else
            {
                MessageBox.Show("You can't book this accommodation,here is the alternative");

            }



        }


        private void CancelButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
