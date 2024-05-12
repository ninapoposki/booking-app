using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class TourRequestService
    {
        private ITourRequestRepository tourRequestRepository;
        private LocationService locationService;
        private LanguageService languageService;
        public TourRequestService(ITourRequestRepository tourRequestRepository,ILocationRepository locationRepository,ILanguageRepository languageRepository)
        {
            this.tourRequestRepository = tourRequestRepository;
            locationService = new LocationService(locationRepository);
            languageService = new LanguageService(languageRepository);
        }
        public List<TourRequest> GetAll()
        {
            return tourRequestRepository.GetAll();
        }
        public TourRequest Update(TourRequest request)
        {
            return tourRequestRepository.Update(request);
        }
        public TourRequest SaveTourRequest(TourRequest tourRequest)
        {
           return tourRequestRepository.Add(tourRequest);
        }
        public List<TourRequestDTO> GetAllUnaccepted()
        {
            List<TourRequestDTO> tourRequests = new List<TourRequestDTO>();
            foreach (TourRequest request in tourRequestRepository.GetAll())
            {
                if (request.State.ToString().Equals("PENDING"))
                {
                    Location location=locationService.GetById(request.LocationId);
                    Language language=languageService.GetById(request.LanguageId);
                    tourRequests.Add(new TourRequestDTO(request,location,language));
                } 
            }return tourRequests;
        }
        public void UpdateState(TourRequest tourRequest)
        {
            tourRequest.State = State.ACCEPTED;
            tourRequest.IsNotified = true;
            tourRequestRepository.Update(tourRequest);
        }
        public IEnumerable<TourRequest> FilterRequests(int id, string type)
        {
            return tourRequestRepository.GetAll().Where(tourRequest =>
                (type == "language" && tourRequest.LanguageId == id) ||
                (type == "location" && tourRequest.LocationId == id));
        }
        internal int GetStatisticsPerYear(int id, string type, int year)
        {
            return FilterRequests(id, type).Where(tourRequest => tourRequest.CreationDate.Year == year).Count();
        }
        internal int GetStatisticsPerMonth(int id, string type, int year, int monthIndex)
        {
            return FilterRequests(id, type).Where(tourRequest => tourRequest.CreationDate.Year == year && tourRequest.CreationDate.Month == monthIndex).Count();
        }
    }
}
