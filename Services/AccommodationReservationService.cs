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
        private GuestGradeService guestGradeService;
        

        public AccommodationReservationService()
        {
            accommodationReservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
            accommodationService = new AccommodationService();
            guestService = new GuestService();
            guestGradeService = new GuestGradeService();

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

        
    }
}
