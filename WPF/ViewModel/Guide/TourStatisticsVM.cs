using BookingApp.DTO;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace BookingApp.WPF.ViewModel.Guide
{
    public class TourStatisticsVM:ViewModelBase
    {
        private int userId;
        public ObservableCollection<TourDTO> FinishedTours { get; set; }
        public List<TourGuestDTO> Guests { get; set; }

        private TourStartDateService tourStartDateService;
        private TourReservationService tourReservationService;
        public TourStatisticsVM(int userId) 
        {
            this.userId = userId;
            FinishedTours = new ObservableCollection<TourDTO>();
            tourStartDateService = new TourStartDateService();  
            tourReservationService = new TourReservationService();
            Guests= new List<TourGuestDTO>();
            LoadFinishedTours();
        }
        private void LoadFinishedTours()
        {
            List<TourDTO> finishedTours = tourStartDateService.GetAllFinishedTours(userId);
            foreach(TourDTO tour in finishedTours)
            {
                FinishedTours.Add(tour);
            }
        }
        public void LoadStatistics(TourDTO tour)
        {
            Guests = tourReservationService.GetFinishedToursGuests(tour.SelectedDateTime.Id);
            YoungVisitorsCount = Guests.FindAll(g => g.Age <=18).Count();
            AdultVisitorsCount=Guests.FindAll(g=>g.Age > 18 && g.Age<50).Count();
            SeniorVisitorsCount=Guests.FindAll(g=>g.Age>50).Count();
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
