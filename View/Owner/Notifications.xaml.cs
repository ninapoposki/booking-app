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
            AllAccommodationReservations = new ObservableCollection<AccommodationReservationDTO>();
            

            Update();
        }


        public void Update()
        {
            AllAccommodationReservations.Clear();
            
            foreach (AccommodationReservation accommodationReservation in accommodationReservationRepository.GetAll())
            {
                if (IsWithinFiveDays(accommodationReservation) && !IsGuestGraded(accommodationReservation.GuestId, accommodationReservation.Id))
                { 
                    var accommodationReservationDTO = new AccommodationReservationDTO(accommodationReservation);
                    var guest = guestRepository.GetById(accommodationReservation.GuestId);
                    accommodationReservationDTO.Guest = new GuestDTO(guest);

                AllAccommodationReservations.Add(accommodationReservationDTO);
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

        private bool IsGuestGraded(int guestId, int reservationId)
        {
            // Putanja do datoteke guestGrade.csv
            string filePath = "../../Resources/Data/guestGrade.csv";

            // Provera da li datoteka postoji
            if (File.Exists(filePath))
            {
                // Čitanje svih linija iz datoteke
                string[] lines = File.ReadAllLines(filePath);

                // Iteracija kroz svaku liniju
                foreach (string line in lines)
                {
                    // Razdvajanje linije na delove koristeći separator (npr. zarez)
                    string[] parts = line.Split(',');

                    // Provera da li linija ima dovoljno delova i da li se podaci podudaraju
                    if (parts.Length >= 2 &&
                        int.TryParse(parts[1], out int currentGuestId) &&
                        int.TryParse(parts[2], out int currentReservationId))
                    {
                        // Ako se podaci podudaraju, vratiti true
                        if (currentGuestId == guestId && currentReservationId == reservationId)
                        {
                            return true;
                        }
                    }
                }
            }

            // Ako nismo pronašli odgovarajući zapis, vratiti false
            return false;
        }




        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}

