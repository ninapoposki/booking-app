using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using BookingApp.WPF.View.Guest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class GuestMainWindowVM : ViewModelBase
    {
        private readonly AccommodationService accommodationService;
        private readonly ImageService imageService;
        private readonly LocationService locationService;
        private readonly OwnerService ownerService;
        public NavigationService navigationService { get; set; }
        public GuestDTO guestDTO { get; set; }

        public ObservableCollection<AccommodationDTO> AllAccommodations { get; set; }
        public ObservableCollection<ImageDTO> Images { get; set; }
        public ObservableCollection<AccommodationType> Types { get; set; }
        private AccommodationDTO selectedAccommodation;
        public AccommodationDTO SelectedAccommodation
        {
            get => selectedAccommodation;
            set
            {
                if (selectedAccommodation != value)
                {
                    selectedAccommodation = value;
                    OnPropertyChanged(nameof(SelectedAccommodation));
                    BookAccommodationCommand.RaiseCanExecuteChanged();  
                }
            }
        }
        private ObservableCollection<string> _currentCitySource;
        public ObservableCollection<string> CurrentCitySource
        {
            get => _currentCitySource;
            set
            {
                _currentCitySource = value;
                OnPropertyChanged(nameof(CurrentCitySource));
            }
        }
        private ObservableCollection<string> _currentCountrySource;
        public ObservableCollection<string> CurrentCountrySource
        {
            get => _currentCountrySource;
            set
            {
                _currentCountrySource = value;
                OnPropertyChanged(nameof(CurrentCountrySource));
            }
        }

        private string nameFilter;
        public string NameFilter{
            get => nameFilter;
            set { nameFilter = value; OnPropertyChanged(nameof(NameFilter)); }
        }
        private string cityFilter;
        public string CityFilter {
            get => cityFilter;
            set{ cityFilter = value; OnPropertyChanged(nameof(CityFilter)); UpdateCitySuggestions(cityFilter); }
        }
        private string countryFilter;
        public string CountryFilter{
            get => countryFilter;
            set { countryFilter = value; OnPropertyChanged(nameof(CountryFilter)); UpdateCountrySuggestions(countryFilter);}
        }
        private AccommodationType? selectedType;
        public AccommodationType? SelectedType {
            get => selectedType;
            set { selectedType = value; OnPropertyChanged(nameof(SelectedType));}
        }
        private string numberOfGuests;
        public string NumberOfGuests {
            get => numberOfGuests;
            set { numberOfGuests = value; OnPropertyChanged(nameof(NumberOfGuests));}
        }
        private string numberOfDaysToStay;
        public string NumberOfDaysToStay{
            get => numberOfDaysToStay;
            set { numberOfDaysToStay = value; OnPropertyChanged(nameof(NumberOfDaysToStay));}
        }
        public ObservableCollection<string> CitySuggestions { get; set; }
        public ObservableCollection<string> CountrySuggestions { get; set; }
        public ObservableCollection<string> CityComboBox { get; set; }
        public ObservableCollection<string> CountryComboBox { get; set; }
        public string SelectedCountry { get; set; }
        private string selectedCity;
        public string SelectedCity
        {
            get { return selectedCity; }
            set { if (selectedCity != value) { selectedCity = value; OnPropertyChanged(nameof(SelectedCity)); } }
        }
        public MyICommand <AccommodationDTO> BookAccommodationCommand { get; set; }
        public MyICommand SearchCommand { get; set; }

        public GuestMainWindowVM(NavigationService navigationService,GuestDTO guestDTO){
            accommodationService = new AccommodationService(Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>());
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            locationService = new LocationService(Injector.Injector.CreateInstance<ILocationRepository>());
            ownerService = new OwnerService(Injector.Injector.CreateInstance<IOwnerRepository>());
            this.navigationService=navigationService;
            this.guestDTO = guestDTO;
            AllAccommodations = new ObservableCollection<AccommodationDTO>();
            Images = new ObservableCollection<ImageDTO>();
            Types = new ObservableCollection<AccommodationType>(Enum.GetValues(typeof(AccommodationType)).Cast<AccommodationType>());
            CitySuggestions = new ObservableCollection<string>();
            CountrySuggestions = new ObservableCollection<string>();
            BookAccommodationCommand = new MyICommand<AccommodationDTO>(OnBookAccommodation); 
            SearchCommand = new MyICommand(OnSearchCommand);
            CityComboBox = new ObservableCollection<string>();
            CountryComboBox = new ObservableCollection<string>();
            CurrentCitySource = new ObservableCollection<string>();
            CurrentCountrySource= new ObservableCollection<string>();
            LoadCountries();
            LoadCity();
            Update();
        }
        public void Update(){
            AllAccommodations.Clear();
            var allImages = imageService.GetImagesForEntityType(EntityType.ACCOMMODATION);
            AddAccommodations(allImages, "SUPEROWNER");
            AddAccommodations(allImages, "OWNER");}
        private void LoadCountries()
        {
            CountryComboBox.Clear();
            foreach (String country in accommodationService.locationService.GetAllCountries()) CountryComboBox.Add(country);
            CurrentCountrySource = new ObservableCollection<string>(CountryComboBox); 

        }
        private void LoadCity()
        {
            
                CityComboBox.Clear();
                foreach (String city in accommodationService.locationService.GetCities()) CityComboBox.Add(city);
            
            CurrentCitySource = new ObservableCollection<string>(CityComboBox); 

        }
        public void UpdateCitySuggestions(string input)
        {
             if (string.IsNullOrWhiteSpace(input))
             {
                CurrentCitySource = new ObservableCollection<string>(CityComboBox); 

            }
            else
             {
                 var suggestions = locationService.GetAutocompleteCity(input);
                CurrentCitySource = new ObservableCollection<string>(suggestions); 
            }
            OnPropertyChanged(nameof(CurrentCitySource));

        }
        public void UpdateCountrySuggestions(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) {
                CurrentCountrySource = new ObservableCollection<string>(CountryComboBox); 

            }
            else
             {
                 var suggestions = locationService.GetAutocompleteCountry(input);
                CurrentCountrySource = new ObservableCollection<string>(suggestions); 
            }
            OnPropertyChanged(nameof(CurrentCountrySource));

        }

        public void AddAccommodations(List<ImageDTO> allImages, string role)
        {
            var accommodations = accommodationService.GetAll();
            foreach (var accommodation in accommodations)
            {
                var owner = ownerService.GetByIdDTO(accommodation.OwnerId);
                if (owner.Role == role)
                {
                    var matchingImages = new ObservableCollection<ImageDTO>(imageService.GetImagesByAccommodation(accommodation.Id, allImages));
                    if (matchingImages.Count == 0) {
                        matchingImages.Add(new ImageDTO { Path = @"\Resources\Images\placeholder_accommodation.jpg" });
                    }
                    LocationDTO location = locationService.GetByIdDTO(accommodation.IdLocation);
                    AllAccommodations.Add(new AccommodationDTO(accommodation.ToAccommodation(), location.ToLocation()) { Images = matchingImages });
                }
            }
        }
        private void OnSearchCommand()
        {
            Update();
            var filteredAccommodations = FilterAccommodations();
            AllAccommodations.Clear();
            filteredAccommodations.ForEach(accommodation => AllAccommodations.Add(accommodation));
        }
        private List<AccommodationDTO> FilterAccommodations(){
            return AllAccommodations
                .Where(accommodation =>
                    (string.IsNullOrEmpty(NameFilter) || accommodation.Name.ToLower().Contains(NameFilter.ToLower())) &&
                    (string.IsNullOrEmpty(CityFilter) || accommodation.Location.City.ToLower().Contains(CityFilter.ToLower())) &&
                    (string.IsNullOrEmpty(CountryFilter) || accommodation.Location.Country.ToLower().Contains(CountryFilter.ToLower())) &&
                    (!SelectedType.HasValue || accommodation.AccommodationType == SelectedType.Value) &&
                    (string.IsNullOrEmpty(NumberOfGuests) || accommodation.Capacity >= int.Parse(NumberOfGuests)) &&
                    (string.IsNullOrEmpty(NumberOfDaysToStay) || accommodation.MinStayDays <= double.Parse(NumberOfDaysToStay))).ToList();
        }
         private void OnBookAccommodation(AccommodationDTO SelectedAccommodation)
         {
             ReserveAccommodation reserveAccommodation = new ReserveAccommodation(navigationService,SelectedAccommodation,guestDTO);
             navigationService.Navigate(reserveAccommodation);
         }
    }
}