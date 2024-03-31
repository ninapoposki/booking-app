using BookingApp.Model;
using BookingApp.Observer;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.Repository
{
    public class AccommodationReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationreservation.csv";

        private readonly Serializer<AccommodationReservation> serializer;
       
        private List<AccommodationReservation> accommodationReservations; 
        public Subject subject; 

        public AccommodationReservationRepository()
        {
            serializer = new Serializer<AccommodationReservation>();
            accommodationReservations = serializer.FromCSV(FilePath);
            subject = new Subject();
        }

        public List<AccommodationReservation> GetReservationsForAccommodation(int accommodationId)
        {
            return accommodationReservations.Where(r => r.AccommodationId == accommodationId).ToList();
        }
        public bool IsCapacityValid(int numberOfGuests, int maxGuests)
        {
            return numberOfGuests <= maxGuests;
        }
        public bool AreDatesValid(DateTime initialDate, DateTime endDate)
        {
            return initialDate < endDate;
        }

        public bool AreStayDaysValid(int daysToStay, int minimumStayDays)
        {
            return daysToStay >= minimumStayDays; //inicijalno je true
        }

        public bool IsValid(AccommodationReservation reservation,Accommodation accommodation)
        {
            return IsCapacityValid(reservation.NumberOfGuests, accommodation.Capacity) &&
                AreStayDaysValid(reservation.DaysToStay, accommodation.MinStayDays) && 
                AreDatesValid(reservation.InitialDate, reservation.EndDate); //vraca true ako je sve tacno

        }

        public List<(DateTime, DateTime)> FindAlternativeDates(AccommodationReservation reservation, int accommodationId)
        {
            List<(DateTime, DateTime)> availablePeriods = new List<(DateTime, DateTime)>();
            DateTime endRange = DateTime.Now.AddMonths(6);

            for (DateTime start = reservation.InitialDate; start <= endRange; start = start.AddDays(1))
            {
                DateTime end = start.AddDays(reservation.DaysToStay - 1);
                if (AreDatesAvailable(accommodationId, start, end))
                {
                    availablePeriods.Add((start, end));
                }
            }

            return availablePeriods;
        }

        public bool AreDatesAvailable(int accommodationId, DateTime start, DateTime end)
        {
            List<AccommodationReservation> reservations = GetReservationsForAccommodation(accommodationId);
            reservations.Sort((a, b) => a.InitialDate.CompareTo(b.EndDate));

            return !reservations.Any(r => r.AccommodationId == accommodationId && IsRangeOverlapping(r, start, end));
        }

        public bool IsRangeOverlapping(AccommodationReservation reservation, DateTime start, DateTime end)
        {
            bool startsBeforeEnd = IsStartBeforeEnd(start, reservation.EndDate);
            bool endsAfterStart = IsEndAfterStart(end, reservation.InitialDate);
            bool startsDuringReservation = IsStartDuringReservation(start, reservation);
            bool endsDuringReservation = IsEndDuringReservation(end, reservation);
            bool overlapsCompletely = OverlapsCompletely(start, end, reservation);

            return startsBeforeEnd && endsAfterStart || startsDuringReservation || endsDuringReservation || overlapsCompletely;
        }

        public bool IsStartBeforeEnd(DateTime start, DateTime endDate)
        {
            return start < endDate;
        }

        public bool IsEndAfterStart(DateTime end, DateTime startDate)
        {
            return end > startDate;
        }

        public bool IsStartDuringReservation(DateTime start, AccommodationReservation reservation)
        {
            return start >= reservation.InitialDate && start < reservation.EndDate;
        }

        public bool IsEndDuringReservation(DateTime end, AccommodationReservation reservation)
        {
            return end > reservation.InitialDate && end <= reservation.EndDate;
        }

        public bool OverlapsCompletely(DateTime start, DateTime end, AccommodationReservation reservation)
        {
            return start <= reservation.InitialDate && end >= reservation.EndDate;
        }

        public List<AccommodationReservation> GetAll()
        {
            return serializer.FromCSV(FilePath);
          
        }

        public AccommodationReservation Add(AccommodationReservation accommodationReservation)
        {
            accommodationReservation.Id = NextId();
            accommodationReservations.Add(accommodationReservation);
            WriteToFile();

            subject.NotifyObservers();
            return accommodationReservation;
        }
        private void WriteToFile()
        {
            serializer.ToCSV(FilePath, accommodationReservations);
        }

        public int NextId()
        {
            accommodationReservations = serializer.FromCSV(FilePath);
            if (accommodationReservations.Count < 1)
            {
                return 1;
            }
            return accommodationReservations.Max(c => c.Id) + 1;
        }

        public void Delete(AccommodationReservation accommodationReservation)
        {
            accommodationReservations = serializer.FromCSV(FilePath);
            AccommodationReservation founded = accommodationReservations.Find(c => c.Id == accommodationReservation.Id);
            serializer.ToCSV(FilePath, accommodationReservations);
            subject.NotifyObservers();
        }

        public AccommodationReservation Update(AccommodationReservation accommodationReservation)
        {
            var existing = accommodationReservations.FindIndex(a => a.Id == accommodationReservation.Id);
            if (existing != -1)
            {
                accommodationReservations[existing] = accommodationReservation;
                WriteToFile();
                subject.NotifyObservers();
            }
            return accommodationReservation;
        }
        public bool IsOverFiveDays(AccommodationReservation accommodationReservation)
        {
            
            DateTime currentDate = DateTime.Now;
            DateTime endDate = accommodationReservation.EndDate;
            TimeSpan difference = currentDate - endDate;
            return difference.Days < 5 && difference.Days > 0;
        }
        
     
    public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }
    }
}
