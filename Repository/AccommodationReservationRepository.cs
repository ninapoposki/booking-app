using BookingApp.Model;
using BookingApp.Observer;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class AccommodationReservationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationreservation.csv";

        private readonly Serializer<AccommodationReservation> serializer;

        private List<AccommodationReservation> accommodationReservations; //lista rezervisanih smestaja 
        public Subject subject;

        public AccommodationReservationRepository()
        {
            serializer = new Serializer<AccommodationReservation>();
            accommodationReservations = serializer.FromCSV(FilePath);
            subject = new Subject();
        }


        //mora accommodation da postoji
        public bool isReservationValid(AccommodationReservation accommodationReservation)
        {
            if(accommodationReservation.DaysToStay>accommodationReservation.Accommodation.MinStayDays  || accommodationReservation.NumberOfGuests<=accommodationReservation.Accommodation.Capacity) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void makeReservation(Accommodation accommodation)
        {
            List<Accommodation> reservedAccommodations=new List<Accommodation>();
            reservedAccommodations.Add(accommodation); //na prvu ruku ali ne valja 
        }
        public List<AccommodationReservation> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public AccommodationReservation Add(AccommodationReservation accommodationReservation)
        {
            accommodationReservation.Id = NextId();
            accommodationReservations = serializer.FromCSV(FilePath);
            accommodationReservations.Add(accommodationReservation);
            serializer.ToCSV(FilePath, accommodationReservations);
            subject.NotifyObservers();
            return accommodationReservation;
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
            accommodationReservations = serializer.FromCSV(FilePath);
            AccommodationReservation current = accommodationReservations.Find(t => t.Id == accommodationReservation.Id);
            int index = accommodationReservations.IndexOf(current);
            accommodationReservations.Remove(current);
            accommodationReservations.Insert(index, accommodationReservation);     
            serializer.ToCSV(FilePath, accommodationReservations);
            subject.NotifyObservers();
            return accommodationReservation;
        }

        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }
    }
}
