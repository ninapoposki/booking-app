using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Observer;
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

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for OwnerGrades.xaml
    /// </summary>
    public partial class OwnerGrades : Window, IObserver
    {
        public readonly GuestGradeRepository guestGradeRepository;
        public readonly GuestRepository guestRepository;
        public readonly AccommodationRepository accommodationRepository;
        private readonly AccommodationReservationRepository accommodationReservationRepository;
        public ObservableCollection<AccommodationReservationDTO> AllAccommodationReservations { get; set; }
        public AccommodationReservationDTO SelectedAccommodationReservation { get; set; }
        public OwnerGrades()
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

        private void AddAccommodationClick(object sender, RoutedEventArgs e)
        {

            GradeDetails details = new GradeDetails();
            details.ShowDialog();

        }
    }
}
