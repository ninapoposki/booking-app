using BookingApp.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class ReservationRequestService
    {
        private IReservationRequestRepository reservationRequestRepository;


        public ReservationRequestService() {
            reservationRequestRepository = Injector.Injector.CreateInstance<IReservationRequestRepository>();
        }
    }
}
