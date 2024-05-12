using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using BookingApp.WPF.View.Guest;
using BookingApp.WPF.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class OwnersRatingsVM:ViewModelBase
    {
        public NavigationService navigationService { get; set; }
        private GuestGradeService guestGradeService;
        private GuestService guestService;
        private AccommodationGradeService accommodationGradeService;
        private AccommodationReservationService accommodationReservationService;
        public ObservableCollection<GuestGradeDTO> AllOwnersRatings { get; set; }
        public GuestGradeDTO SelectedOwnersGrade { get; set; }

        private int currentUserId;
        public int CurrentUserId
        {
            get { return currentUserId; }
            set
            {
                if (currentUserId != value)
                {
                    currentUserId = value;
                    OnPropertyChanged(nameof(CurrentUserId));
                }
            }
        }
        public int guestUserId;
        public MyICommand<GuestGradeDTO> DetailsCommand { get; set; }
        public OwnersRatingsVM(NavigationService navigationService,int currentUserId)
        {
            this.navigationService = navigationService;
            guestGradeService = new GuestGradeService(Injector.Injector.CreateInstance<IGuestGradeRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>());
            guestService = new GuestService(Injector.Injector.CreateInstance<IGuestRepository>());
            accommodationGradeService = new AccommodationGradeService(Injector.Injector.CreateInstance<IAccommodationGradeRepository>(),
                Injector.Injector.CreateInstance<IUserRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>(),
                Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>());
            accommodationReservationService = new AccommodationReservationService(Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
                           Injector.Injector.CreateInstance<IGuestRepository>(),
                           Injector.Injector.CreateInstance<IUserRepository>(),
                           Injector.Injector.CreateInstance<IAccommodationRepository>(),
                           Injector.Injector.CreateInstance<IImageRepository>(),
                           Injector.Injector.CreateInstance<ILocationRepository>(),
                           Injector.Injector.CreateInstance<IOwnerRepository>());
            AllOwnersRatings = new ObservableCollection<GuestGradeDTO>();
            SelectedOwnersGrade=new GuestGradeDTO();
            DetailsCommand = new MyICommand<GuestGradeDTO>(OnOpenDetails);
            guestUserId=guestService.GetByUserIdDTO(currentUserId).Id;
            Update();
        }
        public void Update()
        {
            AllOwnersRatings.Clear();
            foreach (GuestGradeDTO guestGradeDTO in guestGradeService.GetAll())
            {
                if (guestUserId == guestGradeDTO.GuestId && accommodationGradeService.IsReservationGraded(guestGradeDTO.ReservationId))
                {
                    var updatedDTO = guestGradeDTO;
                    var reservationDTO= new AccommodationReservationDTO(accommodationReservationService.GetById(guestGradeDTO.ReservationId));
                    updatedDTO.AccommodationReservation = accommodationReservationService.GetOneReservation(reservationDTO);
                    AllOwnersRatings.Add(updatedDTO);
                }
            }
        }

        private void OnOpenDetails(GuestGradeDTO selectedGrade)
        {
            OwnerGradeDetails ownerGradeDetails = new OwnerGradeDetails(navigationService, selectedGrade);
            navigationService.Navigate(ownerGradeDetails);
        }
       
    }
}
