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
        public ReservationRequestService(IReservationRequestRepository reservationRequestRepository,IAccommodationReservationRepository accommodationReservationRepository,
            IGuestRepository guestRepository, IUserRepository userRepository, IAccommodationRepository accommodationRepository, IImageRepository imageRepository, ILocationRepository locationRepository, IOwnerRepository ownerRepository)
        {
            this.reservationRequestRepository = reservationRequestRepository;
            accommodationReservationService = new AccommodationReservationService(accommodationReservationRepository,guestRepository,userRepository,accommodationRepository,imageRepository,locationRepository,ownerRepository);
            accommodationService=new AccommodationService(accommodationRepository,imageRepository,locationRepository,ownerRepository);
        }
        public List<ReservationRequestDTO> GetAll()
        {
            List<ReservationRequest> reservationRequests = reservationRequestRepository.GetAll();
            List<ReservationRequestDTO> reservationRequestsDTOs = reservationRequests.Select(resreq => new ReservationRequestDTO(resreq)).ToList();
            return reservationRequestsDTOs;
        }
        public void DeleteById(int reservationId){
            reservationRequestRepository.DeleteById(reservationId);
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
                Comment="Still no answer!"
            };
            reservationRequestRepository.Add(reservationRequest);

        }
        public List<(DateTime, DateTime)> GenerateNewDateRange( int daysToStay)
        {
            (DateTime initialDate, DateTime endDate) = reservationRequestRepository.GetInitialDateRange();
            return  reservationRequestRepository.GenerateNewDateRange(initialDate, daysToStay);
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
