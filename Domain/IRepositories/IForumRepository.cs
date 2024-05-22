using BookingApp.Domain.Model;
using BookingApp.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IForumRepository
    {
        List<Forum> GetAll();
        Forum Add(Forum forum);
        int NextId();
        void Delete(Forum forum);
        Forum Update(Forum forum);
        Forum GetById(int id);
        int GetCurrentId();
        void Subscribe(IObserver observer);

    }
}
