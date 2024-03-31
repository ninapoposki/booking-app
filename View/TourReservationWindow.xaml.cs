using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Observer;
using BookingApp.Repository;
using System;
using System.Collections;
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

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for TourReservation.xaml
    /// </summary>
    public partial class TourReservationWindow : Window, IObserver
    {
        public TourReservationDTO TourReservationDTO { get; set; }
        private TourDTO selectedTour;
        private List<Tuple<string, int>> temporaryGuests = new List<Tuple<string, int>>();
        private readonly TourGuestRepository tourGuestRepository;
        private readonly TourRepository tourRepository;
        private readonly TourReservationRepository tourReservationRepository;
        private readonly TourStartDateRepository tourStartDateRepository;
        private readonly UserRepository userRepository;
        public ObservableCollection<TourReservationDTO> Reservations { get; set; }
        private int currentGuestCount = 0;
        private int maxGuests;
        public TourReservationWindow(TourDTO selectedTour)
        {
            InitializeComponent();
            DataContext = this;

            tourRepository = new TourRepository();
            tourStartDateRepository = new TourStartDateRepository();
            tourReservationRepository = new TourReservationRepository();
            tourGuestRepository = new TourGuestRepository();
            userRepository = new UserRepository();
            Reservations = new ObservableCollection<TourReservationDTO>();

            maxGuests = 0;
            this.selectedTour = selectedTour;


            Update();
        }


        public void Update()
        {

        }

        private void CancelTour(object sender, RoutedEventArgs e)
        {


            this.Close();
        }

        private void ConfirmTourReservation(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput(out int numberOfPeople, out int age))
            {
                return;
            }

            if (!IsTourCapacitySufficient(numberOfPeople))
            {
                EnableGuestNumberInput();
                return;
            }

            ProcessGuestAddition(numberOfPeople, age);
        }

        private bool IsTourCapacitySufficient(int numberOfGuests)
        {
            Tour tour = ValidateTourCapacity(numberOfGuests);
            if (tour != null)
            {
                txtNumberOfPeople.IsEnabled = false;
                return true;
            }

            return false;
        }

        private void EnableGuestNumberInput()
        {
            txtNumberOfPeople.IsEnabled = true;
            txtNumberOfPeople.Focus();
        }

        private void ProcessGuestAddition(int numberOfPeople, int age)
        {
            maxGuests = numberOfPeople;
            AddGuest(txtNameSurname.Text, age);
        }

        private bool ValidateInput(out int numberOfPeople, out int age)
        {
            numberOfPeople = 0;
            age = 0;

            if (!int.TryParse(txtNumberOfPeople.Text, out numberOfPeople) || numberOfPeople <= 0)
            {
                MessageBox.Show("Unesite validan broj ljudi.");
                return false;
            }

            if (!int.TryParse(txtAge.Text, out age))
            {
                MessageBox.Show("Unesite validan broj za godine.");
                return false;
            }
            maxGuests = numberOfPeople;
           
            return true;
        }

        private Tour ValidateTourCapacity(int numberOfPeople)
        {
            Tour tour = tourRepository.GetById(this.selectedTour.Id);
            if (tour == null)
            {
                MessageBox.Show("Greška, nije pronađena ta tura.");
                return null;
            }

            if (numberOfPeople > tour.Capacity)
            {
                MessageBox.Show($"Na turi koju ste odabrali nema mjesta za odabrani broj ljudi, broj trenutno slobodnih mjesta je: {tour.Capacity}");
                return null;
            }

            return tour;
        }

        private BookingApp.Model.TourReservation CreateReservation(int numberOfPeople)
        {
            return tourReservationRepository.AddNewReservation(this.selectedTour.SelectedDateTime.Id, userRepository.GetCurrentUserId(), numberOfPeople);
        }

        private void AddGuest(string fullName, int age)
        {
            if (currentGuestCount >= maxGuests)
            {
                MessageBox.Show($"Već ste dodali maksimalan broj gostiju: {maxGuests}.");
                return;
            }

            temporaryGuests.Add(Tuple.Create(fullName, age));
            currentGuestCount++;
            UpdateTourCapacity();
            ShowGuestAddedMessage();
        }

        private void UpdateTourCapacity()
        {
            Tour tour = tourRepository.GetById(selectedTour.Id);
            if (tour == null) return;

            tour.Capacity--;
            tourRepository.Update(tour);
        }

        private void ShowGuestAddedMessage()
        {
            MessageBox.Show("Gost je uspješno dodat.");
            if (currentGuestCount == maxGuests)
            {
                FinishReservation();
            }
            else
            {
                MessageBox.Show($"Trenutno dodato gostiju: {currentGuestCount}. Možete dodati još {maxGuests - currentGuestCount} gostiju.");
            }

            ResetGuestInputFields();
        }

        private void FinishReservation()
        {
            BookingApp.Model.TourReservation newReservation = CreateReservation(maxGuests);
            if (newReservation == null)
            {
                MessageBox.Show("Nije moguće kreirati rezervaciju.");
                return;
            }

            AddTemporaryGuests(newReservation);
            UpdateTourInformation(newReservation);
        }

        private void AddTemporaryGuests(TourReservation newReservation)
        {
            foreach (Tuple<string, int> guest in temporaryGuests)
            {
                TourGuest newGuest = new TourGuest
                {
                    FullName = guest.Item1,
                    Age = guest.Item2,
                    TourReservationId = newReservation.Id,
                    CheckPointId = -1 
                };
                tourGuestRepository.Add(newGuest);
            }
        }

        private void UpdateTourInformation(TourReservation newReservation)
        {
            Tour tour = tourRepository.GetById(selectedTour.Id);
            if (tour != null)
            {
                int remainingCapacity = tour.Capacity;
                MessageBox.Show($"VAŠA REZERVACIJA JE USPJEŠNO KREIRANA!!! \n Sa {temporaryGuests.Count} gostiju. \nPREOSTALO MJESTA NA TURI: {remainingCapacity}");
            }
            else
            {
                MessageBox.Show("Došlo je do greške prilikom ažuriranja informacija o turi.");
            }
        }
        private void ResetGuestInputFields()
        {
            txtNameSurname.Text = string.Empty;
            txtAge.Text = string.Empty;
        }
    }




   
}
