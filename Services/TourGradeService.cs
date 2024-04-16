using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class TourGradeService
    {

        private ITourGradeRepository tourGradeRepository;
        private UserService userService;
        private TourService tourService;
        private TourReservationService tourReservationService;
        

        public TourGradeService()
        {
            tourGradeRepository=Injector.Injector.CreateInstance<ITourGradeRepository>();
            userService = new UserService();
           
          tourReservationService = new TourReservationService();
            tourService = new TourService();

        }

        public TourGrade Add(TourGrade grade)
        {
            return tourGradeRepository.Add(grade);
        }
        public TourGradeDTO GetOneTourGrade(TourReservationDTO tourReservationDTO, TourGradeDTO tourGradeDTO)
        {
            var tour = tourService.GetById(tourReservationDTO.TourStartDateId);


            return tourGradeDTO;

        }

        public int GetCurrentId()
        {
            return tourGradeRepository.GetCurrentId();
        }

    }
}
