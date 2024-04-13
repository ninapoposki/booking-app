using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Services;
using BookingApp.WPF.View.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class TourReservationWindowVM:ViewModelBase
    {

        public TourReservationDTO TourReservationDTO { get; set; }
        private TourDTO selectedTour;
        private List<Tuple<string, int>> temporaryGuests = new List<Tuple<string, int>>();
        private readonly TourGuestService tourGuestService;
        private readonly TourService tourService;
        private readonly TourReservationService tourReservationService;
        private readonly TourStartDateService tourStartDateService;
        private readonly UserService userService;
        private string username;
        public ObservableCollection<TourReservationDTO> Reservations { get; set; }
        private int currentGuestCount = 0;
        private int maxGuests;
        public TourReservationWindowVM(TourDTO selectedTour,string username)
        {
            this.username = username;
            tourService = new TourService();
            tourStartDateService = new TourStartDateService();
            tourReservationService = new TourReservationService();
            this.selectedTour= selectedTour;    
            tourGuestService = new TourGuestService();
            Reservations = new ObservableCollection<TourReservationDTO>();
            userService = new UserService();
            maxGuests = 0;
            
        }

       

        public void ConfirmTourReservation()
        {
            if (!ValidateInput(out int numberOfPeople, out int age))
            {
                return;
            }
            if (!ValidateTourCapacity(numberOfPeople))
            {
                IsInputEnabled = true;
                return;
            }
            if (!IsTourCapacitySufficient(numberOfPeople))
            {
                EnableGuestNumberInput();
                return;
            }
            ProcessGuestAddition(numberOfPeople, age);
        }

        private bool isInputEnabled = true;
        public bool IsInputEnabled
        {
            get => isInputEnabled;
            set
            {
                isInputEnabled = value;
                OnPropertyChanged(nameof(IsInputEnabled));
            }
        }
        public bool IsTourCapacitySufficient(int numberOfGuests)
        {
           
            if (tourService.IsCapacitySufficient(selectedTour.Id, numberOfGuests))
            {
                IsInputEnabled = false;
                return true;
            }

            return false;
        }


        private bool shouldFocusTextBox;
        public bool ShouldFocusTextBox
        {
            get => shouldFocusTextBox;
            set
            {
                shouldFocusTextBox = value;
                OnPropertyChanged(nameof(ShouldFocusTextBox));
            }
        }

        private int ttxtNumberOfPeople;
        public int txtNumberOfPeople
        {
            get => ttxtNumberOfPeople;
            set
            {
                ttxtNumberOfPeople = value;
                OnPropertyChanged(nameof(txtNumberOfPeople));
            }
        }



        public void EnableGuestNumberInput()
        {
            //IsInputEnabled = true;
           // txtNumberOfPeople.Focus();
        }

        private string nameSurname;
        public string NameSurname
        {
            get => nameSurname;
            set
            {
                nameSurname = value;
                OnPropertyChanged(nameof(NameSurname));
            }
        }

        private int age;
      

        public int Age
        {
            get => age;
            set
            {
                age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        public void ProcessGuestAddition(int numberOfPeople, int age)
        {
            maxGuests = numberOfPeople;
            AddGuest(NameSurname, Age);
        }

        public bool ValidateInput(out int numberOfPeople, out int age)
        { numberOfPeople = ttxtNumberOfPeople;
            age = Age;
            if ( numberOfPeople <= 0)
            {MessageBox.Show("Unesite validan broj ljudi.");
                return false; }
            if (age<0)
            { MessageBox.Show("Unesite validan broj za godine.");
                return false; }
            if (currentGuestCount == 0)
            { maxGuests = numberOfPeople;
                RemainingGuestsToAdd = maxGuests;
                IsInputEnabled = false;}
            return true;
        }

        private bool ValidateTourCapacity(int numberOfPeople)
        {
            int capacity = tourService.GetTourCapacity(selectedTour.Id);
            if (capacity == -1)
            {
                MessageBox.Show("Greška, nije pronađena ta tura.");
                return false;
            }
            if (numberOfPeople > capacity)
            {
                MessageBox.Show($"Na turi koju ste odabrali nema mjesta za odabrani broj ljudi, broj trenutno slobodnih mjesta je: {capacity}");
                return false;
            }
            return true;
        }
        private int remainingGuestsToAdd;
        public int RemainingGuestsToAdd
        {
            get => remainingGuestsToAdd;
            set
            {
                remainingGuestsToAdd = value;
                OnPropertyChanged(nameof(RemainingGuestsToAdd));
            }
        }
        public void AddGuest(string fullName, int age)
        {
            if (currentGuestCount >= maxGuests || RemainingGuestsToAdd <= 0)
            {
                MessageBox.Show($"Već ste dodali maksimalan broj gostiju: {maxGuests}.");
                return;
            }
            temporaryGuests.Add(Tuple.Create(fullName, age));
            currentGuestCount++;
            RemainingGuestsToAdd--;
            ShowGuestAddedMessage();
        }

        public void UpdateTourCapacity()
        {
            int remainingCapacity;  
            bool success = tourService.UpdateTourCapacity(selectedTour.Id,out remainingCapacity);
            if (success)
            {
                MessageBox.Show($"Kapacitet ažuriran.");
            }
            else
            {
                MessageBox.Show("Došlo je do greške");
            }
        }

        public void ShowGuestAddedMessage()
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
       
        public void FinishReservation()
        { 
            var user=userService.GetByUsername(username);
            if (!tourReservationService.TryCreateReservation(selectedTour.SelectedDateTime.Id, user.Id,username, maxGuests, out int reservationId))
            {
                MessageBox.Show("Nije moguće kreirati rezervaciju.");
                return;
            }
            AddTemporaryGuests(reservationId);
            UpdateTourCapacity();
            UpdateTourInformation(reservationId);
        }

        public void AddTemporaryGuests(int reservationId)
        {
            foreach (Tuple<string, int> guest in temporaryGuests)
            {
                tourGuestService.AddGuest(guest.Item1, guest.Item2, reservationId);
            }
        }

        public void UpdateTourInformation(int reservationId)
        {
            int guestsToAdd = temporaryGuests.Count;
            if (tourService.UpdateTourCapacity(selectedTour.Id, out int remainingCapacity))
            {
                MessageBox.Show($"VAŠA REZERVACIJA JE USPJEŠNO KREIRANA!!! \n Sa {temporaryGuests.Count} gostiju. \nPREOSTALO MJESTA NA TURI: {tourService.GetTourCapacity(selectedTour.Id)}");
            }
            else
            {
                MessageBox.Show("Došlo je do greške prilikom ažuriranja informacija o turi.");
            }
        }
        public void ResetGuestInputFields()
        {
             NameSurname= string.Empty;
            Age = 0;
        }
      

    }




}

