using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class TourGuestService
    {

        private ITourGuestRepository tourGuestRepository;
        public TourGuestService(ITourGuestRepository tourGuestRepository) 
        {
            this.tourGuestRepository=tourGuestRepository;
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
                if (guest.TourReservationId == reservation.Id && guest.CheckPointId == -1 && guest.Type.ToString().Equals("TOUR"))
                {
                    guests.Add(new TourGuestDTO(guest));
                }
            } return guests;
        }
        public void UpdatePresentGuest(TourGuestDTO tourGuest, CheckPointDTO currentCheckPoint)
        {
            TourGuest guest = tourGuest.ToTourGuest();
            guest.CheckPointId = currentCheckPoint.Id;
            tourGuestRepository.Update(guest);
        }
        public void AddGuest(string fullName, int age, int reservationId,Gender gender)
        {
            TourGuest newGuest = new TourGuest(fullName, age, reservationId, gender);   
            tourGuestRepository.Add(newGuest);
        }
        public void MarkGuestAsArrived(TourGuestDTO tourGuestDTO)
        {
            TourGuest tourGuest = tourGuestDTO.ToTourGuest();
            tourGuest.HasArrived = true;
            tourGuestRepository.Update(tourGuest);
        }
        public void AddGuestRequest(string fullName, int age,int tourRequestId, Gender gender)
        {
            
            TourGuest newGuest = new TourGuest(fullName, age, tourRequestId, gender, BookingApp.Domain.Model.Type.REQUEST);
            tourGuestRepository.Add(newGuest);
        }
        public string GetCreatorName(int requestId)
        {
            return tourGuestRepository.GetAll().Where(tg => tg.TourReservationId == requestId && tg.Type.ToString().Equals("REQUEST")).First().FullName;
        }
        public List<TourGuestDTO> GetRequestGuest(int requestId)
        {
            return tourGuestRepository.GetAll().Where(tg => tg.TourReservationId == requestId && tg.Type.ToString().Equals("REQUEST")).Select(t => new TourGuestDTO(t)).ToList();
        }
    }
}
