using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Services;
using BookingApp.Utilities;
using BookingApp.WPF.View.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class TourReservationWindowVM:ViewModelBase
    {
        private TourDTO selectedTour;
        public TourDTO SelectedTour
        { get => selectedTour;
            set{ if (selectedTour != value){
                    selectedTour = value;
                    OnPropertyChanged(nameof(SelectedTour));}}}
        private TourGuest _currentGuest;
        public TourGuest CurrentGuest
        {
            get => _currentGuest;
            set
            {
                if (_currentGuest != value)
                {
                    _currentGuest = value;
                    OnPropertyChanged(nameof(CurrentGuest));
                }
            }
        }
        public string SelectedGender
        {
            get => CurrentGuest?.Gender.ToString();
            set
            {
                if (CurrentGuest != null && Enum.TryParse<Gender>(value, out var newGender))
                {
                    CurrentGuest.Gender = newGender;
                    OnPropertyChanged(nameof(SelectedGender));
                    ConfirmTourReservationCommand.RaiseCanExecuteChanged();
                }
            }
        }
        private ObservableCollection<Tuple<string, int>> TemporaryGuests = new ObservableCollection<Tuple<string, int>>();
        public ObservableCollection<Tuple<string, int>> temporaryGuests 
    
        {
            get { return TemporaryGuests; }
            set
            {
                if (TemporaryGuests != value)
                {
                    TemporaryGuests = value;
                    OnPropertyChanged(nameof(temporaryGuests));
                }
            }
        }
        private readonly TourGuestService tourGuestService;
        private readonly TourService tourService;
        private readonly ImageService imageService;
        private readonly TourReservationService tourReservationService;
 
        private readonly UserService userService;
        private readonly VoucherService voucherService;
        private string username;
        public ObservableCollection<TourReservationDTO> Reservations { get; set; }
        private int currentGuestCount = 0;
        private int maxGuests;
        private VoucherDTO selectedVoucher;
        public ObservableCollection<VoucherDTO> AllVouchers { get; set; }
        public ObservableCollection<string> Genders { get; set; }
        public MyICommand ConfirmTourReservationCommand { get; set; }
        public VoucherDTO SelectedVoucher
        {get => selectedVoucher;
            set
            {selectedVoucher = value;
               OnPropertyChanged(nameof(SelectedVoucher));
                ConfirmTourReservationCommand.RaiseCanExecuteChanged();
            } }
        private int currentIndex = 0;
        private ObservableCollection<ImageDTO> images;
        private ImageDTO _currentImage;
        public ImageDTO CurrentImage
        {
            get => _currentImage;
            set
            {
                if (_currentImage != value)
                {
                    _currentImage = value;
                    OnPropertyChanged(nameof(CurrentImage));

                }
            }
        }
        public MyICommand MoveNextCommand { get; set; }
        public MyICommand MovePreviousCommand { get; set; }
        public MyICommand<Tuple<string, int>> RemoveGuestCommand { get; set; }
        public TourReservationWindowVM(TourDTO selectedTour,string username)
        {
            this.username = username;
            
            tourService = new TourService(Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            tourReservationService = new TourReservationService(Injector.Injector.CreateInstance<ITourReservationRepository>(), Injector.Injector.CreateInstance<ITourGuestRepository>(),
                Injector.Injector.CreateInstance<IUserRepository>(), Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(),
                Injector.Injector.CreateInstance<ILanguageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>());
           
            tourGuestService = new TourGuestService(Injector.Injector.CreateInstance<ITourGuestRepository>());
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            Reservations = new ObservableCollection<TourReservationDTO>();
            userService = new UserService(Injector.Injector.CreateInstance<IUserRepository>());
            voucherService = new VoucherService(Injector.Injector.CreateInstance<IVoucherRepository>(), Injector.Injector.CreateInstance<ITourReservationRepository>(), Injector.Injector.CreateInstance<ITourGuestRepository>(),
                 Injector.Injector.CreateInstance<IUserRepository>(), Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(),
                 Injector.Injector.CreateInstance<ILanguageRepository>(),
                 Injector.Injector.CreateInstance<ILocationRepository>());
            selectedVoucher = new VoucherDTO();
            AllVouchers = new ObservableCollection<VoucherDTO>();
            maxGuests = 0;
            //TourImage = selectedTour.Path; 
            //TourDescription = selectedTour.Description;
           ConfirmTourReservationCommand = new MyICommand(ConfirmTourReservation, CanConfirmTourReservation);
            images = new ObservableCollection<ImageDTO>(); 
            if (images.Any())
            {
                CurrentImage = images[0];
            }
            this.selectedTour= selectedTour;
            SetSelectedTour(selectedTour);
            MoveNextCommand = new MyICommand(MoveNext);
            MovePreviousCommand = new MyICommand(MovePrevious);
            Genders = new ObservableCollection<string>(Enum.GetNames(typeof(Gender)));
            CurrentGuest = new TourGuest();
            RemoveGuestCommand = new MyICommand<Tuple<string, int>>(RemoveGuest);
            Update();
        }
        private bool CanConfirmTourReservation()
        {   
            return !string.IsNullOrEmpty(NameSurname) && !string.IsNullOrEmpty(Age) && !string.IsNullOrEmpty(txtNumberOfPeople) && SelectedGender != null && SelectedVoucher != null;
        }
        private void MoveNext()
        {
            if (currentIndex < images.Count - 1)
            {
                currentIndex++;
                CurrentImage = images[currentIndex];
                OnPropertyChanged(nameof(CurrentImage));
            }
        }
        private void RemoveGuest(Tuple<string, int> guest)
        {
            if (guest != null)
            {
                temporaryGuests.Remove(guest);
                currentGuestCount--;
                RemainingGuestsToAdd++;
                OnPropertyChanged(nameof(RemainingGuestsToAdd));
            }
        }
        private void MovePrevious()
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                CurrentImage = images[currentIndex];
                OnPropertyChanged(nameof(CurrentImage));
            }
        }
        public void SetSelectedTour(TourDTO tour)
        {
            SelectedTour = tour;
            images = new ObservableCollection<ImageDTO>(imageService.GetImagesForTour(tour.Id));
            if (images.Any())
            {
                CurrentImage = images[0];
                OnPropertyChanged(nameof(CurrentImage));
            }
            OnPropertyChanged(nameof(SelectedTour));
        }
        public void Update()
        {GetVouchers();}
        public void GetVouchers()
        {
            AllVouchers.Clear();
            List<VoucherDTO> vouchers = voucherService.GetAll().Where(v => v.Status == Domain.Model.Status.VALID).ToList();
            foreach (VoucherDTO voucher in vouchers){
                AllVouchers.Add(voucher);}
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
        {get => isInputEnabled;
            set {isInputEnabled = value;
                OnPropertyChanged(nameof(IsInputEnabled));}}
        public bool IsTourCapacitySufficient(int numberOfGuests)
        {if (tourService.IsCapacitySufficient(selectedTour.Id, numberOfGuests)){
                IsInputEnabled = false;
                return true; }
            return false;}

        private bool shouldFocusTextBox;
        public bool ShouldFocusTextBox
        {get => shouldFocusTextBox;
            set {shouldFocusTextBox = value;
                OnPropertyChanged(nameof(ShouldFocusTextBox)); }}

        private string ttxtNumberOfPeople;
        public string txtNumberOfPeople
        { get => ttxtNumberOfPeople;
            set { ttxtNumberOfPeople = value;
                OnPropertyChanged(nameof(txtNumberOfPeople));
                ConfirmTourReservationCommand.RaiseCanExecuteChanged();
            }
        }

        public void EnableGuestNumberInput() {}

        private string nameSurname;
        public string NameSurname
        {get => nameSurname;
            set{nameSurname = value;
                OnPropertyChanged(nameof(NameSurname));
                ConfirmTourReservationCommand.RaiseCanExecuteChanged();
            }
        }

        private string age;
        public string Age
        { get => age;
            set{age = value;
                OnPropertyChanged(nameof(Age));
                ConfirmTourReservationCommand.RaiseCanExecuteChanged();
            } }

        public void ProcessGuestAddition(int numberOfPeople, int age)
        {age = Convert.ToInt32(Age);
            maxGuests = numberOfPeople;
            AddGuest(NameSurname, age); }

        public bool ValidateInput(out int numberOfPeople, out int age)
        {   numberOfPeople = 0; 
            age = 0;
            if (!int.TryParse(txtNumberOfPeople, out numberOfPeople) || numberOfPeople <= 0)
            {MessageBox.Show("Unesite validan broj ljudi.");
                return false; }
            if (!int.TryParse(Age, out age) ||  age < 0)
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
        {get => remainingGuestsToAdd;
            set{remainingGuestsToAdd = value;
                OnPropertyChanged(nameof(RemainingGuestsToAdd));} }
        public void AddGuest(string fullName, int age)
        { if (currentGuestCount >= maxGuests || RemainingGuestsToAdd <= 0){
                MessageBox.Show($"Već ste dodali maksimalan broj gostiju: {maxGuests}.");
                return; }
            var newGuest = Tuple.Create(fullName, age);
            temporaryGuests.Add(Tuple.Create(fullName, age));
            currentGuestCount++;
            RemainingGuestsToAdd--;
            OnPropertyChanged(nameof(RemainingGuestsToAdd));
            ShowGuestAddedMessage();
        }

        public void UpdateTourCapacity(int numberOfGuestsToAdd)
        { bool success = tourService.UpdateTourCapacity(selectedTour.Id, numberOfGuestsToAdd, out int remainingCapacity);
            if (success){
                MessageBox.Show($"Kapacitet ažuriran. Preostalo mesta: {remainingCapacity}."); }
            else{
                MessageBox.Show("Došlo je do greške");}}

        public void ShowGuestAddedMessage()
        {MessageBox.Show("Gost je uspješno dodat.");
            if (currentGuestCount == maxGuests){
                FinishReservation(); }
            else {
                MessageBox.Show($"Trenutno dodato gostiju: {currentGuestCount}. Možete dodati još {maxGuests - currentGuestCount} gostiju."); }
                ResetGuestInputFields();}
     
       public void FinishReservation()
        {   var user=userService.GetByUsername(username);
            if (!tourReservationService.TryCreateReservation(selectedTour.SelectedDateTime.Id, user.Id,username, maxGuests, out int reservationId))
            {   MessageBox.Show("Nije moguće kreirati rezervaciju.");
                return;
            }
            if (SelectedVoucher != null && SelectedVoucher.Description != "Ne koristi vaučer")
            {   SelectedVoucher.Status = Status.USED;
                SelectedVoucher.TourReservationId = reservationId;
                voucherService.UpdateVoucherFromDTO(SelectedVoucher); }
            Update();
            AddTemporaryGuests(reservationId);
            int remainingCapacity;
            if (tourService.UpdateTourCapacity(selectedTour.Id, maxGuests, out remainingCapacity))
            { MessageBox.Show($"Kapacitet ture ažuriran. Preostalo mesta: {remainingCapacity}."); }
            else
            { MessageBox.Show("Došlo je do greške prilikom ažuriranja kapaciteta ture.");}}

        public void AddTemporaryGuests(int reservationId)
        { foreach (Tuple<string, int> guest in temporaryGuests) {
                tourGuestService.AddGuest(guest.Item1, guest.Item2, reservationId,CurrentGuest.Gender);} }

        public void ResetGuestInputFields()
        {  NameSurname= string.Empty;
            Age = string.Empty;      }      
    }
}

