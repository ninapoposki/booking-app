using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Repository;
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
      
    }
}
