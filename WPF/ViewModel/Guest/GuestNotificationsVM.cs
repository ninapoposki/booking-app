using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class GuestNotificationsVM:ViewModelBase
    {
        private readonly ReservationRequestService reservationRequestService;
        private readonly ImageService imageService;
        private readonly AccommodationReservationService accommodationReservationService;
        public NavigationService navigationService { get; set; }

        public ObservableCollection<ReservationRequestDTO> AllReservationRequests { get; set; }
        public ObservableCollection<ImageDTO> Images { get; set; }
        public int currentIndex = 0;


        private ReservationRequestDTO _selectedReservationRequest;
        public ReservationRequestDTO SelectedReservationRequest
        {
            get => _selectedReservationRequest;
            set
            {
                if (_selectedReservationRequest != value)
                {
                    _selectedReservationRequest = value;
                    OnPropertyChanged(nameof(SelectedReservationRequest));
                }
            }
        }
      
        public GuestNotificationsVM(NavigationService navigationService)
        {
            AllReservationRequests = new ObservableCollection<ReservationRequestDTO>();
            SelectedReservationRequest = new ReservationRequestDTO();
            reservationRequestService = new ReservationRequestService(Injector.Injector.CreateInstance<IReservationRequestRepository>(),
                Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
                Injector.Injector.CreateInstance<IGuestRepository>(),
                Injector.Injector.CreateInstance<IUserRepository>(),
                Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>());
            accommodationReservationService = new AccommodationReservationService(Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
                Injector.Injector.CreateInstance<IGuestRepository>(),
                Injector.Injector.CreateInstance<IUserRepository>(),
                Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>());
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            this.navigationService = navigationService;
            Images = new ObservableCollection<ImageDTO>();
            Update();
        }

        public void Update()
        {
            AllReservationRequests.Clear();
            var allImages = imageService.GetImagesForEntityType(EntityType.ACCOMMODATION);
            foreach (ReservationRequestDTO reservationRequestDTO in reservationRequestService.GetAll())
            {
                var requestDTO = reservationRequestService.GetOneRequest(reservationRequestDTO);
                var reservationDTO = accommodationReservationService.GetById(requestDTO.ReservationId);
                var matchingImages = new ObservableCollection<ImageDTO>(imageService. GetImagesByAccommodation(reservationDTO.AccommodationId, allImages));

                if (matchingImages.Count == 0)
                {
                    matchingImages.Add(new ImageDTO { Path = @"\Resources\Images\placeholder_accommodation.jpg" });
                }

                requestDTO.Images = matchingImages;
                AllReservationRequests.Add(requestDTO);
            }
        }
    }
}
