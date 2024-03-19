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
    public class TourReservationRepository
    {

        private const string FilePath = "../../../Resources/Data/tourReservation.csv";

        private readonly Serializer<TourReservation> serializer;

        private List<TourReservation> tourReservations;

        public Subject subject;

        public TourReservationRepository()
        {
            serializer = new Serializer<TourReservation>();
            tourReservations = serializer.FromCSV(FilePath);
            subject = new Subject();
        }

        public List<TourReservation> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public TourReservation Add(TourReservation tourReservation)
        {
            tourReservation.Id = NextId();
            tourReservations = serializer.FromCSV(FilePath);
            tourReservations.Add(tourReservation);
            serializer.ToCSV(FilePath, tourReservations);
            subject.NotifyObservers();
            return tourReservation;
        }

        public int NextId()
        {
            tourReservations = serializer.FromCSV(FilePath);
            if (tourReservations.Count < 1)
            {
                return 1;
            }
            return tourReservations.Max(c => c.Id) + 1;
        }

        public void Delete(TourReservation tourReservation)
        {
            tourReservations = serializer.FromCSV(FilePath);
            TourReservation founded = tourReservations.Find(t => t.Id == tourReservation.Id);
            tourReservations.Remove(tourReservation);
            serializer.ToCSV(FilePath, tourReservations);
            subject.NotifyObservers();
        }

        public TourReservation Update(TourReservation tourReservation)
        {
            tourReservations = serializer.FromCSV(FilePath);
            TourReservation current = tourReservations.Find(t => t.Id == tourReservation.Id);
            int index = tourReservations.IndexOf(tourReservation);
            tourReservations.Remove(current);
            tourReservations.Insert(index, tourReservation);       // keep ascending order of ids in file 
            serializer.ToCSV(FilePath, tourReservations);
            subject.NotifyObservers();
            return tourReservation;
        }

        public List<TourReservation> GetByTourDateId(int id)
        {
            tourReservations=serializer.FromCSV(FilePath);
            return tourReservations.FindAll(tr=>tr.TourStartDateId == id);
        }
      

        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }

        public TourReservation AddNewReservation(int tourStartDateId, int userId, int numberOfPeople)
        {
            int newId = NextId(); 
            TourReservation newReservation = new TourReservation(newId, tourStartDateId, userId, numberOfPeople);
            tourReservations.Add(newReservation);
            serializer.ToCSV(FilePath, tourReservations); 
            subject.NotifyObservers();

            return newReservation;
        }

    }
}
