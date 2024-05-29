using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Navigation;
using BookingApp.Services;
using BookingApp.Domain.IRepositories;
using BookingApp.Utilities;
using BookingApp.DTO;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class CreateForumVM : ViewModelBase
    {
        private readonly LocationService locationService;
        private readonly AccommodationService accommodationService;
        private readonly ForumService forumService;
        private readonly ForumCommentService forumCommentService;
        private readonly GuestService guestService;
        public NavigationService NavigationService { get; set; }
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

        private string _cityFilter;
        public string CityFilter
        {
            get => _cityFilter;
            set
            {
                _cityFilter = value;
                OnPropertyChanged(nameof(CityFilter));
                UpdateCitySuggestions(_cityFilter);
            }
        }

        private string _countryFilter;
        public string CountryFilter
        {
            get => _countryFilter;
            set
            {
                _countryFilter = value;
                OnPropertyChanged(nameof(CountryFilter));
                UpdateCountrySuggestions(_countryFilter);
            }
        }
        public ObservableCollection<string> CitySuggestions { get; set; }
        public ObservableCollection<string> CountrySuggestions { get; set; }
        public ObservableCollection<string> CityComboBox { get; set; }
        public ObservableCollection<string> CountryComboBox { get; set; }

        private string _selectedCity;
        public string SelectedCity
        {
            get => _selectedCity;
            set
            {
                if (_selectedCity != value)
                {
                    _selectedCity = value;
                    OnPropertyChanged(nameof(SelectedCity));
                }
            }
        }

        private string _selectedCountry;
        public string SelectedCountry
        {
            get => _selectedCountry;
            set
            {
                if (_selectedCountry != value)
                {
                    _selectedCountry = value;
                    OnPropertyChanged(nameof(SelectedCountry));
                }
            }
        }
        private string _comment;
        public string Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }
        public MyICommand CreateNewForumCommand {  get; set; }  
        public MyICommand ExitCommand {  get; set; }
        public GuestDTO guestDTO { get; set; }
        public int loggedInUserId { get; set; }
        public CreateForumVM(NavigationService navigationService,int loggedInUserId)
        {
            NavigationService = navigationService;
            accommodationService = new AccommodationService(Injector.Injector.CreateInstance<IAccommodationRepository>(),
               Injector.Injector.CreateInstance<IImageRepository>(),
               Injector.Injector.CreateInstance<ILocationRepository>(),
               Injector.Injector.CreateInstance<IOwnerRepository>());
            guestService = new GuestService(Injector.Injector.CreateInstance<IGuestRepository>());
            locationService = new LocationService(Injector.Injector.CreateInstance<ILocationRepository>());
            forumCommentService = new ForumCommentService(Injector.Injector.CreateInstance < IForumCommentRepository >());
            forumService=new ForumService(Injector.Injector.CreateInstance<IForumRepository>());
            CityComboBox = new ObservableCollection<string>();
            CountryComboBox = new ObservableCollection<string>();
            CurrentCitySource = new ObservableCollection<string>();
            CurrentCountrySource = new ObservableCollection<string>();
            // this.guestDTO = guestDTO;
           // guestDTO = guestService.UpdateGuest(loggedInUserId);
           this.loggedInUserId=loggedInUserId;
            LoadCountries();
            LoadCity();
            ExitCommand = new MyICommand(OnGoBack);
            CreateNewForumCommand = new MyICommand(OnAddForum);

        }
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
            if (string.IsNullOrWhiteSpace(input))
            {
                CurrentCountrySource = new ObservableCollection<string>(CountryComboBox);

            }
            else
            {
                var suggestions = locationService.GetAutocompleteCountry(input);
                CurrentCountrySource = new ObservableCollection<string>(suggestions);
            }
            OnPropertyChanged(nameof(CurrentCountrySource));

        }
        public void OnAddForum()
        {
            int locationId= locationService.GetLocationId(SelectedCity, SelectedCountry);
            //int guestId = guestDTO.Id;
            var guestDTO=guestService.GetByUserIdDTO(loggedInUserId);
            var forumDTO=forumService.AddNewForum(guestDTO.Id, locationId);
            forumCommentService.AddNewForumComment(loggedInUserId, forumDTO.Id,Comment);
            MessageBox.Show("Successfully created forum!");

        }
        private void OnGoBack()
        {
            NavigationService.GoBack();
        }
    }
}
