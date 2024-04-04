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
    public class TourStartDateRepository
    {
        private const string FilePath = "../../../Resources/Data/tourStartDates.csv";

        private readonly Serializer<TourStartDate> serializer;

        private List<TourStartDate> tourStartDates;

        public Subject subject;

        public TourStartDateRepository()
        {
            serializer = new Serializer<TourStartDate>();
            tourStartDates = serializer.FromCSV(FilePath);
            subject = new Subject();
        }

        public List<TourStartDate> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public List<TourStartDate> GetByTourId(int id)
        {
            return tourStartDates.FindAll(tsd => tsd.TourId == id);
        }

        public TourStartDate Add(TourStartDate tourStartDate)
        {
            tourStartDate.Id = NextId();
            tourStartDates = serializer.FromCSV(FilePath);
            tourStartDates.Add(tourStartDate);
            serializer.ToCSV(FilePath, tourStartDates);
            subject.NotifyObservers();
            return tourStartDate;
        }

        public int NextId()
        {
            tourStartDates = serializer.FromCSV(FilePath);
            if (tourStartDates.Count < 1)
            {
                return 1;
            }
            return tourStartDates.Max(c => c.Id) + 1;
        }

        public void Delete(TourStartDate tourStartDate)
        {
            tourStartDates = serializer.FromCSV(FilePath);
            TourStartDate founded = tourStartDates.Find(t => t.Id == tourStartDate.Id);
            tourStartDates.Remove(founded);
            serializer.ToCSV(FilePath, tourStartDates);
            subject.NotifyObservers();
        }

        public TourStartDate Update(TourStartDate tourStartDate)
        {
            tourStartDates = serializer.FromCSV(FilePath);
            TourStartDate current = tourStartDates.Find(t => t.Id == tourStartDate.Id);
            int index = tourStartDates.IndexOf(current);
            tourStartDates.Remove(current);
            tourStartDates.Insert(index, tourStartDate);       // keep ascending order of ids in file 
            serializer.ToCSV(FilePath, tourStartDates);
            subject.NotifyObservers();
            return tourStartDate;
        }

        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }
    }
}
