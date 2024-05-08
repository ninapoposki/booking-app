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
        private bool FilterByCityAndCountry(AccommodationDTO accommodation, string cityFilter, string countryFilter){
            bool cityMatch = string.IsNullOrEmpty(cityFilter) || accommodation.Location.City.ToLower().Contains(cityFilter.ToLower());
            bool countryMatch = string.IsNullOrEmpty(countryFilter) || accommodation.Location.Country.ToLower().Contains(countryFilter.ToLower());
            return cityMatch && countryMatch;
        }
        private bool FilterByTypeAndName(AccommodationDTO accommodation, string nameFilter){
            return string.IsNullOrEmpty(nameFilter) || accommodation.Name.ToLower().Contains(nameFilter.ToLower());
        }

        private bool FilterByMinStayDaysAndCapacity(AccommodationDTO accommodation, string numberOfGuests, string numberOfDaysToStay)
        {
            bool capacityMatch = string.IsNullOrEmpty(numberOfGuests) || accommodation.Capacity >= int.Parse(numberOfGuests);
            bool minStayDaysMatch = string.IsNullOrEmpty(numberOfDaysToStay) || accommodation.MinStayDays <= double.Parse(numberOfDaysToStay);

            return capacityMatch && minStayDaysMatch;
        }

    }

}

