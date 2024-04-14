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
        public OwnerService ownerService;
        public UserService userService;
        private AccommodationGradeService accommodationGradeService;
        
        public ObservableCollection<AccommodationGradeDTO> AllOwnerGrades { get; set; }
        public AccommodationGradeDTO SelectedAccommodationGrade { get; set; }
        public int ownerId;

        public OwnerGradesVM(string username) {
            ownerService = new OwnerService();
            userService = new UserService();
            accommodationGradeService = new AccommodationGradeService();
            AllOwnerGrades = new ObservableCollection<AccommodationGradeDTO>();
            SelectedAccommodationGrade = new AccommodationGradeDTO();
            ownerId = ownerService.GetByUserId(userService.GetByUsername(username).Id).Id;
            Update();
        }
        public void Update()
        {
            AllOwnerGrades.Clear();
           
            foreach (AccommodationGradeDTO accommodationGradeDTO in accommodationGradeService.GetAll())
            {
                if(ownerId == accommodationGradeDTO.OwnerId && accommodationGradeService.isGuestGraded(accommodationGradeDTO.ReservationId)) { 
                    var updatedDTO = accommodationGradeService.GetAllInfo(accommodationGradeDTO);
                    AllOwnerGrades.Add(updatedDTO);
                }
            }
        }

        public void GradeDetailsClick()
        {
            
            if (SelectedAccommodationGrade != null)
            {
                GradeDetails details = new GradeDetails(SelectedAccommodationGrade);
                details.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select an accommodation grade first.");
            }
        }


    }
}
