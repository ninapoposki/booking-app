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
    public interface ICheckPointRepository
    {
         List<CheckPoint> GetAll();
         CheckPoint Add(CheckPoint checkPoint);
         int NextId();
         void Delete(CheckPoint checkPoint);
         CheckPoint Update(CheckPoint checkPoint);
         void Subscribe(IObserver observer);
         List<CheckPoint> GetByTourId(int id);
         CheckPoint GetById(int id);    
    }
}

