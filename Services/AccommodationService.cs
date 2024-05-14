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
        public LocationService locationService;
        public ImageService imageService;
        private OwnerService ownerService;
        public AccommodationService(IAccommodationRepository accommodationRepository,IImageRepository imageRepository,ILocationRepository locationRepository,IOwnerRepository ownerRepository)
        {
            this.accommodationRepository = accommodationRepository;
            imageService = new ImageService(imageRepository);
            locationService = new LocationService(locationRepository);
            ownerService = new OwnerService(ownerRepository);

        }
        public Accommodation Add(Accommodation accommodation)
        {
            return accommodationRepository.Add(accommodation);
        }
        public int GetCurrentId()
        {
            return accommodationRepository.GetCurrentId();
        }

        public void Delete(AccommodationDTO accommodationDTO)
        {
            accommodationRepository.Delete(accommodationDTO.ToAccommodation());
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
            accommodationDTO.Location = locationService.GetById(accommodation.IdLocation);
            accommodationDTO.Owner = ownerService.GetByUserId(accommodation.OwnerId);
           // var images = imageService.GetImagesDTO();
            //accommodationDTO.Images = new ObservableCollection<ImageDTO>(images);
            //commodationDTO.Images = new ImageDTO(images);
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

