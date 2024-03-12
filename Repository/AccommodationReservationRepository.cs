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

        private List<AccommodationReservation> accommodationReservations;
        public Subject subject;

        public AccommodationReservationRepository()
        {
            serializer = new Serializer<AccommodationReservation>();
            accommodationReservations = serializer.FromCSV(FilePath);
            subject = new Subject();
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
            AccommodationReservation founded = accommodationReservations.Find(c => c.Id == accommodationReservation.Id); //moguca nula vr-zato je podvuceno,obrati paznju
            accommodationReservations.Remove(founded);
            serializer.ToCSV(FilePath, accommodationReservations);
            subject.NotifyObservers();
        }

        public AccommodationReservation Update(AccommodationReservation accommodationReservation)
        {
            accommodationReservations = serializer.FromCSV(FilePath);
            AccommodationReservation current = accommodationReservations.Find(t => t.Id == accommodationReservation.Id);
            int index = accommodationReservations.IndexOf(current);
            accommodationReservations.Remove(current);
            accommodationReservations.Insert(index, accommodationReservation);       // keep ascending order of ids in file 
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
