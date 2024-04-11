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
    public interface ITourGuestRepository
    {
         List<TourGuest> GetAll();
         TourGuest Add(TourGuest tourGuest);
         int NextId();
         void Delete(TourGuest tourGuest);
         TourGuest Update(TourGuest tourGuest);
         void Subscribe(IObserver observer);
    }
}
