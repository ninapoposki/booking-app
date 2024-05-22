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
    public class RenovationRecommendationService
    {
        private IRenovationRecommendationRepository recommendationRepository;
        private AccommodationService accommodationService;
        private OwnerService ownerService;

        public RenovationRecommendationService(IRenovationRecommendationRepository recommendationRepository, IAccommodationRepository accommodationRepository, IImageRepository imageRepository, ILocationRepository locationRepository,
            IOwnerRepository ownerRepository)
        {
            this.recommendationRepository = recommendationRepository;
            ownerService = new OwnerService(ownerRepository);
            accommodationService = new AccommodationService(accommodationRepository, imageRepository, locationRepository, ownerRepository);
        }
        public RenovationRecommendation Add(RenovationRecommendation recommendation)
        {
            return recommendationRepository.Add(recommendation);
        }
        public RenovationRecommendationDTO GetOneRecommendation(AccommodationReservationDTO accommodationReservationDTO, RenovationRecommendationDTO recommendationDTO)
        {
            var accommodation = accommodationService.GetById(accommodationReservationDTO.AccommodationId);
            var owner = ownerService.GetById(accommodation.OwnerId);
            recommendationDTO.OwnerId = accommodation.OwnerId;
            recommendationDTO.ReservationId = accommodationReservationDTO.Id;
            return recommendationDTO;
        }
        public List<RenovationRecommendationDTO> GetAll()
        {
            List<RenovationRecommendation> recommendations = recommendationRepository.GetAll();
            List<RenovationRecommendationDTO> recommendationsDTOs = recommendations.Select(acc => new RenovationRecommendationDTO(acc)).ToList();
            return recommendationsDTOs;
        }
    }
}
