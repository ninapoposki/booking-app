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
        public AccommodationReservation accommodationReservation ;
        public AccommodationReservationDTO accommodationReservationDTO { get; set; }
        public AccommodationReservationRepository accommodationReservationRepository=new AccommodationReservationRepository(); 
        public AccommodationDTO selectedAccommodationDTO; 
        public Accommodation selectedAccommodation;
        public UserRepository userRepository = new UserRepository();
        public GuestDTO guestDTO { get; set; }
        public GuestRepository guestRepository=new GuestRepository();
 
       
        public ReserveAccommodation(AccommodationDTO accommodationDTO)
        {
            InitializeComponent();
            DataContext = this;
            accommodationReservationDTO=new AccommodationReservationDTO();
            selectedAccommodationDTO=new AccommodationDTO();
            guestDTO=new GuestDTO();
            selectedAccommodationDTO = accommodationDTO;
            guestRepository = new GuestRepository(); 
            accommodationReservationDTO.AccommodationId = accommodationDTO.Id; 

        }

        private void TryToBookClick(object sender, RoutedEventArgs e)
        {
            accommodationReservation = accommodationReservationDTO.ToAccommodationReservation();
            selectedAccommodation = selectedAccommodationDTO.ToAccommodation();
            if (!IsGuestDataValid())
            {
                return; 
            }

            if (accommodationReservationRepository.IsValid(accommodationReservation, selectedAccommodation))
            {   
                CheckReservationAvailability();
            }
            else
            {
                HandleInvalidData();
            }            
        }
          private void CheckReservationAvailability()
          {
              if (accommodationReservationRepository.AreDatesAvailable(selectedAccommodation.Id, accommodationReservation.InitialDate, accommodationReservation.EndDate))
              {
                  ProcessValidReservation();
              }
              else
              {
                  HandleUnavailableDates();
              }
     }


        private bool IsGuestDataValid()
        {
            if (string.IsNullOrWhiteSpace(guestDTO.FirstName) || string.IsNullOrWhiteSpace(guestDTO.LastName))
            {
                MessageBox.Show("Please enter guest information (first name and last name) before booking.");
                return false;
            }
            return true;
        }


        private void ProcessValidReservation()
        {
            //List<(DateTime, DateTime)> dates = accommodationReservationRepository.FindDateRange(accommodationReservation, selectedAccommodation.Id);
            //        public List<(DateTime, DateTime)> FindDateRange(AccommodationReservation reservation, int accommodationId, int numDays)

            List<(DateTime, DateTime)> dates = accommodationReservationRepository.FindDateRange( accommodationReservation,selectedAccommodation.Id);
            guestDTO.UserId = userRepository.GetCurrentGuestUserId();
            guestRepository.Add(guestDTO.ToGuest());
            accommodationReservationDTO.GuestId = guestRepository.GetCurrentId();
            // accommodationReservationRepository.Add(accommodationReservationDTO.ToAccommodationReservation());
            var dialog = new AvailableDatesWindow(dates, accommodationReservationDTO);
            dialog.Closed += (sender, args) =>
            {
                this.Close();
            };
            dialog.ShowDialog();
           // MessageBox.Show("Reservation added successfully");
            //this.Close();
        }

        private void HandleUnavailableDates()
        {
            MessageBox.Show("The requested dates are not available. Here are some alternative options.");
            List<(DateTime, DateTime)> dates = accommodationReservationRepository.FindAlternativeDates(accommodationReservation, selectedAccommodation.Id);
            guestDTO.UserId = userRepository.GetCurrentGuestUserId();
            guestRepository.Add(guestDTO.ToGuest());
            accommodationReservationDTO.GuestId = guestRepository.GetCurrentId();
            var dialog = new AvailableDatesWindow(dates, accommodationReservationDTO);
            dialog.Closed += (sender, args) =>
            {
                this.Close();
            };
            dialog.ShowDialog();
        }

        private void HandleInvalidData()
        {
            MessageBox.Show("The data you entered is not valid");
            this.Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
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
