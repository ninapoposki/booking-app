using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using BookingApp.WPF.View.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Tourist
{
   
    public class ToursToRateVM
    {
        public ObservableCollection<TourDTO> FinishedTours { get; set; }
        public TourDTO SelectedTour { get; set; }   
        private TourStartDateService tourStartDateService;
        private TourService tourService;
        private TourReservationService tourReservationService;
        private ImageService imageService;
        private TourGradeService tourGradeService;
        public MyICommand<TourDTO> RateTourCommand { get; set; }
        private int userId;
        public NavigationService NavigationService { get; set; }
        public ToursToRateVM(NavigationService navigationService,int userId)
        {
            this.userId = userId;
            NavigationService = navigationService;
            FinishedTours = new ObservableCollection<TourDTO>();
            SelectedTour=new TourDTO();

            tourService = new TourService(Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            tourReservationService = new TourReservationService(Injector.Injector.CreateInstance<ITourReservationRepository>(), Injector.Injector.CreateInstance<ITourGuestRepository>(),
              Injector.Injector.CreateInstance<IUserRepository>(), Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(),
              Injector.Injector.CreateInstance<ILanguageRepository>(),
              Injector.Injector.CreateInstance<ILocationRepository>());
            tourGradeService = new TourGradeService(Injector.Injector.CreateInstance<ICheckPointRepository>(), Injector.Injector.CreateInstance<ITourGradeRepository>(),
              Injector.Injector.CreateInstance<ITourReservationRepository>(), Injector.Injector.CreateInstance<ITourGuestRepository>(),
              Injector.Injector.CreateInstance<IUserRepository>(), Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(),
              Injector.Injector.CreateInstance<ILanguageRepository>(),
              Injector.Injector.CreateInstance<ILocationRepository>());
         
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            tourStartDateService = new TourStartDateService(Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            RateTourCommand = new MyICommand<TourDTO>(RateTour);
            LoadFinishedTours();
        }

        private void LoadFinishedTours()
        {
            List<TourDTO> finishedTours = tourService.GetAll();
            var allImages = imageService.GetAll().Where(img => img.EntityType == EntityType.TOUR).ToList();
            
            foreach (TourDTO tourDTO in finishedTours) {
                
                var finishedDates= tourStartDateService.GetTourDates(tourDTO.Id).Where(t=> t.TourStatus==Domain.Model.TourStatus.FINISHED);
                foreach (TourStartDateDTO tourStart in finishedDates) 
                {
                    TourDTO tour=tourService.GetTour(tourStart.TourId);
                    tour.SelectedDateTime = tourStart;
                    var matchingImages = new ObservableCollection<ImageDTO>(allImages.Where(img => img.EntityId == tour.Id).ToList());
                    var firstImage = matchingImages.FirstOrDefault();
                    tour.Images = new ObservableCollection<ImageDTO>();
                    tour.Images.Add(firstImage);
                    FinishedTours.Add(tour);
                }
            }
        }
        public void RateTour(TourDTO tour)
        {
            if (tour == null)
            {
                MessageBox.Show("Niste selektovali turu");
                return;
            }
            if (tourGradeService.IsTourRated(tour.SelectedDateTime.Id))
            {
                MessageBox.Show("Ova tura je već ocenjena.");
                return;
            }
            TourGradeWindow tourGradeWindow = new TourGradeWindow(tour.SelectedDateTime.Id);
            tourGradeWindow.Show();
        }
    }
}
