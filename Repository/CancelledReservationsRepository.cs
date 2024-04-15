
using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Observer;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class CancelledReservationsRepository:ICancelledReservationsRepository
    {
        private const string FilePath = "../../../Resources/Data/cancelledReservations.csv";

        private readonly Serializer<CancelledReservations> serializer;

        private List<CancelledReservations> cancelledReservations;

        public Subject subject;

        public CancelledReservationsRepository()
        {
            serializer = new Serializer<CancelledReservations>();
            cancelledReservations = serializer.FromCSV(FilePath);
            subject = new Subject();
        }

        public List<CancelledReservations> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }
        public bool IsReservationPassed(DateTime initialDate)
        {
            DateTime currentDate = DateTime.Now;
            return currentDate >= initialDate;
        }

        public CancelledReservations Add(CancelledReservations cancelledReservation)
        {
            cancelledReservation.Id = NextId();
            cancelledReservations = serializer.FromCSV(FilePath);
            cancelledReservations.Add(cancelledReservation);
            serializer.ToCSV(FilePath, cancelledReservations);
            subject.NotifyObservers();
            return cancelledReservation;
        }

        public int NextId()
        {
            cancelledReservations = serializer.FromCSV(FilePath);
            if (cancelledReservations.Count < 1)
            {
                return 1;
            }
            return cancelledReservations.Max(c => c.Id) + 1;
        }
        public bool IsCancellationPeriodValid( Accommodation accommodation, DateTime initialDate)
        {
            if (accommodation.CancellationPeriod == 0) 
            {
                DateTime currentDate = DateTime.Now;
                TimeSpan timeUntilCheckIn = initialDate - currentDate;
                return timeUntilCheckIn.TotalHours >= 24;
            }
            else
            {
                DateTime currentDate = DateTime.Now;
                TimeSpan timeUntilCheckIn = initialDate - currentDate;
                return timeUntilCheckIn.TotalDays >= accommodation.CancellationPeriod;
            }
        }


        public void Delete(CancelledReservations cancelledReservation)
        {
            cancelledReservations = serializer.FromCSV(FilePath);
            CancelledReservations founded = cancelledReservations.Find(r => r.Id == cancelledReservation.Id);
            cancelledReservations.Remove(founded);
            serializer.ToCSV(FilePath, cancelledReservations);
            subject.NotifyObservers();
        }

        public CancelledReservations Update(CancelledReservations cancelledReservation)
        {
            cancelledReservations = serializer.FromCSV(FilePath);
            CancelledReservations current = cancelledReservations.Find(r => r.Id == cancelledReservation.Id);
            int index = cancelledReservations.IndexOf(current);
            cancelledReservations.Remove(current);
            cancelledReservations.Insert(index, cancelledReservation);
            serializer.ToCSV(FilePath, cancelledReservations);
            subject.NotifyObservers();
            return cancelledReservation;
        }


        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }
    }
}
