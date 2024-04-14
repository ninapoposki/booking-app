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
    public interface IReservationRequestRepository
    {

        public List<ReservationRequest> GetAll();
        public ReservationRequest Add(ReservationRequest reservationRequest);
        public int GetCurrentId();
        public ReservationRequest GetById(int id);
        public int NextId();
        public void Delete(ReservationRequest reservationRequest);
        public ReservationRequest Update(ReservationRequest reservationRequest);
        public void Subscribe(IObserver observer);
        List<(DateTime, DateTime)> GenerateNewDateRange(DateTime startDate, int daysToStay);
        (DateTime, DateTime) GetInitialDateRange();

    }
}
