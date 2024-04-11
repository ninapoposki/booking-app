using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Injector;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class GuestService
    {
        private IGuestRepository guestRepository;

        public GuestService()
        {
            guestRepository = Injector.Injector.CreateInstance<IGuestRepository>();
        }

        public Guest Add(Guest guest)
        {
            return guestRepository.Add(guest);
        }
    }
}
