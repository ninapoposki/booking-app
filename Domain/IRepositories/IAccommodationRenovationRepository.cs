using BookingApp.Domain.Model;
using BookingApp.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IAccommodationRenovationRepository
    {
        List<AccommodationRenovation> GetAll();
        public int GetCurrentId();
        public int NextId();
        AccommodationRenovation Add(AccommodationRenovation accommodationRenovation);
        void Delete(AccommodationRenovation accommodationRenovation);
        AccommodationRenovation Update(AccommodationRenovation accommodationRenovation);
        
        public AccommodationRenovation GetById(int id);
        void Subscribe(IObserver observer);
    }
}
