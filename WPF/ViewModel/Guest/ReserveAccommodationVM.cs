using System;
using System.Collections.Generic;
using System.Windows;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.WPF.View.Guest;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class ReserveAccommodationVM : ViewModelBase
    {
        private readonly AccommodationReservationService accommodationReservationService;
        private readonly GuestService guestService;
        private readonly UserService userService;
        public AccommodationReservationDTO accommodationReservationDTO { get; set; } 

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public AccommodationDTO selectedAccommodationDTO { get; set; }
        public GuestDTO guestDTO { get; set; }
        public event EventHandler RequestClose;


        public ReserveAccommodationVM(AccommodationDTO accommodationDTO)
        {
            accommodationReservationService = new AccommodationReservationService();
            guestService = new GuestService();
            userService = new UserService();
            accommodationReservationDTO = new AccommodationReservationDTO(); 
            selectedAccommodationDTO = accommodationDTO;
            guestDTO =new GuestDTO();
            accommodationReservationDTO.AccommodationId = selectedAccommodationDTO.Id;

        }

        public void TryToBookAccommodation()
        {
            var guestDTO = new GuestDTO();
            guestDTO.FirstName = FirstName;
            guestDTO.LastName = LastName;

            var isReservationValid = accommodationReservationService.IsValid(accommodationReservationDTO.ToAccommodationReservation(), selectedAccommodationDTO.ToAccommodation());
            if (isReservationValid)
            {
                CheckReservationAvailability();
            }
            else
            {
                HandleInvalidData();
            }
        }

        private void CheckReservationAvailability()
        {
            if (accommodationReservationService.AreDatesAvailable(selectedAccommodationDTO.Id, accommodationReservationDTO.InitialDate, accommodationReservationDTO.EndDate))
            {
                ProcessValidReservation();
            }
            else
            {
                HandleUnavailableDates();
            }
        }

        
        public void ProcessValidReservation()
        {
            List<(DateTime, DateTime)> dates = accommodationReservationService.FindDateRange(accommodationReservationDTO.ToAccommodationReservation(), selectedAccommodationDTO.Id);
           // var novo = accommodationReservationService.ProcessDateRange(accommodationReservationDTO.ToAccommodationReservation(), selectedAccommodationDTO.Id, guestDTO.ToGuest());
            accommodationReservationDTO = new AccommodationReservationDTO(accommodationReservationService.ProcessDateRange(accommodationReservationDTO.ToAccommodationReservation(), selectedAccommodationDTO.Id, guestDTO.ToGuest()));
            var dialog = new AvailableDatesWindow(dates, accommodationReservationDTO);
            dialog.Closed += (sender, args) =>
            {
                RequestClose?.Invoke(this, EventArgs.Empty); 
            };
            dialog.ShowDialog();
        }

        public void HandleUnavailableDates()
        {
            MessageBox.Show("The requested dates are not available. Here are some alternative options.");
            List<(DateTime, DateTime)> dates = accommodationReservationService.FindAlternativeDates(accommodationReservationDTO.ToAccommodationReservation(), selectedAccommodationDTO.Id);

            accommodationReservationDTO = new AccommodationReservationDTO(accommodationReservationService.ProcessAlternativeDates(accommodationReservationDTO.ToAccommodationReservation(), selectedAccommodationDTO.Id, guestDTO.ToGuest()));

            var dialog = new AvailableDatesWindow(dates, accommodationReservationDTO);
            dialog.Closed += (sender, args) =>
            {
                RequestClose?.Invoke(this, EventArgs.Empty); 
            };
            dialog.ShowDialog();
        }

        private void HandleInvalidData()
        {
            MessageBox.Show("The data you entered is not valid");
        }
        /* private void ProcessValidReservation()
         {
             List<(DateTime, DateTime)> dates = accommodationReservationService.FindDateRange(accommodationReservationDTO.ToAccommodationReservation(), selectedAccommodationDTO.Id);
             guestDTO.UserId = userService.GetCurrentGuestUserId();
             guestService.Add(guestDTO.ToGuest());
             accommodationReservationDTO.GuestId = guestService.GetCurrentId();

             var dialog = new AvailableDatesWindow(dates, accommodationReservationDTO);
             dialog.Closed += (sender, args) =>
             {
                 RequestClose?.Invoke(this, EventArgs.Empty); 


             };
             dialog.ShowDialog();
         }

         private void HandleUnavailableDates()
         {
             MessageBox.Show("The requested dates are not available. Here are some alternative options.");
             List<(DateTime, DateTime)> dates = accommodationReservationService.FindAlternativeDates(accommodationReservationDTO.ToAccommodationReservation(), selectedAccommodationDTO.Id);
             guestDTO.UserId = userService.GetCurrentGuestUserId();
             guestService.Add(guestDTO.ToGuest());
             accommodationReservationDTO.GuestId = guestService.GetCurrentId();

             var dialog = new AvailableDatesWindow(dates, accommodationReservationDTO);
             dialog.Closed += (sender, args) =>
             {
                 RequestClose?.Invoke(this, EventArgs.Empty); // Pokretanje događaja za zatvaranje

             };
             dialog.ShowDialog();
         }*/
    }
}
