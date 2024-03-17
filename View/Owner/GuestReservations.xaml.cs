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
        public readonly AccommodationRepository accommodationRepository;
        private readonly AccommodationReservationRepository accommodationReservationRepository;
        public ObservableCollection<AccommodationReservationDTO> AllAccommodationReservations { get; set; }
        public AccommodationReservationDTO SelectedAccommodationReservation { get; set; }
        public GuestReservations()
        {
            InitializeComponent();
            DataContext = this;
            accommodationReservationRepository = new AccommodationReservationRepository();
            guestRepository = new GuestRepository();
            guestGradeRepository = new GuestGradeRepository();
            accommodationRepository = new AccommodationRepository();
            AllAccommodationReservations = new ObservableCollection<AccommodationReservationDTO>();
            SelectedAccommodationReservation = new AccommodationReservationDTO();

            Update();
        }

        public void Update()
        {
            AllAccommodationReservations.Clear();
          
            foreach (AccommodationReservation accommodationReservation in accommodationReservationRepository.GetAll())
            {
                var accommodationReservationDTO = new AccommodationReservationDTO(accommodationReservation);

                var guest = guestRepository.GetById(accommodationReservation.GuestId);
                accommodationReservationDTO.Guest = new GuestDTO(guest);

                var accomm = accommodationRepository.GetById(accommodationReservation.AccommodationId);
                accommodationReservationDTO.Accommodation = new AccommodationDTO(accomm);

                AllAccommodationReservations.Add(accommodationReservationDTO);
            }
        }

        

        private void GuestDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            if (GuestDataGrid.SelectedItem != null)
            {
                
                AccommodationReservationDTO selectedAccommodationReservation = (AccommodationReservationDTO)GuestDataGrid.SelectedItem;

                bool isGuestAlreadyGraded = IsGuestGraded(selectedAccommodationReservation.Id);

                if (isGuestAlreadyGraded)
                {
                    MessageBox.Show("Guest is already graded.");
                }
                else
                {

                    bool TimeSpan = accommodationReservationRepository.IsOverFiveDays(selectedAccommodationReservation.ToAccommodationReservation());

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
        }

        private bool IsGuestGraded(int reservationId)
        {
           
            return guestGradeRepository.GetAll().Any(grade => grade.ReservationId == reservationId);
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

       

    }
}