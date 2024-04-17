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
    public class TourGradeRepository:ITourGradeRepository
    {

        private const string FilePath = "../../../Resources/Data/tourGrade.csv";

        private readonly Serializer<TourGrade> serializer;

        private List<TourGrade> tourGrades;
        public Subject subject;

        public TourGradeRepository()
        {
            serializer = new Serializer<TourGrade>();
            tourGrades = serializer.FromCSV(FilePath);
            subject = new Subject();
        }

       

        public TourGrade Add(TourGrade tourGrade)
        {
            tourGrade.Id = NextId();
            tourGrades = serializer.FromCSV(FilePath);
            tourGrades.Add(tourGrade);
            serializer.ToCSV(FilePath, tourGrades);
            subject.NotifyObservers();
            return tourGrade;
        }

        public int NextId()
        {
            tourGrades = serializer.FromCSV(FilePath);
            if (tourGrades.Count < 1)
            {
                return 1;
            }
            return tourGrades.Max(c => c.Id) + 1;
        }

        public void Delete(TourGrade tourGrade)
        {
            tourGrades = serializer.FromCSV(FilePath);
            TourGrade founded = tourGrades.Find(t => t.Id == tourGrade.Id);
            tourGrades.Remove(founded);
            serializer.ToCSV(FilePath, tourGrades);
            subject.NotifyObservers();
        }

        public TourGrade Update(TourGrade tourGrade)
        {
            tourGrades = serializer.FromCSV(FilePath);
            TourGrade current = tourGrades.Find(t => t.Id == tourGrade.Id);
            int index = tourGrades.IndexOf(current);
            tourGrades.Remove(current);
            tourGrades.Insert(index, tourGrade);       // keep ascending order of ids in file 
            serializer.ToCSV(FilePath, tourGrades);
            subject.NotifyObservers();
            return tourGrade;
        }
        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }
        public List<TourGrade> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public int GetCurrentId()
        {
            tourGrades = serializer.FromCSV(FilePath);
            int maxId = tourGrades.Count > 0 ? tourGrades.Max(t => t.Id) : 0;
            return maxId ;
        }
    }
}
