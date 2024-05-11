using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class TourStartDateService
    {
        private ITourStartDateRepository tourStartDateRepository;
        private TourService tourService;

        public TourStartDateService(ITourStartDateRepository tourStartDateRepository, ITourRepository tourRepository, ILanguageRepository languageRepository,ILocationRepository locationRepository)
        {
            this.tourStartDateRepository = tourStartDateRepository;
            tourService=new TourService(tourRepository,languageRepository,locationRepository);
        }
        public void Add(DateTime tourStartDate,int tourId)
        {
            TourStartDate tourDates = new TourStartDate(tourId, tourStartDate);
            tourStartDateRepository.Add(tourDates);
        }
        public IEnumerable<TourStartDateDTO> GetTourDates(int tourId)
        {
            var dateTimesForTour = new List<TourStartDateDTO>();
            foreach (var startTime in tourStartDateRepository.GetByTourId(tourId))
            {
                dateTimesForTour.Add(new TourStartDateDTO(startTime));
            }
            return dateTimesForTour;
        }
        public void UpdateStartTime(int id)
        {
            TourStartDate? tourStart = tourStartDateRepository.Get(id);
            tourStart.TourStatus = TourStatus.ACTIVE;
            tourStartDateRepository.Update(tourStart);   
        }
        public void UpdateEndTime(int id)
        {
            TourStartDate? tourStart=tourStartDateRepository.Get(id);
            tourStart.TourStatus=TourStatus.FINISHED;
            tourStartDateRepository.Update(tourStart);
        }
        public TourStartDateDTO GetActiveTour()
        {
            foreach(TourStartDate tourStart in tourStartDateRepository.GetAll())
            {
                if (tourStart.TourStatus.ToString().Equals("ACTIVE"))
                {
                    return new TourStartDateDTO(tourStart);
                }
            }return null;
        }     
        public void UpdateCurrentCheckPoint(int checkPointId,int selectedDateId)
        {
            TourStartDate? startDate=tourStartDateRepository.Get(selectedDateId);
            startDate.CurrentCheckPointId = checkPointId;
            tourStartDateRepository.Update(startDate);
        }
        public void UpdateTourStatus(int selectedDateId)
        {
            TourStartDate? startDate = tourStartDateRepository.Get(selectedDateId);
            startDate.TourStatus = TourStatus.CANCELED;
            tourStartDateRepository.Update(startDate);  
        }
        public List<TourDTO> GetAllFinishedTours(int userId)
        {
            List<TourDTO> finishedTours = new List<TourDTO>();
            foreach (TourDTO tour in tourService.GetAllForUser(userId))
            {
                var finishedDates = GetTourDates(tour.Id).Where(t => t.TourStatus == TourStatus.FINISHED);
                foreach (TourStartDateDTO tourStart in finishedDates)
                {
                    TourDTO tourDTO = tourService.GetTour(tourStart.TourId);
                    tourDTO.SelectedDateTime = tourStart;
                    finishedTours.Add(tourDTO);
                }
            }return finishedTours;
        }
        public List<TourDTO> GetByYear(int year, int userId)
        {
            return GetAllFinishedTours(userId).Where(t => t.SelectedDateTime.StartDateTime.Year == year).ToList();
        }
        public IEnumerable<TourStartDateDTO> GetAllInactiveTourDates()
        {
            var allTourStartDates = tourStartDateRepository.GetAll();
            var inactiveTourDates = allTourStartDates.Where(tsd => tsd.TourStatus == TourStatus.INACTIVE);
            return inactiveTourDates.Select(tsd => new TourStartDateDTO(tsd));
        }
        public TourStartDateDTO GetTourStartDate(int id)
        {
           var tourStartDate = tourStartDateRepository.Get(id);
           if (tourStartDate != null) { return new TourStartDateDTO(tourStartDate); }
            return null;
        }
        public List<DateTime> GetUnavailableDates(DateOnly startDate,DateOnly endDate)
        {
            List<DateTime> unavailableDates=new List<DateTime>();
            foreach(TourStartDate tourStartDate in tourStartDateRepository.GetAll())
            {
                if(DateOnly.FromDateTime(tourStartDate.StartTime)>=startDate && DateOnly.FromDateTime(tourStartDate.StartTime) <= endDate && tourStartDate.TourStatus.ToString().Equals("INACTIVE"))
                {
                    unavailableDates.Add(tourStartDate.StartTime.Date);
                }
            }return unavailableDates;
        }
    }
}
