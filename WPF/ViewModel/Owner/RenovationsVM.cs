using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using BookingApp.WPF.View.Guest;
using BookingApp.WPF.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class RenovationsVM
    {
        public ObservableCollection<AccommodationRenovationDTO> AllRenovations { get; set; }
        public AccommodationRenovationDTO SelectedRenovation { get; set; }
        public AccommodationService AccommodationService { get; set; }
        public AccommodationRenovationService AccommodationRenovationService { get; set; }
        private readonly AccommodationReservationService accommodationReservationService;

        public AccommodationDTO AccommodationDTO { get; set; }
        public MyICommand<AccommodationRenovationDTO> Delete{ get; private set; }
        public AccommodationRenovationDTO accommodationRenovationDTO { get; set; }
        public MyICommand TryToBookRenovationCommand { get; set; }
        public MyICommand ConfirmRenovation { get; set; }
        public List<(DateTime, DateTime)> dates { get; set; }
       
        public RenovationsVM(AccommodationDTO accommodationDTO) {
            AccommodationDTO = accommodationDTO;
            AllRenovations = new ObservableCollection<AccommodationRenovationDTO>();
            AccommodationService = new AccommodationService(Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>());
            accommodationReservationService = new AccommodationReservationService(Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
                          Injector.Injector.CreateInstance<IGuestRepository>(),
                          Injector.Injector.CreateInstance<IUserRepository>(),
                          Injector.Injector.CreateInstance<IAccommodationRepository>(),
                          Injector.Injector.CreateInstance<IImageRepository>(),
                          Injector.Injector.CreateInstance<ILocationRepository>(),
                          Injector.Injector.CreateInstance<IOwnerRepository>());
            AccommodationRenovationService = new AccommodationRenovationService(Injector.Injector.CreateInstance<IAccommodationRenovationRepository>());
            Delete = new MyICommand<AccommodationRenovationDTO>(DeleteRenovation);
            accommodationRenovationDTO = new AccommodationRenovationDTO();
            TryToBookRenovationCommand = new MyICommand(OnBookRenovation);
            ConfirmRenovation = new MyICommand(BookRenovation);
            dates = new List<(DateTime, DateTime)>();
            /*AvailableDatesVM availableDatesVM = new AvailableDatesVM(dates, accommodationReservation);
            availableDatesVM.DatesSelected += (sender, selectedDate) =>
            {
                // Ovde možete koristiti odabrane datume kako vam odgovara
                // Na primer, možete zatvoriti prozor i proslediti datume drugom prozoru
                // this.Close();
                // renovationsVM.ProcessSelectedDates(selectedDate);
            };*/

            Update();
        }
       
        public void BookRenovation()
        {
           
            AccommodationRenovationService.Add(accommodationRenovationDTO);
        }
        public void Update()
        {
            AllRenovations.Clear();
            foreach (AccommodationRenovationDTO accommodationRenovationDTO in AccommodationRenovationService.GetAll())
            {

               
                var accommodationDTO = AccommodationService.GetByIdDTO(accommodationRenovationDTO.AccommodationId);
                  
                    if (accommodationRenovationDTO.AccommodationId== accommodationDTO.Id)
                    {
                        AllRenovations.Add(accommodationRenovationDTO);
                    }
                
            }
        }
        public void DeleteRenovation(AccommodationRenovationDTO accommodationRenovationDTO)
        {
            if (SelectedRenovation != null )
            {
                if(IsAboveFiveDays(SelectedRenovation)) { 
                AccommodationRenovationService.Delete(SelectedRenovation);

                    Update();
                }
                else { MessageBox.Show("You cannot cancel reservation.It has been less than 5 days until renovation!"); }
            }
        }
        private bool IsAboveFiveDays(AccommodationRenovationDTO accommodationRenovationDTO)
        {
            DateTime currentDate = DateTime.Now;
            DateTime endDate = accommodationRenovationDTO.EndDate;
            TimeSpan difference = currentDate - endDate;
            return difference.Days > 5 ;
        }
        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        private void OnBookRenovation()
        {
          
            var accommodationDTO = accommodationReservationService.accommodationService.GetByIdDTO(AccommodationDTO.Id);
            var accommodationReservationDTO = accommodationReservationService.GetReservationByAccommodation(accommodationDTO.Id);
           
            if (accommodationRenovationDTO.InitialDate < accommodationRenovationDTO.EndDate)
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
            var accommodationDTO = accommodationReservationService.accommodationService.GetByIdDTO(AccommodationDTO.Id);
            if (accommodationReservationService.AreDatesAvailable(accommodationDTO.Id, accommodationRenovationDTO.InitialDate, accommodationRenovationDTO.EndDate))
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
            var accommodationReservationDTO =new AccommodationReservationDTO();
            accommodationReservationDTO.InitialDate = accommodationRenovationDTO.InitialDate;
            accommodationReservationDTO.EndDate = accommodationRenovationDTO.EndDate;
            accommodationReservationDTO.DaysToStay = accommodationRenovationDTO.Duration;
            List<(DateTime, DateTime)> dates = accommodationReservationService.FindDateRange(accommodationReservationDTO.ToAccommodationReservation(),AccommodationDTO.Id);
            //accommodationReservationDTO = new AccommodationReservationDTO(accommodationReservationService.ProcessDateRange(accommodationReservationDTO.ToAccommodationReservation(), selectedAccommodationDTO.Id, guestDTO.ToGuest()));
            AvailableDates availableDates = new AvailableDates( dates, accommodationReservationDTO);
            availableDates.Show();
        }

        public void HandleUnavailableDates()
        {
          /*  MessageBox.Show("The requested dates are not available. Here are some alternative options.");
            List<(DateTime, DateTime)> dates = accommodationReservationService.FindAlternativeDates(accommodationReservationDTO.ToAccommodationReservation(), selectedAccommodationDTO.Id);
            accommodationReservationDTO = new AccommodationReservationDTO(accommodationReservationService.ProcessAlternativeDates(accommodationReservationDTO.ToAccommodationReservation(), selectedAccommodationDTO.Id, guestDTO.ToGuest()));
            AvailableDatesWindow availableDates = new AvailableDatesWindow(navigationService, dates, accommodationReservationDTO);
            navigationService.Navigate(availableDates);*/
        }

        private void HandleInvalidData()
        {
            MessageBox.Show("The data you entered is not valid");
        }
    }
}
