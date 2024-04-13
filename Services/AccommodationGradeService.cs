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

        public AccommodationGradeService()
        {
            accommodationGradeRepository = Injector.Injector.CreateInstance<IAccommodationGradeRepository>();
            userService = new UserService();
            ownerService = new OwnerService();
            
        }

        public List<double> GetAverageGrades(string username)
        {
            int ownerId = ownerService.GetByUserId(userService.GetByUsername(username).Id).Id;
            return accommodationGradeRepository.GetAverageGrades(ownerId);
        }
        public List<AccommodationGradeDTO> GetAll()
        {
            List<AccommodationGrade> accommodationGrades = accommodationGradeRepository.GetAll();
            List<AccommodationGradeDTO> accommodationGradeDTOs = accommodationGrades.Select(accres => new AccommodationGradeDTO(accres)).ToList();
            return accommodationGradeDTOs;
        }

    }
}
