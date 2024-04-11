using BookingApp.Domain.Model;
using BookingApp.Observer;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface ITourStartDateRepository
    {
         List<TourStartDate> GetAll();
         List<TourStartDate> GetByTourId(int id);
         TourStartDate Add(TourStartDate tourStartDate);
         int NextId();
         void Delete(TourStartDate tourStartDate);
         TourStartDate Update(TourStartDate tourStartDate);
         void Subscribe(IObserver observer);
    }
}
