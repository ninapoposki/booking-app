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
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class RenovationsVM : ViewModelBase {
        public ObservableCollection<AccommodationRenovationDTO> AllRenovations { get; set; }
        public AccommodationRenovationDTO SelectedRenovation { get; set; }
        public AccommodationService AccommodationService { get; set; }
        public AccommodationRenovationService AccommodationRenovationService { get; set; }
        public readonly AccommodationReservationService accommodationReservationService;
        public AccommodationDTO AccommodationDTO { get; set; }
        public MyICommand<AccommodationRenovationDTO> Delete{ get; private set; }
        public AccommodationRenovationDTO accommodationRenovationDTO { get; set; }
        public MyICommand TryToBookRenovationCommand { get; set; }
        public MyICommand ConfirmRenovation { get; set; }
        private string description;
        public string Description {
            get { return description; }
            set { description = value;  OnPropertyChanged(nameof(Description)); } }
       
        public RenovationsVM(AccommodationDTO accommodationDTO) {
            AccommodationDTO = accommodationDTO;
            AllRenovations = new ObservableCollection<AccommodationRenovationDTO>();
            AccommodationService = new AccommodationService(Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>());
            accommodationReservationService = new AccommodationReservationService(Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
                          Injector.Injector.CreateInstance<IGuestRepository>(), Injector.Injector.CreateInstance<IUserRepository>(),
                          Injector.Injector.CreateInstance<IAccommodationRepository>(),Injector.Injector.CreateInstance<IImageRepository>(),
                          Injector.Injector.CreateInstance<ILocationRepository>(), Injector.Injector.CreateInstance<IOwnerRepository>());
            AccommodationRenovationService = new AccommodationRenovationService(Injector.Injector.CreateInstance<IAccommodationRenovationRepository>());
            Delete = new MyICommand<AccommodationRenovationDTO>(DeleteRenovation);
            accommodationRenovationDTO = new AccommodationRenovationDTO();
            TryToBookRenovationCommand = new MyICommand(OnBookRenovation);
            ConfirmRenovation = new MyICommand(BookRenovation);
            Update();
        }
       
        public void BookRenovation() {
            AccommodationRenovationService.UpdateData(Description, AccommodationDTO.Id);
            Update();
        }
        public void Update()  {
            AllRenovations.Clear();
            foreach (AccommodationRenovationDTO accommodationRenovationDTO in AccommodationRenovationService.GetAll()) {
                    if (AccommodationDTO.Id== accommodationRenovationDTO.AccommodationId)  {
                        AllRenovations.Add(accommodationRenovationDTO);
                    }
            }
        }
        public void DeleteRenovation(AccommodationRenovationDTO accommodationRenovationDTO) {
            if (SelectedRenovation != null ) {
                if(AccommodationRenovationService.IsAboveFiveDays(SelectedRenovation)) { 
                AccommodationRenovationService.Delete(SelectedRenovation);
                    Update();
                }  else { MessageBox.Show("You cannot cancel reservation.It has been less than 5 days until renovation!"); }
            }
        }

        private void OnBookRenovation() {
             if (accommodationRenovationDTO.InitialDate < accommodationRenovationDTO.EndDate){
                ProcessValidRenovation();
            }  else {  HandleInvalidData();
            }
        }
      
        public void ProcessValidRenovation(){
            var accommodationReservationDTO =new AccommodationReservationDTO();
            accommodationReservationDTO.InitialDate = accommodationRenovationDTO.InitialDate;
            accommodationReservationDTO.EndDate = accommodationRenovationDTO.EndDate;
            accommodationReservationDTO.DaysToStay = accommodationRenovationDTO.Duration;
            List<(DateTime, DateTime)> dates1 = accommodationReservationService.FindDateRange(accommodationReservationDTO.ToAccommodationReservation(),AccommodationDTO.Id);
            List<(DateTime, DateTime)> dates = new List<(DateTime, DateTime)>();
            var accommodationRenovationDTOs = AccommodationRenovationService.GetAllByAccommodationId(AccommodationDTO.Id);
            foreach (var date in dates1) {
                bool found = false;
                foreach (var accren in accommodationRenovationDTOs) {
                    if (date.Item1 >= accren.InitialDate && date.Item1 <= accren.EndDate) {
                        found = true;
                        break;
                    }
                }
                if (!found){  dates.Add(date);}
            }
            if (dates.Count ==0) { 
                HandleUnavailableDates(); } else
            {
            AvailableDates availableDates = new AvailableDates( dates, accommodationReservationDTO);
            availableDates.Show();
            }
        }
        public void HandleUnavailableDates() {
            MessageBox.Show("The requested dates are not available. Here are some alternative options.");
            var accommodationReservationDTO = new AccommodationReservationDTO();
            accommodationReservationDTO.InitialDate = accommodationRenovationDTO.InitialDate;
            accommodationReservationDTO.EndDate = accommodationRenovationDTO.EndDate;
            accommodationReservationDTO.DaysToStay = accommodationRenovationDTO.Duration;
            List<(DateTime, DateTime)> dates = accommodationReservationService.FindAlternativeDates(accommodationReservationDTO.ToAccommodationReservation(),AccommodationDTO.Id);
          
            AvailableDates availableDates = new AvailableDates( dates, accommodationReservationDTO);
            availableDates.Show();
        }
        private void HandleInvalidData() {
            MessageBox.Show("The data you entered is not valid");
        }
    }
}
