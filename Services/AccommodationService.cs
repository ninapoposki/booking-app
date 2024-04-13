using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class AccommodationService
    {
        private IAccommodationRepository accommodationRepository;
        

        public AccommodationService()
        {
            accommodationRepository = Injector.Injector.CreateInstance<IAccommodationRepository>();
            
        }
        public Accommodation Add(Accommodation accommodation)
        {
            return accommodationRepository.Add(accommodation);
        }
        public int GetCurrentId()
        {
           return accommodationRepository.GetCurrentId();
        }
        
        public Accommodation GetById(int id)
        {
            return accommodationRepository.GetById(id);
        }
    }
}
