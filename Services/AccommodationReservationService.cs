using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Injector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class AccommodationReservationService
    {

        private IAccommodationReservationRepository accommodationReservationRepository;

        public AccommodationReservationService()
        {
            accommodationReservationRepository = Injector.Injector.CreateInstance<IAccommodationReservationRepository>();
        }
    }
}
