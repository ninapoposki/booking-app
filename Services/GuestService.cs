using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Injector;
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

        public Guest GetById(int id)
        {
            return guestRepository.GetById(id); 
        }
    }
}
