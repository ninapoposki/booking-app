using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class ActiveToursWindowVM:ViewModelBase
    {
    
        private int userId;
        private TourStartDateService tourStartDateService;
        private CheckPointService checkPointService;
        private TourReservationService tourReservationService;
        private TourService tourService;
        public ObservableCollection<TourDTO> ActiveTours { get; set; }

        public ActiveToursWindowVM(int userId) { 
        
            this.userId = userId;
            tourStartDateService = new TourStartDateService();
            ActiveTours = new ObservableCollection<TourDTO>();
            tourReservationService = new TourReservationService();
            checkPointService = new CheckPointService();
            tourService = new TourService();
            LoadActiveTour();
        }
        private void LoadActiveTour()
        {
            List<TourDTO> activeTours = tourService.GetAll(); 
            foreach (TourDTO tourDTO in activeTours)
            {
                IEnumerable<TourStartDateDTO> activeDates = tourStartDateService.GetTourDates(tourDTO.Id).Where(t => t.TourStatus == Domain.Model.TourStatus.ACTIVE);

                foreach (TourStartDateDTO tourStart in activeDates)
                {
                    TourDTO tour = tourService.GetTour(tourStart.TourId);
                    if (tour != null)
                    {
                        tour.SelectedDateTime = tourStart;
                        foreach (TourReservationDTO tr in tourReservationService.GetByUserId(userId))
                        {
                            int checkpointId = tour.SelectedDateTime.CurrentCheckPointId;
                            var checkpoint = checkPointService.GetById(checkpointId);
                            if (checkpoint != null)
                            {
                                tour.CheckPointName = checkpoint.Name;
                            }
                            ActiveTours.Clear();
                            ActiveTours.Add(tour);
                        }
                      
                    }
                }
            }
        }
   
      
    }
}
