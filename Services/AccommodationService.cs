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
        private IUserRepository userRepository;
        private LocationService locationService;
        private ImageService imageService;
        public AccommodationService()
        {
            accommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            userRepository = Injector.Injector.CreateInstance<IUserRepository>();
            imageService = new ImageService();
            locationService = new LocationService();
        }
        public Accommodation Add(Accommodation accommodation)
        {
            return accommodationRepository.Add(accommodation);
        }
        public int GetCurrentId()
        {
           return accommodationRepository.GetCurrentId();
        }
        public List<AccommodationDTO> GetAll()
        {
            List<Accommodation> accommodations = accommodationRepository.GetAll();
            List<AccommodationDTO> accommodationDTOs = accommodations.Select(acc => new AccommodationDTO(acc)).ToList();
            return accommodationDTOs;
        }

        public User FindUser( string currentUsername)
        {
            return userRepository.GetByUsername(currentUsername);
           // image.EntityType = EntityType.TOUR;
            //imageRepository.Update(image.ToImage());
        }

        public void UpdateUser(AccommodationDTO accommodation,string currentUsername ) { 
            accommodation.OwnerId = FindUser(currentUsername).Id;
        }
       

    }

}

