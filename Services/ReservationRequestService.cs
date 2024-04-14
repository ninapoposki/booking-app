using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class ReservationRequestService
    {
        private IReservationRequestRepository reservationRequestRepository;

        public AccommodationReservationService accommodationReservationService;
        private AccommodationService accommodationService;


        public ReservationRequestService()
        {
            reservationRequestRepository = Injector.Injector.CreateInstance<IReservationRequestRepository>();
            accommodationReservationService = new AccommodationReservationService();
            accommodationService=new AccommodationService();
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
      
        public void SetNewDates(DateTime initialDate, DateTime endDate,int reservationId)
        {
            var reservationRequest = new ReservationRequest
            {
                ReservationId = reservationId,
                NewInitialDate = initialDate,
                NewEndDate = endDate,
                RequestStatus=RequestStatus.ONHOLD,
            };
            reservationRequestRepository.Add(reservationRequest);

        }
        public List<(DateTime, DateTime)> GenerateNewDateRange( int daysToStay)
        {
            (DateTime initialDate, DateTime endDate) = reservationRequestRepository.GetInitialDateRange();
            return  reservationRequestRepository.GenerateNewDateRange(initialDate, daysToStay);
        }
        public List<ReservationRequestDTO> GetAll()
        {
            List<ReservationRequest> reservationRequests = reservationRequestRepository.GetAll();
            List<ReservationRequestDTO> reservationRequestDTOs = reservationRequests.Select(resreq => new ReservationRequestDTO(resreq)).ToList();
            return reservationRequestDTOs;
        }

        public ReservationRequestDTO GetOneRequest(ReservationRequestDTO requestDTO)
        {
            var reservation = accommodationReservationService.GetById(requestDTO.ReservationId);
            var accommodation = accommodationService.GetById(reservation.AccommodationId);
            requestDTO.Accommodation = new AccommodationDTO(accommodation);
            return requestDTO;

        }

    }
}
