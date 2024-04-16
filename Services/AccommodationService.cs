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
    public class AccommodationService
    {
        private IAccommodationRepository accommodationRepository;
        private LocationService locationService;
        private ImageService imageService;
        private OwnerService ownerService;
        public AccommodationService()
        {
            accommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            locationService = new LocationService(Injector.Injector.CreateInstance<ILocationRepository>());
            ownerService = new OwnerService();

        }
        public Accommodation Add(Accommodation accommodation)
        {
            return accommodationRepository.Add(accommodation);
        }
        public int GetCurrentId()
        {
            return accommodationRepository.GetCurrentId();
        }


        public Accommodation GetById(int id)
        {
            return accommodationRepository.GetById(id);
        }
        public AccommodationDTO GetByIdDTO(int id)
        {
            var accommodationDTO=new AccommodationDTO(accommodationRepository.GetById(id));
            return accommodationDTO;
        }
        public AccommodationDTO GetAccommodation(int accommodationId)
        {
            var accommodation=accommodationRepository.GetById(accommodationId);
            var accommodationDTO = new AccommodationDTO(accommodation);
            return accommodationDTO;
        }

        public List<AccommodationDTO> GetAll()
        {
            List<Accommodation> accommodations = accommodationRepository.GetAll();
            List<AccommodationDTO> accommodationDTOs = accommodations.Select(acc => new AccommodationDTO(acc)).ToList();
            return accommodationDTOs;
        }
       
    }

}

