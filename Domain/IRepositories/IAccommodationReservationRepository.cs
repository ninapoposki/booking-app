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
    public interface IAccommodationReservationRepository
    {
        List<AccommodationReservation> GetReservationsForAccommodation(int accommodationId); //mislimd a mi ne treba ovo

        //bool IsValid(AccommodationReservation reservation,Accommodation accommodation);
        List<(DateTime, DateTime)> FindAlternativeDates(AccommodationReservation reservation, int accommodationId);
        bool AreDatesAvailable(int accommodationId,DateTime start,DateTime end);
        List<(DateTime, DateTime)> FindDateRange(AccommodationReservation reservation, int accommodationId);
        List<AccommodationReservation> GetAll();
        public AccommodationReservation GetById(int id);
        AccommodationReservation Add(AccommodationReservation reservation);
        int NextId();
        //WriteToFile?
        void Delete(AccommodationReservation accommodationReservation);
        AccommodationReservation Update(AccommodationReservation reservation);
       // bool IsOverFiveDays(AccommodationReservation reservation);
        void Subscribe(IObserver observer);
        bool AreDatesValid(DateTime initialDate, DateTime endDate);


    }
}
