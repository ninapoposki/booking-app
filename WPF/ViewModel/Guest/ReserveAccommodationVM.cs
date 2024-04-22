using System;
using System.Collections.Generic;
using System.Windows;
using BookingApp.Domain.IRepositories;
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
        public ReserveAccommodationVM(AccommodationDTO accommodationDTO){
            accommodationReservationService = new AccommodationReservationService(Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
                           Injector.Injector.CreateInstance<IGuestRepository>(),
                           Injector.Injector.CreateInstance<IUserRepository>(),
                           Injector.Injector.CreateInstance<IAccommodationRepository>(),
                           Injector.Injector.CreateInstance<IImageRepository>(),
                           Injector.Injector.CreateInstance<ILocationRepository>(),
                           Injector.Injector.CreateInstance<IOwnerRepository>()); 
            guestService = new GuestService(Injector.Injector.CreateInstance<IGuestRepository>());
            userService = new UserService(Injector.Injector.CreateInstance<IUserRepository>());
            accommodationReservationDTO = new AccommodationReservationDTO(); 
            selectedAccommodationDTO = accommodationDTO;
            guestDTO =new GuestDTO();
            accommodationReservationDTO.AccommodationId = selectedAccommodationDTO.Id;
        }
        public void TryToBookAccommodation(){
            var guestDTO = new GuestDTO();
            guestDTO.FirstName = FirstName;
            guestDTO.LastName = LastName;
            var isReservationValid = accommodationReservationService.IsValid(accommodationReservationDTO.ToAccommodationReservation(), selectedAccommodationDTO.ToAccommodation());
            if (isReservationValid){ 
                CheckReservationAvailability();
            }else{
                HandleInvalidData();
            }
        }
        private void CheckReservationAvailability(){
            if (accommodationReservationService.AreDatesAvailable(selectedAccommodationDTO.Id, accommodationReservationDTO.InitialDate, accommodationReservationDTO.EndDate)){
                ProcessValidReservation();
            }else{
                HandleUnavailableDates();
            }
        }
        public void ProcessValidReservation() {
            List<(DateTime, DateTime)> dates = accommodationReservationService.FindDateRange(accommodationReservationDTO.ToAccommodationReservation(), selectedAccommodationDTO.Id);
            accommodationReservationDTO = new AccommodationReservationDTO(accommodationReservationService.ProcessDateRange(accommodationReservationDTO.ToAccommodationReservation(), selectedAccommodationDTO.Id, guestDTO.ToGuest()));
            var dialog = new AvailableDatesWindow(dates, accommodationReservationDTO);
            dialog.Closed += (sender, args) =>
            {
                RequestClose?.Invoke(this, EventArgs.Empty); 
            };
            dialog.ShowDialog();
        }

        public void HandleUnavailableDates(){
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

        private void HandleInvalidData(){
            MessageBox.Show("The data you entered is not valid");
        }
    }
}
