using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

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
        public ObservableCollection<CheckPointDTO> CheckPoints { get; set; }
        public NavigationService NavigationService { get; set; }
        public MyICommand MoveNextImageCommand { get; set; }
        public MyICommand MovePreviousImageCommand { get; set; }
        private int currentIndex = 0;
        private ObservableCollection<ImageDTO> images;
        private ImageDTO _currentImage;
        public ImageDTO CurrentImage
        {
            get => _currentImage;
            set
            {
                if (_currentImage != value)
                {
                    _currentImage = value;
                    OnPropertyChanged(nameof(CurrentImage));

                }
            }
        }

        public ActiveToursWindowVM(NavigationService navigationService,int userId) { 
        
            this.userId = userId;
            NavigationService = navigationService;
            tourStartDateService = new TourStartDateService(Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            tourReservationService = new TourReservationService(Injector.Injector.CreateInstance<ITourReservationRepository>(), Injector.Injector.CreateInstance<ITourGuestRepository>(),
                Injector.Injector.CreateInstance<IUserRepository>(), Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(),
                Injector.Injector.CreateInstance<ILanguageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>());
            ActiveTours = new ObservableCollection<TourDTO>();
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            checkPointService = new CheckPointService(Injector.Injector.CreateInstance<ICheckPointRepository>());
            tourService = new TourService(Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            CheckPoints= new ObservableCollection<CheckPointDTO>();
           
            LoadActiveTour();
        }
        private void MoveNext()
        {
            if (currentIndex < images.Count - 1)
            {
                currentIndex++;
                CurrentImage = images[currentIndex];
                OnPropertyChanged(nameof(CurrentImage));

            }
        }
        private void MovePrevious()
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                CurrentImage = images[currentIndex];
                OnPropertyChanged(nameof(CurrentImage));
            }
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
                            LoadCheckPointsForTour(tour.Id);
                        }
                      
                    }
                }
            }
        }

        private void LoadCheckPointsForTour(int tourId)
        {
            var checkpoints = checkPointService.GetByTourID(tourId);
            CheckPoints.Clear();
            foreach (var checkpoint in checkpoints)
            {
              
                CheckPoints.Add(checkpoint);
            }
        }

    }
}
