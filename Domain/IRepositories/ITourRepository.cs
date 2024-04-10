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
    public interface ITourRepository
    {
        List<Tour> GetAll();
        Tour Add(Tour tour);
        int NextId();
        void Delete(Tour tour);
        Tour Update(Tour tour);
        List<Tour> GetByUser(User user);
        void Subscribe(IObserver observer);
        int GetCurrentId();
        Tour? GetById(int id);
    }
}

