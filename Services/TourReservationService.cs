﻿using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class TourReservationService
    {

        private ITourReservationRepository tourReservationRepository;
        private TourGuestService tourGuestService;
        private UserService userService;
        public TourReservationService()
        {
            tourReservationRepository = Injector.Injector.CreateInstance<ITourReservationRepository>();
            tourGuestService = new TourGuestService();
            userService = new UserService();
        }
        public bool DoReservationExists(int tourStartId)
        {
            return tourReservationRepository.GetByTourDateId(tourStartId).Count > 0;
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
        
        public bool TryCreateReservation(int tourStartId, int userId,string username, int numberOfGuests, out int reservationId)
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
    }
}