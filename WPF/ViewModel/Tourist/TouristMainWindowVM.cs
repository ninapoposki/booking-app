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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        private readonly TourGradeService tourGradeService;
        public int loggedInUserId;
        public ObservableCollection<TourDTO> AllTours { get; set; }
        public ObservableCollection<ImageDTO> Images { get; set; }
        public List<LanguageDTO> Languages { get; set; }
        public UserService userService;
        public TourDTO SelectedTour { get; set; }
        public LanguageDTO SelectedLanguage { get; set; }   


        public TouristMainWindowVM(string username)
        {

            tourService = new TourService();
            locationService = new LocationService();
            languageService = new LanguageService();
            userService = new UserService();
            tourGradeService = new TourGradeService();
            SelectedLanguage = new LanguageDTO();
            SelectedTour = new TourDTO();
            imageService = new ImageService();
            loggedInUserUsername=username;
            loggedInUserId=userService.GetByUsername(loggedInUserUsername).Id;
            tourStartDateService = new TourStartDateService();
            AllTours = new ObservableCollection<TourDTO>();
            Images = new ObservableCollection<ImageDTO>();
            Languages = new List<LanguageDTO>();
            Update();
        }

        public void Update()
        {
            GetTours();
            GetLanguages();
        }


        public void GetTours()
        {
            AllTours.Clear();
           var allImages = imageService.GetAll().Where(img => img.EntityType == EntityType.TOUR).ToList();
            foreach (var tour in tourService.GetAll())
            {
                var matchingImages = new ObservableCollection<ImageDTO>(allImages.Where(img => img.EntityId == tour.Id).ToList());
                LocationDTO location = locationService.GetByIdDTO(tour.LocationId);
                LanguageDTO language = languageService.GetByIdDTO(tour.LanguageId);
                var dateTimes = new ObservableCollection<TourStartDateDTO>(tourStartDateService.GetTourDates(tour.Id));
                AllTours.Add(new TourDTO(tour.ToTour(), location.ToLocation(), language.ToLanguage()) { Images = matchingImages,DateTimes=dateTimes });
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

        /* public void TourGradeDataGrid(TourDTO selectedTour)
         {
             int tourId = selectedTour.Id;

             if (tourGradeService.IsTourGraded(tourId))
             {
                 MessageBox.Show("Tour is already graded.");
             }
             else
             {
                 IsTourFinished(selectedTour);
             }
         }
         */
        /* private void IsTourFinished(TourDTO selectedTour)
         {
             var tourStart = tourStartDateService.GetById(selectedTour.Id);
             if (tourStart != null && tourStart.TourStatus == TourStatus.FINISHED)
             {
                 OpenTourGradeWindow(selectedTour);
             }
             else
             {
                 MessageBox.Show("Tura još uvek nije završena.");
             }

         }*/

        /* private void OpenTourGradeWindow(TourDTO selectedTour)
         {
             TourGradeWindow tourGradeWindow = new TourGradeWindow(selectedTour);
             tourGradeWindow.Show();
         }*/
        /*   public void RateTour()
          {

               if (SelectedTour == null)
               {
                   MessageBox.Show("Molimo Vas da odaberete turu koju želite da rezervišete.");
                   return;
               }
               int tourId = tourService.GetById(SelectedTour.ToTour());
               if (tourGradeService.IsTourGraded(tourId, loggedInUserId))
               {
                   MessageBox.Show("Već ste ocenili ovu turu.");
               }
               else
               {
                   IsTourFinished(SelectedTour);
               }
           }*/


        public void RateTour()
        {

            if (SelectedTour == null)
            {
                MessageBox.Show("Molimo Vas da odaberete turu koju želite da rezervišete.");
                return;
            }

            var tourStartDate = GetTourStartDateByTourId(SelectedTour.Id);
            if (tourStartDate == null || tourStartDate.TourStatus != TourStatus.FINISHED)
            {
                MessageBox.Show("Tura još uvek nije završena.");
                return;
            }
            if (tourGradeService.IsTourGraded(tourStartDate.Id,loggedInUserId))
            {
                MessageBox.Show("Već ste ocenili ovu turu.");
                return;
            }
            StartTourGrade(SelectedTour);

        }
       
        private TourStartDateDTO GetTourStartDateByTourId(int tourId)
        {
            return tourStartDateService.GetTourDates(tourId).FirstOrDefault(t => t.TourStatus == TourStatus.FINISHED);
        }

        private void StartTourGrade(TourDTO tour)
        {
            TourGradeWindow tourGradeWindow = new TourGradeWindow(SelectedTour);
            tourGradeWindow.Show();
        }
    }
}
