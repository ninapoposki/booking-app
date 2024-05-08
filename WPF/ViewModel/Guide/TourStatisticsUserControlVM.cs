using BookingApp.Domain.IRepositories;
using BookingApp.DTO;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel.Guide
{
    class TourStatisticsUserControlVM:ViewModelBase
    {
        private int userId;
        private int maxNumberOfGuests = 0;
        private List<TourDTO> bestTours = new List<TourDTO>();
        public ObservableCollection<TourStatisticsDTO> FinishedTours { get; set; }
        public ObservableCollection<TourStatisticsDTO> BestTours { get; set; }
        public List<TourGuestDTO> Guests { get; set; }
        public List<string> YearComboBox { get; set; }
        private TourStartDateService tourStartDateService;
        private TourReservationService tourReservationService;
        private ImageService imageService;
        public TourStatisticsUserControlVM(int userId)
        {
            this.userId = userId;
            FinishedTours = new ObservableCollection<TourStatisticsDTO>();
            BestTours = new ObservableCollection<TourStatisticsDTO>();
            tourStartDateService = new TourStartDateService(Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            tourReservationService = new TourReservationService(Injector.Injector.CreateInstance<ITourReservationRepository>(), Injector.Injector.CreateInstance<ITourGuestRepository>(),
                Injector.Injector.CreateInstance<IUserRepository>(), Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(),
                Injector.Injector.CreateInstance<ILanguageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>());
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            Guests = new List<TourGuestDTO>();
            YearComboBox = new List<string>();
            LoadFinishedTours();
            LoadYears();
        }
        private void LoadYears()
        {
            YearComboBox.Add("All time");
            for (int year = 2015; year <= DateTime.Now.Year; year++) { YearComboBox.Add(year.ToString()); }
        }
        private List<TourDTO> GetToursBasedOnSelection()
        {
            if (SelectedYear == "All time") { return tourStartDateService.GetAllFinishedTours(userId); }
            else {
                int year = Convert.ToInt32(SelectedYear);
                return tourStartDateService.GetByYear(year, userId);
            }
        }
        public void LoadBestTours()
        {
            BestTours.Clear();
            maxNumberOfGuests = 0;
            bestTours = GetToursBasedOnSelection();
            foreach (TourDTO tour in bestTours)
            {
                int guestsCount = tourReservationService.GetFinishedToursGuests(tour.SelectedDateTime.Id).Count();
                TourStatisticsDTO tourStatisticsDTO = new TourStatisticsDTO(tour);
                AddTourIfEligible(tourStatisticsDTO, guestsCount);
            }
            UpdateTouristNumbersForBestTours();
        }
        private void AddTourIfEligible(TourStatisticsDTO tour, int guestsCount)
        {
            if (guestsCount > maxNumberOfGuests)
            {
                maxNumberOfGuests = guestsCount;
                BestTours.Clear();
                BestTours.Add(tour);
                SetImage(tour);
            }
            else if (guestsCount == maxNumberOfGuests) { BestTours.Add(tour); SetImage(tour); }
        }
        private void SetImage(TourStatisticsDTO tour)
        {
            var path = imageService.GetFirstPath(tour.Id, "TOUR");
            tour.Path = path ?? "..\\..\\..\\Resources\\Images\\placeholderGuide.png";
        }
        public void UpdateTouristNumbersForBestTours()
        {
            foreach(TourStatisticsDTO bestTour in BestTours) 
            {
                bestTour.TotalNumberOfTourists = maxNumberOfGuests;
                LoadStatistics(bestTour);
            }
        }
        private void LoadFinishedTours()
        {
            List<TourDTO> finishedTours = tourStartDateService.GetAllFinishedTours(userId);
            foreach (TourDTO tour in finishedTours) { 
                TourStatisticsDTO finishedTour= new TourStatisticsDTO(tour);
                FinishedTours.Add(finishedTour);  
                SetImage(finishedTour); }
        }
        public void LoadStatistics(TourStatisticsDTO tour)
        {
            Guests = tourReservationService.GetFinishedToursGuests(tour.SelectedDateTime.Id);
            tour.YoungVisitorsCount = Guests.FindAll(g => g.Age <= 18).Count();
            tour.AdultVisitorsCount = Guests.FindAll(g => g.Age > 18 && g.Age < 50).Count();
            tour.SeniorVisitorsCount = Guests.FindAll(g => g.Age > 50).Count();
        }
        public void YearChanged()
        {
            BestTours.Clear();
            LoadBestTours();
        }
        private TourStatisticsDTO selectedTour;
        public TourStatisticsDTO SelectedTour
        {
            get { return selectedTour; }
            set
            {
                if (selectedTour != value)
                {
                    selectedTour = value;
                    OnPropertyChanged("SelectedTour");
                    LoadStatistics(value);
                }
            }
        }
        private string selectedYear;
        public string SelectedYear
        {
            get { return selectedYear; }
            set
            {
                if (selectedYear != value)
                {
                    selectedYear = value;
                    OnPropertyChanged("SelectedYear");
                    LoadBestTours();
                }
            }
        }
    }
}
