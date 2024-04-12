using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Services;
using BookingApp.WPF.View.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class LiveTourVM:ViewModelBase
    {
        public ObservableCollection<TourDTO> Tours { get; set; }

        private TourService tourService;
        private TourStartDateService tourStartDateService;
        private LocationService locationService;
        private LanguageService languageService;
        private TourReservationService tourReservationService;
        public TourDTO SelectedTour { get; set; }

        public LiveTourVM()
        {
           
           Tours = new ObservableCollection<TourDTO>();
           SelectedTour = new TourDTO();
           
           tourService = new TourService();
           tourStartDateService = new TourStartDateService();
           tourReservationService = new TourReservationService();
           languageService= new LanguageService();
           locationService= new LocationService();

            LoadTodaysTours();
        }

        private void LoadTodaysTours()
        {
            foreach(TourDTO tour in tourStartDateService.GetTours())
            {
                if (!AlreadyExist(tour.Id))
                {
                    tour.DateTimes = new ObservableCollection<TourStartDateDTO>(tourStartDateService.UpdateDate(tour.Id));
                    Tours.Add(tour);
                }
            }        
           
        }
        private bool AlreadyExist(int newTourId)
        {
            foreach (var tour in Tours)
            {
                if (tour.Id == newTourId)
                {
                    return true;
                }
            }
            return false;
        }
        private TourDTO GetTour(TourStartDate tourDate)
        { 
            TourDTO tour=tourService.GetTour(tourDate.TourId);
            return tour;
        }

        public void StartTourClick()
        {
            if (!tourReservationService.DoReservationExists(SelectedTour.SelectedDateTime.Id))
            {
                MessageBox.Show("There are no reservations for selected tour and date");
                return;
            }
            TourCheckPoints tourCheckPoints = new TourCheckPoints(SelectedTour.SelectedDateTime);
            tourCheckPoints.Show();

        }

        private DateTime todayDate = DateTime.Now.Date;

        public DateTime TodayDate
        {
            get { return todayDate; }
            set
            {
                if (todayDate != value)
                {
                    todayDate = value;
                    OnPropertyChanged("TodayDate");
                }
            }
        }

    }
}
