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
        private AccommodationService accommodationService;
        public AccommodationGradeService()
        {
            accommodationGradeRepository = Injector.Injector.CreateInstance<IAccommodationGradeRepository>();
            userService = new UserService();
            ownerService = new OwnerService();
            accommodationService = new AccommodationService();
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
        public AccommodationGrade Add(AccommodationGrade grade)
        {
            return accommodationGradeRepository.Add(grade);
        }
        public AccommodationGradeDTO GetOneAccommodationGrade(AccommodationReservationDTO accommodationReservationDTO,AccommodationGradeDTO accommodationGradeDTO)
        { 
            var accommodation = accommodationService.GetById(accommodationReservationDTO.AccommodationId);
            var owner = ownerService.GetById(accommodation.OwnerId);
            accommodationGradeDTO.OwnerId = accommodation.OwnerId; 
            accommodationGradeDTO.ReservationId = accommodationReservationDTO.Id;
            return accommodationGradeDTO;
        }
        public int GetCurrentId()
        {
            return accommodationGradeRepository.GetCurrentId();
        }
        public bool IsReservationGraded(int reservationId)
        {
            return accommodationGradeRepository.IsReservationGraded(reservationId);
        }
        public int GetReservationId(AccommodationReservation accommodationReservation) 
        {
            return accommodationGradeRepository.GetReservationId(accommodationReservation);

        }
    }
}
