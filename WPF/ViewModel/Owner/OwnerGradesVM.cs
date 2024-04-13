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
    public class OwnerGradesVM : ViewModelBase
    {

       // public GuestGradeService guestGradeService;
        private AccommodationGradeService accommodationGradeService;
        public ObservableCollection<AccommodationGradeDTO> AllOwnerGrades { get; set; }
        public AccommodationGradeDTO SelectedAccommodationGrade { get; set; }

        public OwnerGradesVM() {
           // guestGradeService = new GuestGradeService();
            accommodationGradeService = new AccommodationGradeService();
            AllOwnerGrades = new ObservableCollection<AccommodationGradeDTO>();
            SelectedAccommodationGrade = new AccommodationGradeDTO();
            Update();
        }
        public void Update()
        {
            AllOwnerGrades.Clear();
            foreach (AccommodationGradeDTO accommodationGradeDTO in accommodationGradeService.GetAll())
            {
               // var updatedDTO = accommodationReservationService.GetAllInfo(accommodationReservationDTO);
               // AllAccommodationReservations.Add(updatedDTO);
            }
        }

       /*
        public void GuestDataGridSelectionChanged()
        {
            if (SelectedAccommodationReservation != null)
            {
                GuestDataGrid(SelectedAccommodationReservation);
            }
        }

        */
        
    }
}
