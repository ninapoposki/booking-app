using BookingApp.Domain.Model;
using BookingApp.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface ITourRequestRepository
    {
        List<TourRequest> GetAll();
        TourRequest Add(TourRequest tourRequest);
        int NextId();
        void Delete(TourRequest tourRequest);
        TourRequest Update(TourRequest tourRequest);
        void Subscribe(IObserver observer);
        TourRequest GetById(int id);
    }
}
