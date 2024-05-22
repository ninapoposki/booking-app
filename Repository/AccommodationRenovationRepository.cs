using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Observer;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingApp.Repository
{
    public class AccommodationRenovationRepository: IAccommodationRenovationRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationRenovation.csv";
        private readonly Serializer<AccommodationRenovation> serializer;
        private List<AccommodationRenovation> accommodationRenovations;
        public Subject subject;




        public AccommodationRenovationRepository()
        {
            serializer = new Serializer<AccommodationRenovation>();
            accommodationRenovations = serializer.FromCSV(FilePath);
            subject = new Subject();

        }
       
        public List<AccommodationRenovation> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }
        public AccommodationRenovation Add(AccommodationRenovation accommodationRenovation)
        {
            accommodationRenovation.Id = NextId();
            accommodationRenovations.Add(accommodationRenovation);
            WriteToFile();
            subject.NotifyObservers();
            return accommodationRenovation;
        }
        public int NextId()
        {
            accommodationRenovations = serializer.FromCSV(FilePath);
            if (accommodationRenovations.Count < 1) { return 1; }
            return accommodationRenovations.Max(c => c.Id) + 1;
        }
        public int GetCurrentId()
        {
            accommodationRenovations = serializer.FromCSV(FilePath);
            int maxId = accommodationRenovations.Count > 0 ? accommodationRenovations.Max(t => t.Id) : 0;
            return maxId + 1;
        }
        public AccommodationRenovation GetById(int id)
        {
            accommodationRenovations = serializer.FromCSV(FilePath);
            return accommodationRenovations.Find(i => i.Id == id);
        }
       
        public void Delete(AccommodationRenovation accommodationRenovation)
        {
            accommodationRenovations = serializer.FromCSV(FilePath);
            AccommodationRenovation founded = accommodationRenovations.Find(c => c.Id == accommodationRenovation.Id);
            accommodationRenovations.Remove(founded);
            serializer.ToCSV(FilePath, accommodationRenovations);
            subject.NotifyObservers();
        }
        public AccommodationRenovation Update(AccommodationRenovation accommodationRenovation)
        {
            var existing = accommodationRenovations.FindIndex(a => a.Id == accommodationRenovation.Id);
            if (existing != -1)
            {
                accommodationRenovations[existing] = accommodationRenovation;
                WriteToFile();
                subject.NotifyObservers();
            }
            return accommodationRenovation;
        }
       
        private void WriteToFile()
        {
            serializer.ToCSV(FilePath, accommodationRenovations);
        }
        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }


    }
}
