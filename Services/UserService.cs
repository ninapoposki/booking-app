using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class UserService
    {
        private IUserRepository userRepository;


        public UserService()
        {
            userRepository = Injector.Injector.CreateInstance<IUserRepository>();
        }

        public User GetByUsername(string username) 
        { 
            return userRepository.GetByUsername(username);
        }


        public int GetCurrentGuestUserId()
        {
           return userRepository.GetCurrentGuestUserId();
        }



        public User FindUser(string currentUsername)
        {
            return userRepository.GetByUsername(currentUsername);
        }

        public void UpdateUser(AccommodationDTO accommodation, string currentUsername)
        {
            accommodation.OwnerId = FindUser(currentUsername).Id;
        }

    }
}
