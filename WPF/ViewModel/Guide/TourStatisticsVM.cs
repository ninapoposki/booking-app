using BookingApp.DTO;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;


namespace BookingApp.WPF.ViewModel.Guide
{
    public class TourStatisticsVM : ViewModelBase
    {
        private int userId;
        private int maxNumberOfGuests = 0;
        private List<TourDTO> bestTours = new List<TourDTO>();
        public ObservableCollection<TourDTO> FinishedTours { get; set; }
        public ObservableCollection<TourDTO> BestTours { get; set; }
        public List<TourGuestDTO> Guests { get; set; }
        public List<string> YearComboBox { get; set; }
        public string SelectedYear { get; set; }
        private TourStartDateService tourStartDateService;
        private TourReservationService tourReservationService;
        public TourStatisticsVM(int userId)
        {
            this.userId = userId;
            FinishedTours = new ObservableCollection<TourDTO>();
            BestTours = new ObservableCollection<TourDTO>();
            tourStartDateService = new TourStartDateService();
            tourReservationService = new TourReservationService();
            Guests = new List<TourGuestDTO>();
            YearComboBox = new List<string>();
            SelectedYear = "";
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
            else
            {
                int year = Convert.ToInt32(SelectedYear);
                return tourStartDateService.GetByYear(year, userId);
            }
        }
        public void LoadBestTours()
        {
            BestTours.Clear();
            maxNumberOfGuests = 0;
            bestTours= GetToursBasedOnSelection();
            foreach (TourDTO tour in bestTours)
            {
                int guestsCount = tourReservationService.GetFinishedToursGuests(tour.SelectedDateTime.Id).Count();
                AddTourIfEligible(tour, guestsCount);
            }
            UpdateTouristNumbersForBestTours();
        }
        private void AddTourIfEligible(TourDTO tour, int guestsCount)
        {
            if (guestsCount > maxNumberOfGuests)
            {
                maxNumberOfGuests = guestsCount;
                BestTours.Clear();
                BestTours.Add(tour);
            }
            else if (guestsCount == maxNumberOfGuests) { BestTours.Add(tour); }
        }
        public void UpdateTouristNumbersForBestTours()
        {
            foreach (TourDTO bestTour in BestTours) { bestTour.NumberOfTourists = maxNumberOfGuests; }
        }
        private void LoadFinishedTours()
        {
            List<TourDTO> finishedTours = tourStartDateService.GetAllFinishedTours(userId);
            foreach (TourDTO tour in finishedTours) { FinishedTours.Add(tour); }
        }
        public void LoadStatistics(TourDTO tour)
        {
            Guests = tourReservationService.GetFinishedToursGuests(tour.SelectedDateTime.Id);
            YoungVisitorsCount = Guests.FindAll(g => g.Age <= 18).Count();
            AdultVisitorsCount = Guests.FindAll(g => g.Age > 18 && g.Age < 50).Count();
            SeniorVisitorsCount = Guests.FindAll(g => g.Age > 50).Count();
        }
        public void YearChanged()
        {
            BestTours.Clear();
            LoadBestTours();
        }
        private TourDTO selectedTour;
        public TourDTO SelectedTour
        {
            get { return selectedTour; }
            set
            {
                if (selectedTour != value)
                {
                    selectedTour = value;
                    OnPropertyChanged("SelectedTour");
                }
            }
        }
        private int youngVisitorsCount;
        public int YoungVisitorsCount
        {
            get { return youngVisitorsCount; }
            set
            {
                if (youngVisitorsCount != value)
                {
                    youngVisitorsCount = value;
                    OnPropertyChanged("YoungVisitorsCount");
                }
            }
        }
        private int seniorVisitorsCount;
        public int SeniorVisitorsCount
        {
            get { return seniorVisitorsCount; }
            set
            {
                if (seniorVisitorsCount != value)
                {
                    seniorVisitorsCount = value;
                    OnPropertyChanged("SeniorVisitorsCount");
                }
            }
        }
        private int adultVisitorsCount;
        public int AdultVisitorsCount
        {
            get { return adultVisitorsCount; }
            set
            {
                if (adultVisitorsCount != value)
                {
                    adultVisitorsCount = value;
                    OnPropertyChanged("AdultVisitorsCount");
                }
            }
        }
    }
}