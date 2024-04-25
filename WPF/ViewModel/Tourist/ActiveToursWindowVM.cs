using BookingApp.Domain.IRepositories;
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
        private ImageService imageService;
        private TourReservationService tourReservationService;
        private TourService tourService;
        public ObservableCollection<TourDTO> ActiveTours { get; set; }

        public ActiveToursWindowVM(int userId) { 
        
            this.userId = userId;
            tourStartDateService = new TourStartDateService(Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            tourReservationService = new TourReservationService(Injector.Injector.CreateInstance<ITourReservationRepository>(), Injector.Injector.CreateInstance<ITourGuestRepository>(),
                Injector.Injector.CreateInstance<IUserRepository>(), Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(),
                Injector.Injector.CreateInstance<ILanguageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>());
            ActiveTours = new ObservableCollection<TourDTO>();
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            checkPointService = new CheckPointService(Injector.Injector.CreateInstance<ICheckPointRepository>());
            tourService = new TourService(Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            LoadActiveTour();
        }
        private void LoadActiveTour()
        {
            List<TourDTO> activeTours = tourService.GetAll();
            var allImages = imageService.GetAll().Where(img => img.EntityType == EntityType.TOUR).ToList();
            foreach (TourDTO tourDTO in activeTours)
            {
                IEnumerable<TourStartDateDTO> activeDates = tourStartDateService.GetTourDates(tourDTO.Id).Where(t => t.TourStatus == Domain.Model.TourStatus.ACTIVE);

                foreach (TourStartDateDTO tourStart in activeDates)
                {
                    TourDTO tour = tourService.GetTour(tourStart.TourId);
                    if (tour != null)

                    {
                        var matchingImages = new ObservableCollection<ImageDTO>(allImages.Where(img => img.EntityId == tour.Id).ToList());
                        tour.Images = matchingImages;
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
