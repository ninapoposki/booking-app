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
    public class TourGuestService
    {

        private ITourGuestRepository tourGuestRepository;

        public TourGuestService()
        {
            tourGuestRepository = Injector.Injector.CreateInstance<ITourGuestRepository>();
        }
        public List<TourGuest> GetAll()
        {
            return tourGuestRepository.GetAll();
        }

        public List<TourGuestDTO> GetGuests(TourReservation reservation)
        {
            List<TourGuestDTO> guests = new List<TourGuestDTO>();
            foreach (TourGuest guest in GetAll())
            {
                if (guest.TourReservationId == reservation.Id && guest.CheckPointId == -1)
                {
                    guests.Add(new TourGuestDTO(guest));
                }
            }
            return guests;
        }
        public void UpdatePresentGuest(TourGuestDTO tourGuest, CheckPointDTO currentCheckPoint)
        {
            TourGuest guest = tourGuest.ToTourGuest();
            guest.CheckPointId = currentCheckPoint.Id;
            tourGuestRepository.Update(guest);
        }
        public void AddGuest(string fullName, int age, int reservationId)
        {
            TourGuest newGuest = new TourGuest
            {
                FullName = fullName,
                Age = age,
                TourReservationId = reservationId,
                CheckPointId = -1  
            };
            tourGuestRepository.Add(newGuest);
        }
    }
}
