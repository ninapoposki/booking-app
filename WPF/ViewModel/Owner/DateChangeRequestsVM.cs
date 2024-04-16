using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.WPF.ViewModel.Owner
{
     public class DateChangeRequestsVM : ViewModelBase {
        public ReservationRequestService reservationRequestService;
        public AccommodationReservationService accommodationReservationService;
        public AccommodationService accommodationService; 
        public GuestService guestService;
        public ReservationRequestDTO SelectedReservation { get; set; }
        public ObservableCollection<ReservationRequestDTO> AllReservationRequests { get; set; }
        public TextBox commentTextBox { get; set; }
        public DateChangeRequestsVM(TextBox textBox) {
            reservationRequestService = new ReservationRequestService();
            accommodationService = new AccommodationService();
            guestService = new GuestService(); 
            accommodationReservationService = new AccommodationReservationService();
            AllReservationRequests = new ObservableCollection<ReservationRequestDTO>();
            commentTextBox = textBox;
            Update();
        }
        public void Update() {
            AllReservationRequests.Clear();
            foreach (ReservationRequestDTO reservationRequestDTO in reservationRequestService.GetAll()) {
                if (reservationRequestDTO.RequestStatus == RequestStatus.ONHOLD) {
                    var updatedDTO = reservationRequestDTO;
                    updatedDTO.AccommodationReservation= GetAccommodationReservation(reservationRequestDTO.ReservationId);
                    updatedDTO.AccommodationReservation.Guest = GetGuest(reservationRequestDTO.AccommodationReservation.GuestId);
                    updatedDTO.AccommodationReservation.Accommodation = GetAccommodation(reservationRequestDTO.AccommodationReservation.AccommodationId);
                    bool isDateValid = accommodationReservationService.AreDatesAvailable(updatedDTO.AccommodationReservation.AccommodationId, updatedDTO.NewInitialDate, updatedDTO.NewEndDate);
                    if(isDateValid == true) { updatedDTO.Message = "FREE";
                    } else { updatedDTO.Message = "NOT FREE"; }
                 AllReservationRequests.Add(updatedDTO);
                }
            }
        }
        public AccommodationReservationDTO GetAccommodationReservation(int accommodatiodReservationId) {
            var accommres = accommodationReservationService.GetById(accommodatiodReservationId);
            AccommodationReservationDTO accommodationReservationDTO = new AccommodationReservationDTO(accommres);
            return accommodationReservationDTO;
        }
        public GuestDTO GetGuest(int guestId){
            var guest = guestService.GetById(guestId);
            GuestDTO guestDTO = new GuestDTO(guest);
            return guestDTO;
        }
        public AccommodationDTO GetAccommodation(int accommodationId){
            var accommodation = accommodationService.GetById(accommodationId);
            AccommodationDTO accommodationDTO = new AccommodationDTO(accommodation);
            return accommodationDTO;
        }
        public void DeclineButtonClick(){
            string Comment = commentTextBox.Text;
            reservationRequestService.UpdateStatus(SelectedReservation.ReservationId, RequestStatus.DECLINED, Comment  );
            MessageBox.Show("Requests is declined");
        }
        public void AcceptButtonClick() {
            if (SelectedReservation != null){
                string Comment = commentTextBox.Text;
                reservationRequestService.UpdateStatus(SelectedReservation.ReservationId, RequestStatus.ACCEPTED, Comment);
                int accommodationReservationId = SelectedReservation.AccommodationReservation.Id;
                DateTime initialDate = SelectedReservation.NewInitialDate;
                DateTime endDate = SelectedReservation.NewEndDate;
                accommodationReservationService.UpdateDate(SelectedReservation.AccommodationReservation, initialDate, endDate);
                MessageBox.Show("Requests is accepted");
            } else { MessageBox.Show("Please select a reservation before accepting."); }
        }
    }
}