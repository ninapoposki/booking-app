using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class CancelledReservationsService
    {
        private ICancelledReservationsRepository cancelledReservationsRepository;
        private AccommodationReservationService accommodationReservationService;
        private ReservationRequestService reservationRequestService;
        public CancelledReservationsService()
        {
            cancelledReservationsRepository = Injector.Injector.CreateInstance<ICancelledReservationsRepository>();
            accommodationReservationService=new AccommodationReservationService();
            reservationRequestService=new ReservationRequestService();
        }
        public void CancelReservation( Accommodation accommodation,AccommodationReservation accommodationReservation)
        {
                var cancelledReservation = new CancelledReservations
                {
                    AccommodationId = accommodation.Id,
                    InitialDate = accommodationReservation.InitialDate,
                    EndDate = accommodationReservation.EndDate,
                };
                accommodationReservationService.Delete(accommodationReservation);
                reservationRequestService.DeleteById(accommodationReservation.Id);
                cancelledReservationsRepository.Add(cancelledReservation);            
        }

        public bool IsCancellationPeriodValid(Accommodation accommodation,AccommodationReservation accommodationReservation)
        {
            return cancelledReservationsRepository.IsCancellationPeriodValid(accommodation,accommodationReservation.InitialDate);
        }

        public bool IsReservationPassed(DateTime initialDate)
        {
            return cancelledReservationsRepository.IsReservationPassed(initialDate);
        }
    }
}
