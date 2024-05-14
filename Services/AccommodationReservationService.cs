using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Injector;
using BookingApp.Repository;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class AccommodationReservationService
    {
        private IAccommodationReservationRepository accommodationReservationRepository;
        public AccommodationService accommodationService;
        public GuestService guestService;
        private UserService userService;
        private ImageService imageService;
        public AccommodationReservationService(IAccommodationReservationRepository accommodationReservationRepository, IGuestRepository guestRepository, IUserRepository userRepository, IAccommodationRepository accommodationRepository, IImageRepository imageRepository, ILocationRepository locationRepository, IOwnerRepository ownerRepository)
        {
            this.accommodationReservationRepository = accommodationReservationRepository;
            accommodationService = new AccommodationService(accommodationRepository, imageRepository, locationRepository, ownerRepository);
            guestService = new GuestService(guestRepository);
            userService = new UserService(userRepository);
            imageService = new ImageService(imageRepository);
        }
        public bool IsOverFiveDays(AccommodationReservation accommodationReservation)
        {
            DateTime currentDate = DateTime.Now;
            DateTime endDate = accommodationReservation.EndDate;
            TimeSpan difference = currentDate - endDate;
            return difference.Days < 5 && difference.Days > 0;
        }
        public List<AccommodationReservationDTO> GetAll()
        {
            List<AccommodationReservation> accommodationReservations = accommodationReservationRepository.GetAll();
            List<AccommodationReservationDTO> accommodationReservationDTOs = accommodationReservations.Select(accres => new AccommodationReservationDTO(accres)).ToList();
            return accommodationReservationDTOs;
        }
        public AccommodationReservation GetById(int id) { return accommodationReservationRepository.GetById(id); }
        public AccommodationReservationDTO GetByIdDTO(int id)
        {
            var accommodationReservationDTO = new AccommodationReservationDTO(accommodationReservationRepository.GetById(id));
            return accommodationReservationDTO;
        }
        public void UpdateDate(AccommodationReservationDTO accommodationReservationDTO, DateTime initialDate, DateTime endTime)
        {
            accommodationReservationDTO.InitialDate = initialDate;
            accommodationReservationDTO.EndDate = endTime;
            accommodationReservationRepository.Update(accommodationReservationDTO.ToAccommodationReservation());
        }
        public List<(DateTime, DateTime)> FindAlternativeDates(AccommodationReservation reservation, int accommodationId)
        {
            return accommodationReservationRepository.FindAlternativeDates(reservation, accommodationId);
        }
        public bool IsValid(AccommodationReservation reservation, Accommodation accommodation)
        {
            return reservation.NumberOfGuests <= accommodation.Capacity && reservation.DaysToStay >= accommodation.MinStayDays &&
                accommodationReservationRepository.AreDatesValid(reservation.InitialDate, reservation.EndDate); 
        }
        public List<(DateTime, DateTime)> FindDateRange(AccommodationReservation reservation, int accommodationId)
        {
            return accommodationReservationRepository.FindDateRange(reservation, accommodationId);
        }
        public bool AreDatesAvailable(int accommodationId, DateTime start, DateTime end)
        {
            return accommodationReservationRepository.AreDatesAvailable(accommodationId, start, end);
        }
        public AccommodationReservation Add(AccommodationReservation reservation)
        {
            return accommodationReservationRepository.Add(reservation);
        }
        public AccommodationReservation ProcessAlternativeDates(AccommodationReservation reservation, int accommodationId, Guest guest)
        {
            reservation.GuestId = guest.Id;
            return reservation;
        }
        public AccommodationReservation ProcessDateRange(AccommodationReservation reservation, int accommodationId, Guest guest)
        {
            reservation.GuestId = guest.Id;
            return reservation;
        }
        public AccommodationReservationDTO GetOneReservation(AccommodationReservationDTO reservationDTO)
        {
            var accommodation = accommodationService.GetAccommodation(reservationDTO.AccommodationId);
            var accommodationReservationDTO = new AccommodationReservationDTO(reservationDTO.ToAccommodationReservation(), accommodation.ToAccommodation(), accommodation.Location, accommodation.Owner);
            accommodationReservationDTO.Guest = new GuestDTO(guestService.GetById(reservationDTO.GuestId));
            accommodationReservationDTO.Images = imageService.GetImagesByAccommodation(accommodation.Id, imageService.GetImagesDTO());
            return accommodationReservationDTO;
        }
        public void Delete(AccommodationReservation accommodationReservation) { accommodationReservationRepository.Delete(accommodationReservation); }
        public List<AccommodationReservationDTO> GetOneYearReservations(int guestId)
        {
            var allReservations = GetAll();
            var guestReservations = new List<AccommodationReservationDTO>();
            foreach (var reservation in allReservations)
            {
                if (reservation.InitialDate > DateTime.Now.AddYears(-1) && reservation.EndDate < DateTime.Now && guestId == reservation.GuestId)
                {
                    guestReservations.Add(reservation);
                }
            }
            return guestReservations;
        }
        public List<AccommodationReservationDTO> GetReservationByAccommodation(int accommodationId)
        {
            var accommodationReservations = accommodationReservationRepository.GetReservationsForAccommodation(accommodationId);
            List<AccommodationReservationDTO> accommodationReservationDTOs = accommodationReservations.Select(accres => new AccommodationReservationDTO(accres)).ToList();
            return accommodationReservationDTOs;
        }
        public void SetGuestRole(GuestDTO guest)
        {
            var guestReservations = GetOneYearReservations(guest.Id);
            var currentDate = DateTime.Now;
            if (guest.Role == "SUPERGUEST")
            {
                if (guest.SuperGuestTime.AddYears(1) <= currentDate)
                {
                  UpdateGuestByReservations(guest,guestReservations);
                }
                else
                {
                    guest.Points = guestService.GetCurrentGuestPoints(guest.Id);
                    guestService.Update(guest.ToGuest());
                }
            }
            else
            {
                UpdateGuestByReservations(guest, guestReservations);
            }
        }
        public void UpdateGuestByReservations(GuestDTO guest,List<AccommodationReservationDTO> guestReservations)
        {
            var currentDate = DateTime.Now;
            if (guestReservations.Count() >= 10) {  guestService.SetSuperGuest(guest, currentDate); }
            else { guestService.SetGuest(guest); }
        }
    }
}