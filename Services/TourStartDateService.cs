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

        public TourStartDateService()
        {
            tourStartDateRepository=Injector.Injector.CreateInstance<ITourStartDateRepository>();
            tourService=new TourService();
        }

        public void Add(DateTime tourStartDate,int tourId)
        {
            TourStartDate tourDates = new TourStartDate(tourId, tourStartDate);
            tourStartDateRepository.Add(tourDates);
        }

        public List<TourDTO> GetTours()
        {
            List<TourDTO> tourDTOs = new List<TourDTO>();
            foreach(TourStartDate tourStartDate in tourStartDateRepository.GetAll())
            {
                if(AreToursToday(tourStartDate))
                {
                    TourDTO newTodayTour = tourService.GetTour(tourStartDate.TourId);
                    tourDTOs.Add(newTodayTour);
                }
            }
            return tourDTOs;
          
        }

        private bool AreToursToday(TourStartDate tourStart)
        {
            return tourStart.StartTime.Date == DateTime.Today && tourStart.HasStarted == false && tourStart.HasFinished == false;
        }
        
        public IEnumerable<TourStartDateDTO> UpdateDate(int tourId)
        {
            var dateTimesForTour = new List<TourStartDateDTO>();
            foreach (var startTime in tourStartDateRepository.GetByTourId(tourId))
            {
                if (AreToursToday(startTime))
                {
                    dateTimesForTour.Add(new TourStartDateDTO(startTime));
                }
            }
            return dateTimesForTour;
        }

        public void UpdateStartTime(int id)
        {
            TourStartDate? tourStart = tourStartDateRepository.Get(id);
            tourStart.HasStarted = true;
            tourStartDateRepository.Update(tourStart);   
        }

        public void UpdateEndTime(int id)
        {
            TourStartDate? tourStart=tourStartDateRepository.Get(id);
            tourStart.HasFinished = true;
            tourStartDateRepository.Update(tourStart);
            
        }
    }
}
