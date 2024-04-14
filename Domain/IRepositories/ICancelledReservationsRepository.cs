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
    public interface ICancelledReservationsRepository
    {
        CancelledReservations Add(CancelledReservations cancelledReservation);
        int NextId();
        void Delete(CancelledReservations cancelledReservation);
        CancelledReservations Update(CancelledReservations cancelledReservation);
        void Subscribe(IObserver observer);
        bool IsCancellationPeriodValid(Accommodation accommodation, DateTime initialDate);
        bool IsReservationPassed(DateTime initialDate);

    }
}
