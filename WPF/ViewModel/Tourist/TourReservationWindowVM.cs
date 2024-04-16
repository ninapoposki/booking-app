using BookingApp.Domain.IRepositories;
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
        private readonly VoucherService voucherService;
        private string username;
        public ObservableCollection<TourReservationDTO> Reservations { get; set; }
        private int currentGuestCount = 0;
        private int maxGuests;
        private VoucherDTO selectedVoucher;
        public ObservableCollection<VoucherDTO> AllVouchers { get; set; }
        
        public VoucherDTO SelectedVoucher
        {
            get => selectedVoucher;
            set
            {
                selectedVoucher = value;
                OnPropertyChanged(nameof(SelectedVoucher));
            }
        }
        public TourReservationWindowVM(TourDTO selectedTour,string username)
        {
            this.username = username;
            tourService = new TourService(Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            tourStartDateService = new TourStartDateService(Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            tourReservationService = new TourReservationService(Injector.Injector.CreateInstance<ITourReservationRepository>(), Injector.Injector.CreateInstance<ITourGuestRepository>(),
                Injector.Injector.CreateInstance<IUserRepository>(), Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(),
                Injector.Injector.CreateInstance<ILanguageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>());
            this.selectedTour= selectedTour;
            tourGuestService = new TourGuestService(Injector.Injector.CreateInstance<ITourGuestRepository>());
            Reservations = new ObservableCollection<TourReservationDTO>();
            userService = new UserService(Injector.Injector.CreateInstance<IUserRepository>());
            voucherService = new VoucherService(Injector.Injector.CreateInstance<IVoucherRepository>(), Injector.Injector.CreateInstance<ITourReservationRepository>(), Injector.Injector.CreateInstance<ITourGuestRepository>(),
                 Injector.Injector.CreateInstance<IUserRepository>(), Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(),
                 Injector.Injector.CreateInstance<ILanguageRepository>(),
                 Injector.Injector.CreateInstance<ILocationRepository>());
            selectedVoucher = new VoucherDTO();
            AllVouchers = new ObservableCollection<VoucherDTO>();
            maxGuests = 0;
           
            Update();

            
        }


        public void Update()
        {

            GetVouchers();
        }

        public void GetVouchers()
        {

            AllVouchers.Clear();
           
            List<VoucherDTO> vouchers = voucherService.GetAll().Where(v => v.Status == Domain.Model.Status.VALID).ToList();
            foreach (VoucherDTO voucher in vouchers)
            {
                AllVouchers.Add(voucher);
            }
            AllVouchers.Insert(0, new VoucherDTO { Description = "Ne koristi vaučer" });
        }
        public void ConfirmTourReservation()
        {   if (!ValidateInput(out int numberOfPeople, out int age))
            {   return; }
            if (!ValidateTourCapacity(numberOfPeople))
            {  IsInputEnabled = true;
                return;}
            if (!IsTourCapacitySufficient(numberOfPeople))
            { EnableGuestNumberInput();
                return;}
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

        private string ttxtNumberOfPeople;
        public string txtNumberOfPeople
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

        private string age;
      

        public string Age
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
            age = Convert.ToInt32(Age);
            maxGuests = numberOfPeople;
            AddGuest(NameSurname, age);
        }

        public bool ValidateInput(out int numberOfPeople, out int age)
        { numberOfPeople = Convert.ToInt32(txtNumberOfPeople);
            age = Convert.ToInt32(Age);
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
            return true;}

        private bool ValidateTourCapacity(int numberOfPeople)
        {   int capacity = tourService.GetTourCapacity(selectedTour.Id);
            if (capacity == -1)
            {   MessageBox.Show("Greška, nije pronađena ta tura.");
                return false;
            }
            if (numberOfPeople > capacity)
            { MessageBox.Show($"Na turi koju ste odabrali nema mjesta za odabrani broj ljudi, broj trenutno slobodnih mjesta je: {capacity}");
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

        public void UpdateTourCapacity(int numberOfGuestsToAdd)
        {
            bool success = tourService.UpdateTourCapacity(selectedTour.Id, numberOfGuestsToAdd, out int remainingCapacity);
            if (success)
            {
                MessageBox.Show($"Kapacitet ažuriran. Preostalo mesta: {remainingCapacity}.");
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
        {   var user=userService.GetByUsername(username);
            if (!tourReservationService.TryCreateReservation(selectedTour.SelectedDateTime.Id, user.Id,username, maxGuests, out int reservationId))
            {   MessageBox.Show("Nije moguće kreirati rezervaciju.");
                return;
            }
            if (SelectedVoucher != null && SelectedVoucher.Description != "Ne koristi vaučer")
            {   SelectedVoucher.Status = Status.USED;
                SelectedVoucher.TourReservationId = reservationId;
                voucherService.UpdateVoucherFromDTO(SelectedVoucher);
            }
            Update();
            AddTemporaryGuests(reservationId);
          int remainingCapacity;
            if (tourService.UpdateTourCapacity(selectedTour.Id, maxGuests, out remainingCapacity))
            {
                MessageBox.Show($"Kapacitet ture ažuriran. Preostalo mesta: {remainingCapacity}.");
            }
            else
            {
                MessageBox.Show("Došlo je do greške prilikom ažuriranja kapaciteta ture.");
            }
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
            if (tourService.UpdateTourCapacity(selectedTour.Id, guestsToAdd, out int remainingCapacity))
            {
                MessageBox.Show($"VAŠA REZERVACIJA JE USPJEŠNO KREIRANA!!! \n Sa {guestsToAdd} gostiju. \nPREOSTALO MJESTA NA TURI: {remainingCapacity}");
            }
            else
            {
                MessageBox.Show("Došlo je do greške prilikom ažuriranja informacija o turi.");
            }
        }
        public void ResetGuestInputFields()
        {
             NameSurname= string.Empty;
            Age = string.Empty;
        }
      

    }




}

