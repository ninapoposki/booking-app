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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for ReserveAccommodation.xaml
    /// </summary>
    public partial class ReserveAccommodation : Window, INotifyPropertyChanged
    {
       // public AccommodationReservationDTO accommodationReservation;
        public AccommodationReservation accommodationReservation ;
        public AccommodationReservationDTO accommodationReservationDTO { get; set; }
        public AccommodationReservationRepository accommodationReservationRepository=new AccommodationReservationRepository(); 
        //public AccommodationReservationRepository accommodationReservationRepository {get; set;}
        public AccommodationDTO selectedAccommodationDTO; //ili ovo da ide preko DTO AccommodationDTO
        public Accommodation selectedAccommodation;
 
       // public ObservableCollection<AccommodationReservationDTO> AllAccommodationReservations;
   
        
       // public ReserveAccommodation(AccommodationDTO accommodation)
         public ReserveAccommodation(AccommodationDTO accommodationDTO)
         {
            InitializeComponent();
            DataContext = this;
            accommodationReservationDTO=new AccommodationReservationDTO();
            selectedAccommodationDTO=new AccommodationDTO();
            selectedAccommodationDTO = accommodationDTO;
          //  this.accommodationReservationRepository=accommodationReservationRepository;

            accommodationReservationDTO.AccommodationId = accommodationDTO.Id; //ili selected?


         }

       


        private void TryToBookButton(object sender, RoutedEventArgs e)
        {

            accommodationReservationDTO.GuestId = 1;
            accommodationReservationRepository.Add(accommodationReservationDTO.ToAccommodationReservation()); //gore pozvala ovde samo sredila
               
           
            this.Close();
          

        }


        private void CancelButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
