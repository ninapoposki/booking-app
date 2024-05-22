using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Navigation;
using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using BookingApp.WPF.View.Guest;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class ReserveAccommodationVM : ViewModelBase
    {
        private readonly AccommodationReservationService accommodationReservationService;
        private readonly GuestService guestService;
        private readonly UserService userService;
        private readonly ImageService imageService;
        public NavigationService navigationService { get; set; }
        public AccommodationReservationDTO accommodationReservationDTO { get; set; } 
        public AccommodationDTO selectedAccommodationDTO { get; set; }
        public GuestDTO guestDTO { get; set; }
        public event EventHandler RequestClose;
        public MyICommand TryToBookAccommodationCommand { get; set; }
        public MyICommand ExitCommand { get; set; }
        public ReserveAccommodationVM(NavigationService navigationService,AccommodationDTO accommodationDTO,GuestDTO guestDTO){
            accommodationReservationService = new AccommodationReservationService(Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
                           Injector.Injector.CreateInstance<IGuestRepository>(),
                           Injector.Injector.CreateInstance<IUserRepository>(),
                           Injector.Injector.CreateInstance<IAccommodationRepository>(),
                           Injector.Injector.CreateInstance<IImageRepository>(),
                           Injector.Injector.CreateInstance<ILocationRepository>(),
                           Injector.Injector.CreateInstance<IOwnerRepository>());
            guestService = new GuestService(Injector.Injector.CreateInstance<IGuestRepository>());
            userService = new UserService(Injector.Injector.CreateInstance<IUserRepository>());
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            this.navigationService=navigationService;
            this.guestDTO=guestDTO;
            accommodationReservationDTO = new AccommodationReservationDTO(); 
            selectedAccommodationDTO = accommodationDTO;
            guestDTO =new GuestDTO();
            accommodationReservationDTO.AccommodationId = selectedAccommodationDTO.Id;
            TryToBookAccommodationCommand = new MyICommand(OnBookAccommodation);
            ExitCommand = new MyICommand(OnGoBack);
            UpdateImages();
        }
        public void UpdateImages()
        {
            var allImages = imageService.GetImagesForEntityType(EntityType.ACCOMMODATION); 
            var matchingImages = new ObservableCollection<ImageDTO>(imageService.GetImagesByAccommodation(selectedAccommodationDTO.Id, allImages));
            selectedAccommodationDTO.Images= matchingImages;
        }
        private void OnBookAccommodation()
        {
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
            accommodationReservationDTO = accommodationReservationService.GetOneReservation(accommodationReservationDTO);
            AvailableDatesWindow availableDates = new AvailableDatesWindow(navigationService, dates,accommodationReservationDTO);
            navigationService.Navigate(availableDates);
        }

        public void HandleUnavailableDates(){
            MessageBox.Show("The requested dates are not available. Here are some alternative options.");
            List<(DateTime, DateTime)> dates = accommodationReservationService.FindAlternativeDates(accommodationReservationDTO.ToAccommodationReservation(), selectedAccommodationDTO.Id);
            accommodationReservationDTO = new AccommodationReservationDTO(accommodationReservationService.ProcessAlternativeDates(accommodationReservationDTO.ToAccommodationReservation(), selectedAccommodationDTO.Id, guestDTO.ToGuest()));
            accommodationReservationDTO = accommodationReservationService.GetOneReservation(accommodationReservationDTO);
            AvailableDatesWindow availableDates = new AvailableDatesWindow(navigationService, dates, accommodationReservationDTO);
            navigationService.Navigate(availableDates);
        }

        private void HandleInvalidData(){
            MessageBox.Show("The data you entered is not valid");
        }
        private void OnGoBack()
        {
            navigationService.GoBack();
        }
    }
}
