using BookingApp.Domain.IRepositories;
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

        public int GetCurrentGuestUserId()
        {
           return userRepository.GetCurrentGuestUserId();
        }





    }
}
