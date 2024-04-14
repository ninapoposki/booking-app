using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Injector;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class AccommodationGradeService
    {
        private IAccommodationGradeRepository accommodationGradeRepository;
        private UserService userService;
        private OwnerService ownerService;
        private GuestService guestService;
        private AccommodationService accommodationService;
        private GuestGradeService guestGradeService;
        private AccommodationReservationService accommodationReservationService;

        public AccommodationGradeService()
        {
            accommodationGradeRepository = Injector.Injector.CreateInstance<IAccommodationGradeRepository>();
            userService = new UserService();
            ownerService = new OwnerService();
            guestService = new GuestService();
            guestGradeService = new GuestGradeService();
            accommodationService = new AccommodationService();
            accommodationReservationService = new AccommodationReservationService();
            
        }

        public List<double> GetAverageGrades(string username)
        {
            int ownerId = ownerService.GetByUserId(userService.GetByUsername(username).Id).Id;
            return accommodationGradeRepository.GetAverageGrades(ownerId);
        }
        public List<AccommodationGradeDTO> GetAll()
        {
            List<AccommodationGrade> accommodationGrades = accommodationGradeRepository.GetAll();
            List<AccommodationGradeDTO> accommodationGradeDTOs = accommodationGrades.Select(accg => new AccommodationGradeDTO(accg)).ToList();
            return accommodationGradeDTOs;
        }

        public int GetReservationId(AccommodationGradeDTO selectedAccommodationGrade)
        {
            return accommodationGradeRepository.GetReservationId(selectedAccommodationGrade);
        }

        public AccommodationGradeDTO GetAllInfo(AccommodationGradeDTO accommodationGradeDTO)
        {

            var owner = ownerService.GetById(accommodationGradeDTO.OwnerId);
            accommodationGradeDTO.Owner = new OwnerDTO(owner);
            var accommres = accommodationReservationService.GetById(accommodationGradeDTO.ReservationId);
            accommodationGradeDTO.AccommodationReservation = new AccommodationReservationDTO(accommres);
            int guestId = accommres.GuestId;
            accommodationGradeDTO.AccommodationReservation.Guest = GetGuestFromAccommodationGrade(guestId);
            int accommodationId = accommres.AccommodationId;
            accommodationGradeDTO.AccommodationReservation.Accommodation = GetAccommodationFromAccommodationGrade(accommodationId);
            return accommodationGradeDTO;
        }

        public GuestDTO GetGuestFromAccommodationGrade(int guestId)
        {
            var guest = guestService.GetById(guestId);
            GuestDTO guestDTO = new GuestDTO(guest);

            return guestDTO;
        }

        public AccommodationDTO GetAccommodationFromAccommodationGrade(int accommodationId)
        {
            var accommodation = accommodationService.GetById(accommodationId);
            AccommodationDTO accommodationDTO = new AccommodationDTO(accommodation);

            return accommodationDTO;
        }

        public bool isGuestGraded(int reservationId)
        {
            return guestGradeService.IsGuestGraded(reservationId);
        }
    }
}
