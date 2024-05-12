using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class TourRequestWindowVM: ViewModelBase
    {
       private LocationService locationService;
        private LanguageService languageService;
        private readonly TourGuestService tourGuestService;
        private TourRequestService tourRequestService;
        public TourDTO TourDTO { get; set; }
        public NavigationService NavigationService { get; set; }
        public List<LanguageDTO> LanguageComboBox { get; set; }
        public List<LocationDTO> LocationComboBox { get; set; }
        public TourRequestDTO CurrentRequest { get; set; }
        public ObservableCollection<string> Genders { get; set; }
        public ObservableCollection<TourGuestDTO> Guests { get; set; }
        public MyICommand ConfirmTourRequestCommand { get; set; }
        public bool IsNumberOfTouristsReadOnly { get; set; } = false;
        private TourGuest currentGuest;
        public TourGuest CurrentGuest
        {get => currentGuest;
            set
            {if (currentGuest != value)
                {   currentGuest = value;
                    OnPropertyChanged(nameof(CurrentGuest));
                    ConfirmTourRequestCommand.RaiseCanExecuteChanged(); }
            }
        }
        public LanguageDTO selectedLanguage;
        public LanguageDTO SelectedLanguage
        { get { return selectedLanguage; }
            set{
                if (selectedLanguage != value)
                {
                    selectedLanguage = value;
                    if (selectedLanguage != null)
                    {
                        CurrentRequest.LanguageId = selectedLanguage.Id;
                        OnPropertyChanged("SelectedLanguage");}}}}
         public string SelectedGender
        {
            get => CurrentGuest?.Gender.ToString();
            set
            {
                if (CurrentGuest != null && Enum.TryParse<Gender>(value, out var newGender))
                {
                    CurrentGuest.Gender = newGender;
                    OnPropertyChanged(nameof(SelectedGender));
                    ConfirmTourRequestCommand?.RaiseCanExecuteChanged();
                }
            }
        }
        private int _numberOfTourists;
        public int NumberOfTourists
        {
            get => _numberOfTourists;
            set
            {
                _numberOfTourists = value;
                OnPropertyChanged(nameof(NumberOfTourists));
            }
        }
        private int remainingGuests;
        public int RemainingGuests
        {
            get => remainingGuests;
            set
            {
                if (remainingGuests != value)
                {
                    remainingGuests = value;
                    OnPropertyChanged(nameof(RemainingGuests));
                }
            }
        }
         public TourRequestWindowVM(NavigationService navigationService) {

            NavigationService = navigationService;
            locationService = new LocationService(Injector.Injector.CreateInstance<ILocationRepository>());
            languageService = new LanguageService(Injector.Injector.CreateInstance<ILanguageRepository>());
            tourGuestService = new TourGuestService(Injector.Injector.CreateInstance<ITourGuestRepository>());
            tourRequestService = new TourRequestService(Injector.Injector.CreateInstance<ITourRequestRepository>(), Injector.Injector.CreateInstance<ILocationRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>());
            LocationComboBox = new List<LocationDTO>();
            LanguageComboBox = new List<LanguageDTO>();
            CurrentRequest = new TourRequestDTO();
            SelectedLanguage = new LanguageDTO();
            ConfirmTourRequestCommand = new MyICommand(ConfirmRequest);
            TourDTO = new TourDTO();
            Genders = new ObservableCollection<string>(Enum.GetNames(typeof(Gender)));
            Guests= new ObservableCollection<TourGuestDTO> ();
            CurrentGuest = new TourGuest();
            LoadLanguagesAndCities();
        }
       private void LoadLanguagesAndCities()
        {
            languageService.GetAll(LanguageComboBox);
            locationService.GetAll(LocationComboBox);
        }
        private void LoadCountry()
        {
            Country = locationService.GetCountry(SelectedCity);
        }

        private string country;
        public string Country
        {
            get { return country; }
            set
            {
                if (country != value)
                {
                    country = value;
                    OnPropertyChanged("Country");
                }
            }
        }
        private LocationDTO selectedCity;
        public LocationDTO SelectedCity
        { get { return selectedCity; }
            set{
                if (selectedCity != value)
                {
                    selectedCity = value;
                    CurrentRequest.LocationId = selectedCity.Id;
                    OnPropertyChanged("SelectedCity");
                    LoadCountry();}} }   
        private void ConfirmRequest()
        {
            if (!IsNumberOfTouristsReadOnly)
            {IsNumberOfTouristsReadOnly = true;}
            if (Guests.Count < NumberOfTourists)
            {
                if (Guests.Count == 0)
                { int tourRequestId = CompleteRequest();
                    CurrentRequest.Id = tourRequestId; 
                }
                tourGuestService.AddGuestRequest(CurrentGuest.FullName, CurrentGuest.Age, CurrentRequest.Id, CurrentGuest.Gender);
                Guests.Add(new TourGuestDTO(CurrentGuest));
                CurrentGuest = new TourGuest();
                RemainingGuests = NumberOfTourists - Guests.Count;

                if (Guests.Count == NumberOfTourists)
                {
                    MessageBox.Show("All guests have been added.");
                    IsNumberOfTouristsReadOnly = false; 
                }
            }else{MessageBox.Show("All guests have been added.");}
        }
        private DateTime? startDate;
        public DateTime? StartDate
        {
            get => string.IsNullOrEmpty(CurrentRequest.StartDate) ? null : DateTime.ParseExact(CurrentRequest.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            set
            {
                if (value.HasValue)
                {
                    CurrentRequest.StartDate = value.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    CurrentRequest.StartDate = null;
                }
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private DateTime? endDate;
        public DateTime? EndDate
        {
            get => string.IsNullOrEmpty(CurrentRequest.EndDate) ? null : DateTime.ParseExact(CurrentRequest.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            set
            {
                if (value.HasValue)
                {
                    CurrentRequest.EndDate = value.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    CurrentRequest.EndDate = null;
                }
                OnPropertyChanged(nameof(EndDate));
            }
        }
        private int CompleteRequest()
        {
            DateOnly start = DateOnly.FromDateTime(StartDate.Value);
            DateOnly end = DateOnly.FromDateTime(EndDate.Value);
            CurrentRequest.NumberOfTourists = NumberOfTourists;
            TourRequest newTourRequest = new TourRequest(
                    CurrentRequest.LocationId,
                    CurrentRequest.LanguageId,
                    CurrentRequest.Description,
                    CurrentRequest.NumberOfTourists,
                    start,
                    end);
            int tourRequestId = tourRequestService.SaveTourRequest(newTourRequest).Id;
            MessageBox.Show("Tour request created successfully with all guests.");
            IsNumberOfTouristsReadOnly = false;
            return tourRequestId;
        }
    }
}
