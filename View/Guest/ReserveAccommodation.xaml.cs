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


        private void TryToBookButton(object sender, RoutedEventArgs e)
        {
            accommodationReservation=accommodationReservationDTO.ToAccommodationReservation();
            selectedAccommodation=selectedAccommodationDTO.ToAccommodation();
            if (accommodationReservationRepository.isValid(accommodationReservation, selectedAccommodation))
            {
                if (accommodationReservationRepository.AreDatesAvailable(accommodationReservation.InitialDate, accommodationReservation.EndDate, selectedAccommodation.Id))
                {
                    guestDTO.UserId = userRepository.GetCurrentGuestUserId();
                    guestRepository.Add(guestDTO.ToGuest());
                    accommodationReservationDTO.GuestId = guestRepository.GetCurrentId();
                    accommodationReservationRepository.Add(accommodationReservationDTO.ToAccommodationReservation());
                    MessageBox.Show("Reservation added successfully");
                    this.Close();


                }
                else
                {
                    MessageBox.Show("The requested dates are not available. Here are some alternative options.");
                    //DateTime startDate, DateTime endDate, int accommodationId, int requiredStayDays
                    List<(DateTime, DateTime)> dates = accommodationReservationRepository.FindAvailableDates(accommodationReservation.InitialDate, accommodationReservation.EndDate, selectedAccommodation.Id, accommodationReservation.DaysToStay);

                    var dialog = new AvailableDatesWindow(dates);
                    dialog.ShowDialog();
                }

            }
            else {
                MessageBox.Show("The data you entered is not valid");
                this.Close();
            }
            
            

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
