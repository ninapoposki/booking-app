using BookingApp.Domain.IRepositories;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.WPF.View.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Tourist
{
   
    public class ToursToRateVM
    {
        public ObservableCollection<TourDTO> FinishedTours { get; set; }
        public TourDTO SelectedTour { get; set; }   
        private TourStartDateService tourStartDateService;
        private TourService tourService;
        private TourReservationService tourReservationService;
        
        private int userId;
        public ToursToRateVM(int userId)
        {
            this.userId = userId;
            FinishedTours = new ObservableCollection<TourDTO>();
            SelectedTour=new TourDTO();
            tourService = new TourService(Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            tourReservationService = new TourReservationService(Injector.Injector.CreateInstance<ITourReservationRepository>(), Injector.Injector.CreateInstance<ITourGuestRepository>(),
              Injector.Injector.CreateInstance<IUserRepository>(), Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(),
              Injector.Injector.CreateInstance<ILanguageRepository>(),
              Injector.Injector.CreateInstance<ILocationRepository>());
            tourStartDateService = new TourStartDateService(Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            LoadFinishedTours();
        }

        private void LoadFinishedTours()
        {
            List<TourDTO> finishedTours = tourService.GetAll();
            foreach (TourDTO tourDTO in finishedTours) {
                
                var finishedDates= tourStartDateService.GetTourDates(tourDTO.Id).Where(t=> t.TourStatus==Domain.Model.TourStatus.FINISHED);
                foreach (TourStartDateDTO tourStart in finishedDates) 
                {
                    TourDTO tour=tourService.GetTour(tourStart.TourId);
                    tour.SelectedDateTime = tourStart;
                    FinishedTours.Add(tour);
                }
            }
        }

        public void RateTour()
        {
            if(SelectedTour==null )
            {
                MessageBox.Show("Niste selektovali turu");
            }
            if (tourReservationService.CheckIfReserved(SelectedTour.SelectedDateTime.Id))
            {
                TourGradeWindow tourGradeWindow = new TourGradeWindow(SelectedTour.SelectedDateTime.Id);
                tourGradeWindow.Show();
            }
        }
    }
}
