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
    public class AccommodationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodation.csv";

        private readonly Serializer<Accommodation> serializer;

        private List<Accommodation> accommodations;
        public Subject subject;

        public AccommodationRepository()
        {
            serializer = new Serializer<Accommodation>();
            accommodations = serializer.FromCSV(FilePath);
            subject = new Subject();
        }

        public List<Accommodation> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public Accommodation Add(Accommodation accommodation)
        {
            accommodation.Id = NextId();
            accommodations = serializer.FromCSV(FilePath);
            accommodations.Add(accommodation);
            serializer.ToCSV(FilePath, accommodations);
            subject.NotifyObservers();
            return accommodation;
        }

        public int NextId()
        {
            accommodations = serializer.FromCSV(FilePath);
            if (accommodations.Count < 1)
            {
                return 1;
            }
            return accommodations.Max(c => c.Id) + 1;
        }

        public void Delete(Accommodation accommodation)
        {
            accommodations = serializer.FromCSV(FilePath);
            Accommodation founded = accommodations.Find(c => c.Id == accommodation.Id);
            accommodations.Remove(founded);
            serializer.ToCSV(FilePath, accommodations);
            subject.NotifyObservers();
        }

        public Accommodation Update(Accommodation accommodation)
        {
            accommodations = serializer.FromCSV(FilePath);
            Accommodation current = accommodations.Find(t => t.Id == accommodation.Id);
            int index = accommodations.IndexOf(current);
            accommodations.Remove(current);
            accommodations.Insert(index, accommodation);       // keep ascending order of ids in file 
            serializer.ToCSV(FilePath, accommodations);
            subject.NotifyObservers();
            return accommodation;
        }

        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }
    }
}
