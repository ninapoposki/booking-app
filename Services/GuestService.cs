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

        public GuestService(IGuestRepository guestRepository)
        {
            this.guestRepository = guestRepository;
        }
        public Guest GetById(int id)
        {
            return guestRepository.GetById(id);
        }

        public Guest Add(Guest guest)
        {
            return guestRepository.Add(guest);
        }

        public int GetCurrentId()
        {
            return guestRepository.GetCurrentId();  

        }
        public GuestDTO GetByIdDTO(int id)
        {
            var guestDTO = new GuestDTO(guestRepository.GetById(id));
            return guestDTO;
        }
        public GuestDTO UpdateGuest(int userId)
        {
            var guest = guestRepository.GetByUserId(userId);
            var guestDTO = new GuestDTO(guest);
            return guestDTO;
        }
        public GuestDTO GetByUserIdDTO(int userId)
        {
            var guestDTO=new GuestDTO(guestRepository.GetByUserId(userId));
            return guestDTO;
        }
        public void Update(Guest guest)
        {
            guestRepository.Update(guest);
        }
        public int GetCurrentGuestPoints(int guestId)
        {
            return guestRepository.GetCurrentGuestPoints(guestId);
        }
        
    }
}
