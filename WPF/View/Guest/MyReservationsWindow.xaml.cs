using BookingApp.Repository;
using BookingApp.DTO;

using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace BookingApp.WPF.View.Guest
{
    /// <summary>
    /// Interaction logic for MyReservationsWindow.xaml
    /// </summary>
    public partial class MyReservationsWindow : Window
    {
        public ObservableCollection<AccommodationReservationDTO> AllReservations { get; set; }
        private readonly AccommodationReservationRepository accommodationReservationRepository;
        private readonly AccommodationRepository accommodationRepository;
        private readonly LocationRepository locationRepository;  //mislim da mi ne treba ovo
        private readonly OwnerRepository ownerRepository; //mislim da mi ne treba ovo 
       
        public AccommodationReservationDTO SelectedReservation { get; set; }

        
        public MyReservationsWindow()
        {
            InitializeComponent();
            DataContext = this;

            accommodationReservationRepository=new AccommodationReservationRepository();
            accommodationRepository=new AccommodationRepository();
            locationRepository=new LocationRepository();    
            ownerRepository=new OwnerRepository();
            AllReservations = new ObservableCollection<AccommodationReservationDTO>();


            Update();
        }


        public void Update()
        {
            AllReservations.Clear();

            foreach (var reservation in accommodationReservationRepository.GetAll())
            {
                Accommodation accommodation = accommodationRepository.GetById(reservation.AccommodationId);
                Location location = locationRepository.GetById(accommodation.IdLocation);
                BookingApp.Domain.Model.Owner owner = ownerRepository.GetById(accommodation.OwnerId);

                AllReservations.Add(new AccommodationReservationDTO(reservation, accommodation, location, owner));
            }
        }


        private void RateAccommodationClick(object sender, RoutedEventArgs e)
        {
            if (SelectedReservation != null)
            {
                var dialog = new GradeAccommodation(SelectedReservation);
                dialog.ShowDialog();
            }
            else
            {
                MessageBox.Show("You didn't choose reservation!");
            }
        }
    }
}
