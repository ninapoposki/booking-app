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
    public class ReservationRequestService
    {
        private IReservationRequestRepository reservationRequestRepository;
        public AccommodationReservationService accommodationReservationService;


        public ReservationRequestService()
        {
            reservationRequestRepository = Injector.Injector.CreateInstance<IReservationRequestRepository>();
            accommodationReservationService = new AccommodationReservationService();
        }
        public List<ReservationRequestDTO> GetAll()
        {
            List<ReservationRequest> reservationRequests = reservationRequestRepository.GetAll();
            List<ReservationRequestDTO> reservationRequestsDTOs = reservationRequests.Select(resreq => new ReservationRequestDTO(resreq)).ToList();
            return reservationRequestsDTOs;
        }
        
        public void UpdateStatus(int accommodationReservationId, RequestStatus status, string comment)
        {
            reservationRequestRepository.UpdateStatus(accommodationReservationId, status, comment);
        }
    }
}
