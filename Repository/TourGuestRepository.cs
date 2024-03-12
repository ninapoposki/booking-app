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
    public class TourGuestRepository
    {

        private const string FilePath = "../../../Resources/Data/tourGuest.csv";

        private readonly Serializer<TourGuest> serializer;

        private List<TourGuest> tourGuests;

        public Subject subject;

        public TourGuestRepository()
        {
            serializer = new Serializer<TourGuest>();
            tourGuests = serializer.FromCSV(FilePath);
            subject = new Subject();
        }

        public List<TourGuest> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public TourGuest Add(TourGuest tourGuest)
        {
            tourGuest.Id = NextId();
            tourGuests = serializer.FromCSV(FilePath);
            tourGuests.Add(tourGuest);
            serializer.ToCSV(FilePath,tourGuests);
            subject.NotifyObservers();
            return tourGuest;
        }

        public int NextId()
        {
            tourGuests = serializer.FromCSV(FilePath);
            if (tourGuests.Count < 1)
            {
                return 1;
            }
            return tourGuests.Max(c => c.Id) + 1;
        }

        public void Delete(TourGuest tourGuest)
        {
            tourGuests = serializer.FromCSV(FilePath);
            TourGuest founded = tourGuests.Find(t => t.Id == tourGuest.Id);
            tourGuests.Remove(founded);
            serializer.ToCSV(FilePath, tourGuests);
            subject.NotifyObservers();
        }

        public TourGuest Update(TourGuest tourGuest)
        {
            tourGuests = serializer.FromCSV(FilePath);
            TourGuest current = tourGuests.Find(t => t.Id == tourGuest.Id);
            int index = tourGuests.IndexOf(current);
            tourGuests.Remove(current);
            tourGuests.Insert(index, tourGuest);       
            serializer.ToCSV(FilePath, tourGuests);
            subject.NotifyObservers();
            return tourGuest;
        }

       
        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }
    }
}
