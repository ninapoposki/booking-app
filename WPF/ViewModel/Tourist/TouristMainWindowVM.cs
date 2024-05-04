﻿using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Services;
using BookingApp.Utilities;
using BookingApp.WPF.View.Guide;
using BookingApp.WPF.View.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class TouristMainWindowVM : ViewModelBase
    {

        private readonly TourService tourService;
        private readonly LocationService locationService;
        private readonly LanguageService languageService;
        private readonly TourStartDateService tourStartDateService;
        private readonly ImageService imageService;
        private string loggedInUserUsername;
        public int loggedInUserId;
        public ObservableCollection<TourDTO> AllTours { get; set; }
        public ObservableCollection<ImageDTO> Images { get; set; }
        public List<LanguageDTO> Languages { get; set; }
        public UserService userService;
        public TourDTO SelectedTour { get; set; }
        public List<LocationDTO> LocationComboBox { get; set; }
        public LocationDTO SelectedCity { get; set; }
        public LanguageDTO SelectedLanguage { get; set; } 
        public UserDTO userDTO { get; set; }

        public NavigationService NavigationService { get; set; }
        public TouristMainWindowVM(string username)
        {
            userDTO = new UserDTO();
            tourService = new TourService(Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            locationService = new LocationService(Injector.Injector.CreateInstance<ILocationRepository>());
            languageService = new LanguageService(Injector.Injector.CreateInstance<ILanguageRepository>());
            userService = new UserService(Injector.Injector.CreateInstance<IUserRepository>());
            SelectedLanguage = new LanguageDTO();
            SelectedTour = new TourDTO();
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            loggedInUserUsername=username;
            loggedInUserId=userService.GetByUsername(loggedInUserUsername).Id;
            tourStartDateService = new TourStartDateService(Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            AllTours = new ObservableCollection<TourDTO>();
            Images = new ObservableCollection<ImageDTO>();
            Languages = new List<LanguageDTO>();
            SelectedCity = new LocationDTO();
            LocationComboBox = new List<LocationDTO>();
          
         
            Update();
        }

        public void Update()
        {
            GetTours();
            GetLanguages();
            locationService.GetAll(LocationComboBox);
        }
        private void LoadCountry()
        {
            Country = locationService.GetCountry(SelectedCity);
        }
        public void CityChanged()
        {
            LoadCountry();
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
        public void GetTours()
        {
            AllTours.Clear();
            var allImages = imageService.GetAll().Where(img => img.EntityType == EntityType.TOUR).ToList();
            var inactiveTourDates = tourStartDateService.GetAllInactiveTourDates();
            var inactiveTourIds = inactiveTourDates.Select(tsd => tsd.TourId).Distinct();
            foreach (var tourId in inactiveTourIds)
            {
                var tour = tourService.GetTour(tourId);
                if (tour != null)
                {
                    var matchingImages = new ObservableCollection<ImageDTO>(allImages.Where(img => img.EntityId == tour.Id).ToList());
                    var firstImage = matchingImages.FirstOrDefault();
                    LocationDTO location = locationService.GetByIdDTO(tour.LocationId);
                    LanguageDTO language = languageService.GetByIdDTO(tour.LanguageId);
                    var dateTimes = new ObservableCollection<TourStartDateDTO>(tourStartDateService.GetTourDates(tour.Id));
                    var tourDto = new TourDTO(tour.ToTour(), location.ToLocation(), language.ToLanguage())
                    {
                        DateTimes = dateTimes,
                        Images = new ObservableCollection<ImageDTO>()
                    };

                    if (firstImage != null)
                    {
                        tourDto.Images.Add(firstImage);  
                    }

                    AllTours.Add(tourDto);
                }
            }
        } 
    
        public void GetLanguages()
        {
            Languages.Clear();
            languageService.GetAll(Languages);
        }

      
        public void SearchTour()
        {
            Update();
            string selectedLanguage = GetSelectedLanguage();
            List<TourDTO> filteredTours = FilterTours(PeopleCount, Duration, selectedLanguage);
            AllTours.Clear();
            foreach (var tour in filteredTours)
            {
                AllTours.Add(tour);
            }
        }
       

        public List<TourDTO> FilterTours(int peopleCount, double duration, string selectedLanguage)
        {
            return AllTours.Where(tour =>
                IsMatchCity(tour) &&
                IsMatchCountry(tour) &&
                IsMatchDuration(tour, duration) &&
                IsMatchLanguage(tour, selectedLanguage) &&
                IsMatchPeopleCount(tour, peopleCount)
            ).ToList();
         
        }

        private bool IsMatchCity(TourDTO tour)
        {
            return string.IsNullOrWhiteSpace(CityFilter) || tour.Location.City.Equals(CityFilter, StringComparison.OrdinalIgnoreCase);
        }

        private bool IsMatchCountry(TourDTO tour)
        {
            return string.IsNullOrWhiteSpace(CountryFilter) || tour.Location.Country.Equals(CountryFilter, StringComparison.OrdinalIgnoreCase);
        }

        private bool IsMatchDuration(TourDTO tour, double duration)
        {
            return duration > 0 ? Math.Abs(tour.Duration - duration) < 0.01 : true;
        }

        private bool IsMatchLanguage(TourDTO tour, string selectedLanguage)
        {
            return string.IsNullOrEmpty(selectedLanguage) || tour.Language.Name.Equals(selectedLanguage, StringComparison.OrdinalIgnoreCase);
        }


        private bool IsMatchPeopleCount(TourDTO tour, int peopleCount)
        {
            return peopleCount <= 0 || tour.Capacity >= peopleCount;
        }

        public void BookTour()
        {
            if (SelectedTour == null)
            {
                ShowMessage("Molimo Vas da odaberete turu koju želite da rezervišete.");
                return;
            }

            if (SelectedTour.Capacity > 0)
            {
                StartTourReservation(SelectedTour);
                return;
            }

            ShowNoSpaceAvailableMessage();
            ShowAvailableToursOnSameLocation();
        }

        private void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
        
        private void StartTourReservation(TourDTO tour)
        {
            TourReservationWindow tourReservationWindow = new TourReservationWindow(tour,loggedInUserUsername);
            tourReservationWindow.Show();
        }

        private void ShowNoSpaceAvailableMessage()
        {
            ShowMessage("Nažalost, na ovoj turi nema više slobodnih mesta, ali nudimo vam ostale!");
        }

        public void ShowAvailableToursOnSameLocation()
        {
            var availableTours = GetAvailableToursOnSameLocation();

            if (!availableTours.Any())
            {
                ShowMessage("Nema dostupnih tura na ovoj lokaciji.");
                return;
            }

            ShowAvailableTourWindow(availableTours);
        }

        public List<TourDTO> GetAvailableToursOnSameLocation()
        {
            return AllTours
                .Where(tour => IsTourAvailableOnSameLocation(tour))
                .ToList();
        }

        public bool IsTourAvailableOnSameLocation(TourDTO tour)
        {
            return tour.LocationId == SelectedTour?.LocationId && tour.Capacity > 0;
        }


        private void ShowAvailableTourWindow(List<TourDTO> availableTours)
        {
            AvailableTourWindow availableTourWindow = new AvailableTourWindow(availableTours,loggedInUserUsername);
            availableTourWindow.Show();
        }



        public string GetSelectedLanguage()
        {
            if (SelectedLanguage is LanguageDTO selectedLanguage)
            {
                return selectedLanguage.Name;
            }
            return string.Empty;
        }

        private int peopleCount;
        public int PeopleCount
        {
            get => peopleCount;
            set
            {
                peopleCount = value;
                OnPropertyChanged(nameof(PeopleCount));
            }
        }

        private double duration;
        public double Duration
        {
            get
            {
                return duration;
            }
            set
            {
                if (value != duration)
                {
                    duration = value;
                    OnPropertyChanged(nameof(Duration));
                }

            }
        }



        private string cityFilter;
        public string CityFilter
        {
            get => cityFilter;
            set
            {
                cityFilter = value;
                OnPropertyChanged(nameof(CityFilter));
            }
        }

        private string countryFilter;
        public string CountryFilter
        {
            get => countryFilter;
            set
            {
                countryFilter = value;
                OnPropertyChanged(nameof(CountryFilter));
            }
        }

        public void FinishedTourClick()
        {
            ToursToRateWindow toursToRate = new ToursToRateWindow(loggedInUserId);
            toursToRate.Show();
        }
        public void ActiveTourClick()
        {
            var activeTours = new ActiveToursWindow(loggedInUserId);
           activeTours.Show();
           
        }
        public void NotificationsClick()
        {
            NotificationsWindow notifications = new NotificationsWindow();
            notifications.Show();
        }
    }
}
