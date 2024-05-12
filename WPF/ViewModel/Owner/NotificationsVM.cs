using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Services;
using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Utilities;
using BookingApp.WPF.View.Owner;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class NotificationsVM : ViewModelBase
    {
        public int currentUserId;
        public GuestGradeService guestGradeService;
        public AccommodationService accommodationService;
        public AccommodationReservationService accommodationReservationService;
        public ObservableCollection<AccommodationReservationDTO> AllAccommodationReservations { get; set; }
        public MyICommand<AccommodationReservationDTO> GradeGuestCommand { get; private set; }
        public MyICommand Close {  get; private set; }
        public NotificationsVM( int loggedInUserId) {
            currentUserId = loggedInUserId;
            guestGradeService = new GuestGradeService(Injector.Injector.CreateInstance<IGuestGradeRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>());
            accommodationService = new AccommodationService(Injector.Injector.CreateInstance<IAccommodationRepository>(),
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
            AllAccommodationReservations = new ObservableCollection<AccommodationReservationDTO>();
            GradeGuestCommand = new MyICommand<AccommodationReservationDTO>(GradeGuest);
            Close = new MyICommand(CloseNotifications);
            Update();
        }

        public void Update()
        {
            AllAccommodationReservations.Clear();
            foreach (AccommodationReservationDTO accommodationReservationDTO in accommodationReservationService.GetAll())
            {
                if (IsWithinFiveDays(accommodationReservationDTO) && !IsGuestGraded(accommodationReservationDTO.Id)) {
                    var updatedDTO = accommodationReservationService.GetOneReservation(accommodationReservationDTO);
                   // accommodationReservationDTO.Accommodation = accommodationReservationService.accommodationService.GetByIdDTO(accommodationReservationDTO.AccommodationId); ;
                    //accommodationReservationDTO.Guest = accommodationReservationService.guestService.GetByIdDTO(accommodationReservationDTO.GuestId);
                    // updatedDTO.Guest = GetGuest(accommodationReservationDTO.GuestId);
                    // updatedDTO.Accommodation = GetAccommodation(accommodationReservationDTO.AccommodationId);
                    updatedDTO.Message = "Not graded yet!";
                    if (updatedDTO.Owner.UserId == currentUserId)
                    {
                        AllAccommodationReservations.Add(updatedDTO);
                    }
                }
            }
        }
        
        private bool IsWithinFiveDays(AccommodationReservationDTO accommodationReservationDTO) {
            DateTime currentDate = DateTime.Now;
            DateTime endDate = accommodationReservationDTO.EndDate;
            TimeSpan difference = currentDate - endDate;
            return difference.Days < 5 && difference.Days > 0;
        }
         private bool IsGuestGraded(int reservationId) {
            return guestGradeService.IsGuestGraded(reservationId);  
         }
        public void GradeGuest(AccommodationReservationDTO reservation)
        {

            GradeGuestWindow gradeGuest = new GradeGuestWindow(reservation);
            gradeGuest.ShowDialog();
            Update();
        }
        public void CloseNotifications()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
            }
        }
    }
}
