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
    public class AccommodationService
    {
        private IAccommodationRepository accommodationRepository;
        private IUserRepository userRepository;

        public AccommodationService()
        {
            accommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            userRepository = Injector.Injector.CreateInstance<IUserRepository>();
        }
        public Accommodation Add(Accommodation accommodation)
        {
            return accommodationRepository.Add(accommodation);
        }
        public int GetCurrentId()
        {
           return accommodationRepository.GetCurrentId();
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
