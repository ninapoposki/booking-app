using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Model;
using BookingApp.Observer;

namespace BookingApp.Domain.IRepositories
{
    public interface IOwnerRepository
    {
        List<Owner> GetAll();
        Owner Add(Owner owner);
        int NextId();
        void Delete(Owner owner);
        Owner Update(Owner owner);
        Owner GetByUser(User user);
        Owner GetById(int id);
        int GetCurrentId();
        Owner GetByUserId(int userId);
        void Subscribe(IObserver observer);
    }
}
