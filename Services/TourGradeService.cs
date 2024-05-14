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
        private TourGuestService tourGuestService;
        private TourService tourService;
        private TourReservationService tourReservationService;
        private CheckPointService checkPointService;
        public TourGradeService(ICheckPointRepository checkPointRepository,ITourGradeRepository tourGradeRepository,ITourReservationRepository tourReservationRepository, ITourGuestRepository tourGuestRepository, IUserRepository userRepository, ITourStartDateRepository tourStartDateRepository, ITourRepository tourRepository, ILanguageRepository languageRepository, ILocationRepository locationRepository)
        {
            this.tourGradeRepository = tourGradeRepository;
            tourReservationService = new TourReservationService(tourReservationRepository,tourGuestRepository,userRepository,tourStartDateRepository,tourRepository,languageRepository,locationRepository);
            tourGuestService=new TourGuestService(tourGuestRepository);
            checkPointService=new CheckPointService(checkPointRepository);
            tourService = new TourService(tourRepository,languageRepository,locationRepository);
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
        public List<TourGradeDTO> GetById(int tourStartDateId)
        {
            List<TourGradeDTO> gradesDTO=new List<TourGradeDTO>();
            foreach (TourReservation reservation in tourReservationService.GetReservationsByStartDate(tourStartDateId))
            {
               TourGuest tourGuest= FindTourGuest(reservation.Id);
                List<TourGrade> grades = tourGradeRepository.GetAll().Where(t => t.TourReservationId == reservation.Id).ToList();
                foreach (var grade in grades)
                {
                    gradesDTO.Add(SetAtributes(grade,tourGuest));
                }
            }return gradesDTO;
        }
        private TourGradeDTO SetAtributes(TourGrade grade,TourGuest tourGuest)
        {
            TourGradeDTO gradeDTO = new TourGradeDTO(grade);
            gradeDTO.FullName = tourGuest.FullName;
            gradeDTO.CheckPointName = checkPointService.GetName(tourGuest.CheckPointId);
            return gradeDTO;
        }
        private TourGuest FindTourGuest(int reservationId)
        {
            return tourGuestService.GetAll().First(tg => tg.TourReservationId == reservationId);
        }
        public void UpdateValidity(int id)
        {
            TourGrade? tourGrade = tourGradeRepository.GetAll().Find(g=>g.Id== id);
            tourGrade.Validity = Validity.NO;
            tourGradeRepository.Update(tourGrade);
        }

        
        
            public bool IsTourRated(int reservationId)
            {
                var allGrades = tourGradeRepository.GetAll(); // Dobijamo sve ocene
                return allGrades.Any(grade => grade.TourReservationId == reservationId && grade.Validity == Validity.YES);
            }
       
    }
}
