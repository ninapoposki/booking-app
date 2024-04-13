using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Injector;
using BookingApp.Repository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class AccommodationReservationService
    {
        private IAccommodationReservationRepository accommodationReservationRepository;

        private AccommodationService accommodationService;
        private GuestService guestService;
        private UserService userService;
        

        public AccommodationReservationService()
        {
            accommodationReservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            accommodationService = new AccommodationService();
            guestService = new GuestService();
            userService = new UserService();
          

        }
      
      
        
        public bool IsOverFiveDays(AccommodationReservationDTO accommodationReservationDTO)
        {
            return accommodationReservationRepository.IsOverFiveDays(accommodationReservationDTO.ToAccommodationReservation());
        }

        public List<AccommodationReservationDTO> GetAll()
        {
            List<AccommodationReservation> accommodationReservations= accommodationReservationRepository.GetAll();
            List<AccommodationReservationDTO> accommodationReservationDTOs = accommodationReservations.Select(accres => new AccommodationReservationDTO(accres)).ToList();
            return accommodationReservationDTOs;
        }
        

    
       public AccommodationReservationDTO GetAllInfo(AccommodationReservationDTO accommodationReservationDTO)
        {
            
              var guest = guestService.GetById(accommodationReservationDTO.GuestId);
              accommodationReservationDTO.Guest = new GuestDTO(guest);
              var accomm = accommodationService.GetById(accommodationReservationDTO.AccommodationId);
             accommodationReservationDTO.Accommodation = new AccommodationDTO(accomm);
            return accommodationReservationDTO;
        }

        

        public List<(DateTime, DateTime)> FindAlternativeDates(AccommodationReservation reservation, int accommodationId)
        {
            return accommodationReservationRepository.FindAlternativeDates(reservation, accommodationId);
        }
        public bool IsValid(AccommodationReservation reservation, Accommodation accommodation)
        {
            return accommodationReservationRepository.IsValid(reservation, accommodation);
        }
        public List<(DateTime, DateTime)> FindDateRange(AccommodationReservation reservation, int accommodationId)
        {
            return accommodationReservationRepository.FindDateRange(reservation, accommodationId);
        }

        public bool AreDatesAvailable(int accommodationId, DateTime start, DateTime end)
        {
            return accommodationReservationRepository.AreDatesAvailable(accommodationId, start, end);
        }

        public AccommodationReservation Add(AccommodationReservation reservation)
        {
            return accommodationReservationRepository.Add(reservation);
        }
      

        public AccommodationReservation ProcessAlternativeDates(AccommodationReservation reservation, int accommodationId, Guest guest)
        {
           // var dates = accommodationReservationRepository.FindAlternativeDates(reservation, accommodationId);
            guest.UserId = userService.GetCurrentGuestUserId();
            guestService.Add(guest);
            reservation.GuestId = guestService.GetCurrentId();
            return reservation;
        }
        

        public AccommodationReservation ProcessDateRange(AccommodationReservation reservation, int accommodationId, Guest guest)
        {
           // List<(DateTime, DateTime)> dates = accommodationReservationRepository.FindDateRange(reservation, accommodationId);
            guest.UserId = userService.GetCurrentGuestUserId();
            guestService.Add(guest);

            reservation.GuestId = guestService.GetCurrentId();

            return reservation;
        }


    }
}
