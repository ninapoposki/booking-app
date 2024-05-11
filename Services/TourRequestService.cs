using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
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
        private ILocationRepository locationRepository;
        private ILanguageRepository languageRepository;

        public TourRequestService(ITourRequestRepository tourRequestRepository,ILocationRepository locationRepository,ILanguageRepository languageRepository)
        {
            this.tourRequestRepository = tourRequestRepository;
            this.locationRepository = locationRepository;
            this.languageRepository = languageRepository;
        }
        public List<TourRequest> GetAll()
        {
            return tourRequestRepository.GetAll();
        }
        public List<TourRequestDTO> GetAllUnaccepted()
        {
            List<TourRequestDTO> tourRequests = new List<TourRequestDTO>();
            foreach (TourRequest request in tourRequestRepository.GetAll())
            {
                if (request.State.ToString().Equals("PENDING"))
                {
                    Location location=locationRepository.GetById(request.LocationId);
                    Language language=languageRepository.GetById(request.LanguageId);
                    tourRequests.Add(new TourRequestDTO(request,location,language));
                } 
            }return tourRequests;
        }
        public void Update(TourRequest tourRequest)
        {
            tourRequest.State = State.ACCEPTED;
            tourRequest.IsNotified = true;
            tourRequestRepository.Update(tourRequest);
        }
    }
}
