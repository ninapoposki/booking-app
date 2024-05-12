using BookingApp.Domain.Model;
using BookingApp.Observer;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IGuestRepository
    {
        List <Guest> GetAll ();
        Guest Add(Guest guest);
        int NextId();
        void Delete(Guest guest);
        Guest Update (Guest guest);
        Guest GetByUserId(int userId);
        Guest GetById(int id);
        int GetCurrentId();
        int GetCurrentGuestPoints(int guestId);

        void Subscribe(IObserver observer);

    }
}
