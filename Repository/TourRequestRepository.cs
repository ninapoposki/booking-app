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
    public class TourRequestRepository : ITourRequestRepository
    {
        private const string FilePath = "../../../Resources/Data/tourRequest.csv";

        private readonly Serializer<TourRequest> serializer;

        private List<TourRequest> tourRequests;

        public Subject subject;

        public TourRequestRepository()
        {
            serializer = new Serializer<TourRequest>();
            tourRequests = serializer.FromCSV(FilePath);
            subject = new Subject();
        }
        public TourRequest Add(TourRequest tourRequest)
        {
            tourRequest.Id = NextId();
            tourRequests = serializer.FromCSV(FilePath);
            tourRequests.Add(tourRequest);
            serializer.ToCSV(FilePath, tourRequests);
            subject.NotifyObservers();
            return tourRequest;
        }
        public void Delete(TourRequest tourRequest)
        {
            tourRequests = serializer.FromCSV(FilePath);
            TourRequest? founded = tourRequests.Find(t => t.Id == tourRequest.Id);
            tourRequests.Remove(founded);
            serializer.ToCSV(FilePath, tourRequests);
            subject.NotifyObservers();
        }
        public List<TourRequest> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }
        public TourRequest? GetById(int id)
        {
            tourRequests = serializer.FromCSV(FilePath);
            return tourRequests.Find(s => s.Id == id);
        }
        public int NextId()
        {
            tourRequests = serializer.FromCSV(FilePath);
            if (tourRequests.Count < 1)
            {
                return 1;
            }
            return tourRequests.Max(c => c.Id) + 1;
        }

        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }
        public TourRequest Update(TourRequest tourRequest)
        {
            tourRequests = serializer.FromCSV(FilePath);
            TourRequest current = tourRequests.Find(t => t.Id == tourRequest.Id);
            int index = tourRequests.IndexOf(current);
            tourRequests.Remove(current);
            tourRequests.Insert(index, tourRequest);       // keep ascending order of ids in file 
            serializer.ToCSV(FilePath, tourRequests);
            subject.NotifyObservers();
            return tourRequest;
        }
    }
}
