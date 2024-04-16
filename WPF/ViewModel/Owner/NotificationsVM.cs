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

namespace BookingApp.WPF.ViewModel.Owner
{
    public class NotificationsVM : ViewModelBase
    {
        public GuestGradeService guestGradeService;
        public AccommodationService accommodationService;
        public AccommodationReservationService accommodationReservationService;
        public ObservableCollection<AccommodationReservationDTO> AllAccommodationReservations { get; set; }
        public NotificationsVM() {
            guestGradeService = new GuestGradeService();
            accommodationService = new AccommodationService();
            accommodationReservationService = new AccommodationReservationService();
            AllAccommodationReservations = new ObservableCollection<AccommodationReservationDTO>();
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
                    AllAccommodationReservations.Add(updatedDTO);
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

    }
}
