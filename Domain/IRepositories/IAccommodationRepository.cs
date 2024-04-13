using BookingApp.Domain.Model;
using BookingApp.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BookingApp.Domain.IRepositories
{
    public interface IAccommodationRepository
    {
        
        List<Accommodation> GetAll();
        public int GetCurrentId();
        Accommodation Add(Accommodation accommodation);
        void Delete(Accommodation accommodation);
        Accommodation Update(Accommodation accommodation);
        //List<Accommodation> GetAllByOwner(string owner);
        public Accommodation GetById(int id);
        void Subscribe(IObserver observer);

    }
}
