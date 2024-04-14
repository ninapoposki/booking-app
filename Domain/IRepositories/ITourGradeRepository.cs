using BookingApp.Domain.Model;
using BookingApp.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface ITourGradeRepository
    {
        TourGrade Add(TourGrade tourGrade);
        int NextId();
        void Delete(TourGrade tourGrade);
        TourGrade Update(TourGrade tourGrade);
        void Subscribe(IObserver observer);
    }
}
