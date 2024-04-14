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
    public interface ITourReservationRepository
    {
         List<TourReservation> GetAll();
         TourReservation Add(TourReservation tourReservation);
         int NextId();
         void Delete(TourReservation tourReservation);
         TourReservation Update(TourReservation tourReservation);
         List<TourReservation> GetByTourDateId(int id);
         void Subscribe(IObserver observer);
         TourReservation AddNewReservation(int tourStartDateId, int userId, int numberOfPeople);
       
    }
}
