using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.IO;
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

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for Notifications.xaml
    /// </summary>
    public partial class Notifications : Window
    {

        public readonly GuestGradeRepository guestGradeRepository;
        public readonly GuestRepository guestRepository;
        private readonly AccommodationReservationRepository accommodationReservationRepository;
        public ObservableCollection<AccommodationReservationDTO> AllAccommodationReservations { get; set; }
        
        public Notifications()
        {
            InitializeComponent();
            
            DataContext = this;
            accommodationReservationRepository = new AccommodationReservationRepository();
            guestRepository = new GuestRepository();
            guestGradeRepository = new GuestGradeRepository();
            AllAccommodationReservations = new ObservableCollection<AccommodationReservationDTO>();
            

            Update();
        }


        public void Update()
        {
            AllAccommodationReservations.Clear();
            
            foreach (AccommodationReservation accommodationReservation in accommodationReservationRepository.GetAll())
            {
                if (IsWithinFiveDays(accommodationReservation))
                {
                    if (IsGuestGraded( accommodationReservation.Id) == false)
                    {
                        var accommodationReservationDTO = new AccommodationReservationDTO(accommodationReservation);
                        var guest = guestRepository.GetById(accommodationReservation.GuestId);
                        accommodationReservationDTO.Guest = new GuestDTO(guest);

                        AllAccommodationReservations.Add(accommodationReservationDTO);
                    }
                }
            }


        }

        private bool IsWithinFiveDays(AccommodationReservation accommodationReservation)
        {
            DateTime currentDate = DateTime.Now;
            DateTime endDate = accommodationReservation.EndDate;
            //DateTime dateTime = new DateTime(endDate.Year, endDate.Month, endDate.Day);
            TimeSpan difference = currentDate - endDate;
            return difference.Days < 5 && difference.Days >= 0;
        }



        private bool IsGuestGraded(int reservationId)
        {
            // Provera da li gost ima ocenu za datu rezervaciju
            return guestGradeRepository.GetAll().Any(grade =>grade.ReservationId == reservationId);
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}

