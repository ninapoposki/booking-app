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

    public class AccommodationGradeRepository:IAccommodationGradeRepository
    {
        private const string FilePath = "../../../Resources/Data/accommodationGrade.csv";

        private readonly Serializer<AccommodationGrade> serializer;

        private List<AccommodationGrade> accommodationGrades;
        public Subject subject;

        public AccommodationGradeRepository()
        {
            serializer = new Serializer<AccommodationGrade>();
            accommodationGrades = serializer.FromCSV(FilePath);
            subject = new Subject();
        }
        public List<AccommodationGrade> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public AccommodationGrade Add(AccommodationGrade accommodationGrade)
        {
            accommodationGrade.Id = NextId();
            accommodationGrades = serializer.FromCSV(FilePath);
            accommodationGrades.Add(accommodationGrade);
            serializer.ToCSV(FilePath, accommodationGrades);
            subject.NotifyObservers();
            return accommodationGrade;
        }

        public int NextId()
        {
            accommodationGrades = serializer.FromCSV(FilePath);
            if (accommodationGrades.Count < 1)
            {
                return 1;
            }
            return accommodationGrades.Max(c => c.Id) + 1;
        }


        public void Delete(AccommodationGrade accommodationGrade)
        {
            accommodationGrades = serializer.FromCSV(FilePath);
            AccommodationGrade founded = accommodationGrades.Find(c => c.Id == accommodationGrade.Id);
            accommodationGrades.Remove(founded);
            serializer.ToCSV(FilePath, accommodationGrades);
            subject.NotifyObservers();
        }

        public AccommodationGrade Update(AccommodationGrade accommodationGrade)
        {
            accommodationGrades = serializer.FromCSV(FilePath);
            AccommodationGrade current = accommodationGrades.Find(t => t.Id == accommodationGrade.Id);
            int index = accommodationGrades.IndexOf(current);
            accommodationGrades.Remove(current);
            accommodationGrades.Insert(index, accommodationGrade);        
            serializer.ToCSV(FilePath, accommodationGrades);
            subject.NotifyObservers();
            return accommodationGrade;
        }

        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }
    }

}
