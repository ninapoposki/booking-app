using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BookingApp.Observer;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BookingApp.Repository;
using BookingApp.DTO;
using System.Collections.ObjectModel;
using BookingApp.Model;
using BookingApp.View.Owner;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for GuestReservations.xaml
    /// </summary>
    public partial class GuestReservations : Window, IObserver
    {
        public readonly GuestGradeRepository guestGradeRepository;
        public readonly GuestRepository guestRepository;
        private readonly AccommodationReservationRepository accommodationReservationRepository;
        public ObservableCollection<AccommodationReservationDTO> AllAccommodationReservations { get; set; }
        public AccommodationReservationDTO SelectedAccommodationReservation { get; set; }
        public GuestReservations()
        {
            InitializeComponent();
            DataContext = this;
            accommodationReservationRepository = new AccommodationReservationRepository();
            guestRepository = new GuestRepository();
            AllAccommodationReservations = new ObservableCollection<AccommodationReservationDTO>();
            SelectedAccommodationReservation = new AccommodationReservationDTO();

            Update();
        }

        public void Update()
        {
            AllAccommodationReservations.Clear();
          /*  foreach (AccommodationReservation accommodationReservation in accommodationReservationRepository.GetAll())
            {  
                AllAccommodationReservations.Add(new AccommodationReservationDTO(accommodationReservation));


                var guest = guestRepository.GetById(accommodationReservation.GuestId);
                SelectedAccommodationReservation.Guest = new GuestDTO(guest);

                AllAccommodationReservations.Add(SelectedAccommodationReservation);
            }*/


            foreach (AccommodationReservation accommodationReservation in accommodationReservationRepository.GetAll())
            {
                var accommodationReservationDTO = new AccommodationReservationDTO(accommodationReservation);

                var guest = guestRepository.GetById(accommodationReservation.GuestId);
                accommodationReservationDTO.Guest = new GuestDTO(guest);

                AllAccommodationReservations.Add(accommodationReservationDTO);
            }
        }

        

        private void GuestDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Provjeravamo da li je neki red selektovan
            if (GuestDataGrid.SelectedItem != null)
            {
                
                AccommodationReservationDTO selectedAccommodationReservation = (AccommodationReservationDTO)GuestDataGrid.SelectedItem;

               
                bool TimeSpan = accommodationReservationRepository.IsOverFiveDays(selectedAccommodationReservation.ToAccommodationReservation());

                // Provjeravamo rezultat i vršimo odgovarajuću akciju
                if (TimeSpan)
                {
                    GradeGuestWindow gradeGuestWindow = new GradeGuestWindow(guestGradeRepository, selectedAccommodationReservation);
                gradeGuestWindow.ShowDialog();
                }
                else
                {
                    
                    MessageBox.Show("Grading is not possible, it has been more than 5 days.");
                }
            }
        }

        
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       /* private void CheckGradingPossibility()
        {
            // Provjeravamo da li je neki red selektovan
            if (GuestDataGrid.SelectedItem != null)
            {
                // Dobijamo selektovanu rezervaciju
                AccommodationReservationDTO selectedAccommodationReservation = (AccommodationReservationDTO)GuestDataGrid.SelectedItem;

                // Pozivamo metodu za provjeru je li prošlo više od pet dana
                bool isOverFiveDays = accommodationReservationRepository.IsOverFiveDays(selectedAccommodationReservation.ToAccommodationReservation());

                // Provjeravamo rezultat i vršimo odgovarajuću akciju
                if (isOverFiveDays)
                {
                    // Otvaranje prozora GuestViewWindow
                    GuestViewWindow guestViewWindow = new GuestViewWindow();
                    guestViewWindow.ShowDialog();
                }
                else
                {
                    // Ispisivanje poruke
                    MessageBox.Show("Grading is not possible, it has been more than 5 days.");
                }
            }
            else
            {
                // Ako nije selektovana nijedna rezervacija
                MessageBox.Show("Please select a reservation.");
            }
        }*/


    }
}