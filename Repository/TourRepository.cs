using BookingApp.Model;
using BookingApp.Observer;
using BookingApp.DTO;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class TourRepository
    {

        private const string FilePath = "../../../Resources/Data/tours.csv";

        private readonly Serializer<Tour> serializer;

        private List<Tour> tours;

        public Subject subject;

        public TourRepository()
        {
            serializer = new Serializer<Tour>();
            tours = serializer.FromCSV(FilePath);
            subject= new Subject();
        }

        public List<Tour> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public Tour Add(Tour tour)
        {
            tour.Id = NextId();
            tours = serializer.FromCSV(FilePath);
            tours.Add(tour);
            serializer.ToCSV(FilePath, tours);
            subject.NotifyObservers();
            return tour;
        }

        public int NextId()
        {
            tours = serializer.FromCSV(FilePath);
            if (tours.Count < 1)
            {
                return 1;
            }
            return tours.Max(c => c.Id) + 1;
        }

        public void Delete(Tour tour)
        {
            tours =serializer.FromCSV(FilePath);
            Tour founded = tours.Find(t => t.Id == tour.Id);
            tours.Remove(founded);
            serializer.ToCSV(FilePath, tours);
            subject.NotifyObservers();
        }

        public Tour Update(Tour tour)
        {
            tours = serializer.FromCSV(FilePath);
            Tour current = tours.Find(t => t.Id == tour.Id);
            int index = tours.IndexOf(current);
            tours.Remove(current);
            tours.Insert(index, tour);       // keep ascending order of ids in file 
           serializer.ToCSV(FilePath, tours);
           subject.NotifyObservers();
            return tour;
        }

        public List<Tour> GetByUser(User user)
        {
            tours = serializer.FromCSV(FilePath);
            return tours.FindAll(t => t.User.Id == user.Id);
        }

        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }


        public int GetCurrentId()
        {
            if (tours.Count == 0) return 1;
            return tours.Max(t => t.Id);
        }
        public Tour? GetTourById(int id)
        {
            return tours.Find(s => s.Id == id);

        }
    }
}
