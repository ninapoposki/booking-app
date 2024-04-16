using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.WPF.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class OwnerGradesVM : ViewModelBase {
        private AccommodationGradeService accommodationGradeService;
        private GuestGradeService guestGradeService;
        public AccommodationReservationService accommodationReservationService;
        public ObservableCollection<AccommodationGradeDTO> AllOwnerGrades { get; set; }
        public AccommodationGradeDTO SelectedAccommodationGrade { get; set; }
        public int ownerId;
        public OwnerGradesVM(int currentUserId) {
            guestGradeService = new GuestGradeService();
            accommodationGradeService = new AccommodationGradeService();
            accommodationReservationService = new AccommodationReservationService();
            AllOwnerGrades = new ObservableCollection<AccommodationGradeDTO>();
            SelectedAccommodationGrade = new AccommodationGradeDTO();
            ownerId = guestGradeService.GetByUserId(currentUserId).Id;
            Update();
        }
        public void Update(){
            AllOwnerGrades.Clear();
            foreach (AccommodationGradeDTO accommodationGradeDTO in accommodationGradeService.GetAll()){
                if(ownerId == accommodationGradeDTO.OwnerId && guestGradeService.IsGuestGraded(accommodationGradeDTO.ReservationId)) { 
                    var updatedDTO = accommodationGradeDTO;
                    updatedDTO.AccommodationReservation = GetReservation(accommodationGradeDTO.ReservationId);
                    AllOwnerGrades.Add(updatedDTO);
                }
            }
        }
        public AccommodationReservationDTO GetReservation(int reservationId) {
           // var reservation = accommodationReservationService.GetById(reservationId);
            AccommodationReservationDTO accommodationReservationDTO = new AccommodationReservationDTO(accommodationReservationService.GetById(reservationId));
            //accommodationReservationDTO.Accommodation = accommodationReservationService.accommodationService.GetByIdDTO(reservation.AccommodationId); ;
            // accommodationReservationDTO.Guest = accommodationReservationService.guestService.GetByIdDTO(reservation.GuestId);
            accommodationReservationDTO.Accommodation = accommodationReservationService.accommodationService.GetByIdDTO(accommodationReservationDTO.AccommodationId); ;
             accommodationReservationDTO.Guest = accommodationReservationService.guestService.GetByIdDTO(accommodationReservationDTO.GuestId);
            return accommodationReservationDTO;
        }
        public void GradeDetailsClick() {
            if (SelectedAccommodationGrade != null) {
                GradeDetails details = new GradeDetails(SelectedAccommodationGrade);
                details.ShowDialog();
            }  else {
                MessageBox.Show("Please select an accommodation grade first.");
            }
        }
    }
}
