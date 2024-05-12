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
        public AccommodationReservationService(IAccommodationReservationRepository accommodationReservationRepository,IGuestRepository guestRepository,IUserRepository userRepository, IAccommodationRepository accommodationRepository, IImageRepository imageRepository, ILocationRepository locationRepository, IOwnerRepository ownerRepository)
        {
            this.accommodationReservationRepository = accommodationReservationRepository;
            accommodationService = new AccommodationService(accommodationRepository,imageRepository,locationRepository,ownerRepository);
            guestService = new GuestService(guestRepository);
            userService = new UserService(userRepository);
            imageService = new ImageService(imageRepository);
        }
        public bool IsOverFiveDays(AccommodationReservation accommodationReservation){
            DateTime currentDate = DateTime.Now;
            DateTime endDate = accommodationReservation.EndDate;
            TimeSpan difference = currentDate - endDate;
            return difference.Days < 5 && difference.Days > 0;
        }
        public List<AccommodationReservationDTO> GetAll(){
            List<AccommodationReservation> accommodationReservations= accommodationReservationRepository.GetAll();
            List<AccommodationReservationDTO> accommodationReservationDTOs = accommodationReservations.Select(accres => new AccommodationReservationDTO(accres)).ToList();
            return accommodationReservationDTOs;
        }
        public AccommodationReservation GetById(int id){ return accommodationReservationRepository.GetById(id); }
        public AccommodationReservationDTO GetByIdDTO(int id)
        {
            var accommodationReservationDTO = new AccommodationReservationDTO(accommodationReservationRepository.GetById(id));
            return accommodationReservationDTO;
        }
        public void UpdateDate(AccommodationReservationDTO accommodationReservationDTO, DateTime initialDate, DateTime endTime) {
            accommodationReservationDTO.InitialDate = initialDate;
            accommodationReservationDTO.EndDate = endTime;
            accommodationReservationRepository.Update(accommodationReservationDTO.ToAccommodationReservation());
        }
        public List<(DateTime, DateTime)> FindAlternativeDates(AccommodationReservation reservation, int accommodationId){
            return accommodationReservationRepository.FindAlternativeDates(reservation, accommodationId);
        }
        public bool IsValid(AccommodationReservation reservation, Accommodation accommodation){
            return reservation.NumberOfGuests<=accommodation.Capacity && reservation.DaysToStay>=accommodation.MinStayDays &&
                accommodationReservationRepository.AreDatesValid(reservation.InitialDate, reservation.EndDate); //vraca true ako je sve tacno
        }
        public List<(DateTime, DateTime)> FindDateRange(AccommodationReservation reservation, int accommodationId){
            return accommodationReservationRepository.FindDateRange(reservation, accommodationId);}
        public bool AreDatesAvailable(int accommodationId, DateTime start, DateTime end){
            return accommodationReservationRepository.AreDatesAvailable(accommodationId, start, end); }
        public AccommodationReservation Add(AccommodationReservation reservation){
            return accommodationReservationRepository.Add(reservation); }
        //OVO MISLIMD A SE MOZE I OBRISATI-POGLEDAJ ZA SIMS,spoj u jedno za sims iste su
        public AccommodationReservation ProcessAlternativeDates(AccommodationReservation reservation, int accommodationId, Guest guest){
           // guest.UserId = userService.GetCurrentUserId();
          //  guestService.Add(guest);
            reservation.GuestId = guestService.GetCurrentId();
            return reservation;
        }
        public AccommodationReservation ProcessDateRange(AccommodationReservation reservation, int accommodationId, Guest guest){
           // guest.UserId = userService.GetCurrentUserId();
            //guestService.Add(guest);
            reservation.GuestId = guestService.GetCurrentId();
            return reservation; }
        public AccommodationReservationDTO GetOneReservation(AccommodationReservationDTO reservationDTO){
            var accommodation=accommodationService.GetAccommodation(reservationDTO.AccommodationId);
            var accommodationReservationDTO=new AccommodationReservationDTO(reservationDTO.ToAccommodationReservation(),accommodation.ToAccommodation(),accommodation.Location,accommodation.Owner);
            accommodationReservationDTO.Guest = new GuestDTO(guestService.GetById(reservationDTO.GuestId));
            accommodationReservationDTO.Images=imageService.GetImagesByAccommodation(accommodation.Id,imageService.GetImagesDTO());
            return accommodationReservationDTO;}
        public void Delete(AccommodationReservation accommodationReservation){  accommodationReservationRepository.Delete(accommodationReservation); }
          public List<AccommodationReservationDTO> GetReservationByAccommodation(int accommodationId)
        {
            var accommodationReservations = accommodationReservationRepository.GetReservationsForAccommodation(accommodationId);
            List<AccommodationReservationDTO> accommodationReservationDTOs = accommodationReservations.Select(accres => new AccommodationReservationDTO(accres)).ToList();
            return accommodationReservationDTOs;

        }
    }
}