using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Services;
using BookingApp.WPF.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class GuestReservationsVM : ViewModelBase {
        public  GuestGradeService guestGradeService;
        private AccommodationReservationService accommodationReservationService;
        public ObservableCollection<AccommodationReservationDTO> AllAccommodationReservations { get; set; }
        public AccommodationReservationDTO SelectedAccommodationReservation { get; set; }
        public GuestReservationsVM() {
            guestGradeService = new GuestGradeService();
            accommodationReservationService = new AccommodationReservationService();
            AllAccommodationReservations = new ObservableCollection<AccommodationReservationDTO>();
            SelectedAccommodationReservation = new AccommodationReservationDTO();
            Update();
        }
        public void Update() {
            AllAccommodationReservations.Clear();
            foreach (AccommodationReservationDTO accommodationReservationDTO in accommodationReservationService.GetAll())  {
                var updatedDTO = accommodationReservationService.GetOneReservation(accommodationReservationDTO);
                //accommodationReservationDTO.Accommodation = accommodationReservationService.accommodationService.GetByIdDTO(accommodationReservationDTO.AccommodationId); ;
                // accommodationReservationDTO.Guest = accommodationReservationService.guestService.GetByIdDTO(accommodationReservationDTO.GuestId);
                //updatedDTO.Guest = GetGuest(accommodationReservationDTO.GuestId);
                //updatedDTO.Accommodation = GetAccommodation(accommodationReservationDTO.AccommodationId);
                AllAccommodationReservations.Add(updatedDTO);
            }
        }
        
        public void GuestDataGrid(AccommodationReservationDTO selectedAccommodationReservation) {
                int reservationId = guestGradeService.GetReservationId(selectedAccommodationReservation);
                if (IsGuestGraded(reservationId)) {
                      MessageBox.Show("Guest is already graded.");
                }else {
                    AreDatesValid(selectedAccommodationReservation);
                }
        }
        public void GuestDataGridSelectionChanged() {
            if (SelectedAccommodationReservation != null){
                GuestDataGrid(SelectedAccommodationReservation);
            }
        }
        private void AreDatesValid(AccommodationReservationDTO accommodationReservationDTO) {
            if (accommodationReservationDTO.EndDate > DateTime.Now){
                MessageBox.Show("Guest stay has not finished yet!");
            } else{
                if (accommodationReservationService.IsOverFiveDays(accommodationReservationDTO)) {
                    GradeGuestWindow gradeGuestWindow = new GradeGuestWindow(accommodationReservationDTO);
                    gradeGuestWindow.ShowDialog();
                } else {  MessageBox.Show("Grading is not possible, it has been more than 5 days."); }
            }
        }
        private bool IsGuestGraded(int reservationId) {
            return guestGradeService.IsGuestGraded(reservationId); 
        }
    }
}