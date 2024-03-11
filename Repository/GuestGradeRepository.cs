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
    public class GuestGradeRepository
    {
        private const string FilePath = "../../../Resources/Data/guestGrade.csv";

        private readonly Serializer<GuestGrade> serializer;

        private List<GuestGrade> guestGrades;
        public Subject subject;

        public GuestGradeRepository()
        {
            serializer = new Serializer<GuestGrade>();
            guestGrades = serializer.FromCSV(FilePath);
            subject = new Subject();
        }

        public List<GuestGrade> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public GuestGrade Add(GuestGrade guestGrade)
        {
            guestGrade.Id = NextId();
            guestGrades = serializer.FromCSV(FilePath);
            guestGrades.Add(guestGrade);
            serializer.ToCSV(FilePath, guestGrades);
            subject.NotifyObservers();
            return guestGrade;
        }

        public int NextId()
        {
            guestGrades = serializer.FromCSV(FilePath);
            if (guestGrades.Count < 1)
            {
                return 1;
            }
            return guestGrades.Max(c => c.Id) + 1;
        }

        public void Delete(GuestGrade guestGrade)
        {
            guestGrades = serializer.FromCSV(FilePath);
            GuestGrade founded = guestGrades.Find(c => c.Id == guestGrade.Id);
            guestGrades.Remove(founded);
            serializer.ToCSV(FilePath, guestGrades);
            subject.NotifyObservers();
        }

        public GuestGrade Update(GuestGrade guestGrade)
        {
            guestGrades = serializer.FromCSV(FilePath);
            GuestGrade current = guestGrades.Find(t => t.Id == guestGrade.Id);
            int index = guestGrades.IndexOf(current);
            guestGrades.Remove(current);
            guestGrades.Insert(index, guestGrade);       // keep ascending order of ids in file 
            serializer.ToCSV(FilePath, guestGrades);
            subject.NotifyObservers();
            return guestGrade;
        }

        public void Subscribe(IObserver observer)
        {
            subject.Subscribe(observer);
        }
    }
}
