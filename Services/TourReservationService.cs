using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.Services
{
    public class TourReservationService
    {
        private ITourReservationRepository tourReservationRepository;
        private TourGuestService tourGuestService;
        private UserService userService;
        private TourStartDateService tourStartDateService;
        public TourReservationService(ITourReservationRepository tourReservationRepository,ITourGuestRepository tourGuestRepository,IUserRepository userRepository, ITourStartDateRepository tourStartDateRepository, ITourRepository tourRepository, ILanguageRepository languageRepository, ILocationRepository locationRepository)
        {
            this.tourReservationRepository = tourReservationRepository;
            tourGuestService = new TourGuestService(tourGuestRepository);
            userService = new UserService(userRepository);
            tourStartDateService = new TourStartDateService(tourStartDateRepository,tourRepository,languageRepository,locationRepository);
        }
        public bool DoReservationExists(int tourStartId)
        {
            return tourReservationRepository.GetByTourDateId(tourStartId).Count > 0;
        }

        public bool TryCreateReservation(int tourStartId, int userId, string username, int numberOfGuests, out int reservationId)
        {
            var currentUserId = userService.GetByUsername(username);
            var reservation = tourReservationRepository.AddNewReservation(tourStartId, currentUserId.Id, numberOfGuests);
            if (reservation == null)
            {
                reservationId = -1;
                return false;
            }
            reservationId = reservation.Id;
            return true;
        }

        public List<TourGuestDTO> GetByStartDate(int id)
        {
            List<TourGuestDTO> guests = new List<TourGuestDTO>();
            foreach (TourReservation reservation in tourReservationRepository.GetByTourDateId(id))
            {
                guests = tourGuestService.GetGuests(reservation);
            }
            return guests;
        }
        public List<TourReservation> GetReservationsByStartDate(int id)
        {
            return tourReservationRepository.GetAll().FindAll(t => t.TourStartDateId == id);

        }
        public List<TourGuestDTO> GetFinishedToursGuests(int tourStartDateId)
        {
            List<TourGuestDTO> guests = new List<TourGuestDTO>();
            foreach (TourGuest tourGuest in tourGuestService.GetAll())
            {
                foreach (TourReservation tourReservation in tourReservationRepository.GetAll().Where(t => t.Id == tourGuest.TourReservationId))
                {
                    if (tourReservation.TourStartDateId == tourStartDateId)
                    {
                        guests.Add(new TourGuestDTO(tourGuest));
                    }
                }
            } return guests;
        }
        public TourReservationDTO GetReservationByTourId(int tourStartDateId) {
            TourReservation? tourReservation = tourReservationRepository.GetAll().Find(t => t.TourStartDateId == tourStartDateId);
            return new TourReservationDTO(tourReservation);
        }

           public bool CheckIfReserved(int tourStartDateId)
           {
               TourGuestDTO guests = GetFinishedToursGuests(tourStartDateId).First(t => t.HasArrived == true && t.CheckPointId != -1);
               if (guests != null)    
                   return true;
               return false;
           }

       
        public List<TourReservationDTO> GetByUserId(int userId)
        {
            List<TourReservationDTO> reservations = new List<TourReservationDTO>();
            foreach (TourReservation tr in tourReservationRepository.GetAll().Where(tr => tr.UserId == userId))
            { 
                reservations.Add(new TourReservationDTO(tr));
            }
            return reservations;
        }
    }
}