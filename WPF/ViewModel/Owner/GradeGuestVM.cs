using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class GradeGuestVM : ViewModelBase
    {
       public GuestGradeService GuestGradeService { get; set; }
        public AccommodationReservationDTO selectedAccommodationReservation;
        public GuestGradeDTO guestGradeDTO { get; set; }
        public GradeGuestVM(AccommodationReservationDTO accommodationReservationDTO) {

            
            selectedAccommodationReservation = accommodationReservationDTO;
            GuestGradeService = new GuestGradeService();
            guestGradeDTO = new GuestGradeDTO();
        }

       

        public void ConfirmButtonClick(int cleanness, int followingrules)
        {
            CleannessRadio = cleanness;
            FollowingTheRulesRadio = followingrules;
            guestGradeDTO.Cleanless = CleannessRadio;
            guestGradeDTO.RulesFollowing = FollowingTheRulesRadio;
            guestGradeDTO.Comment = Comments;
            guestGradeDTO.GuestId = selectedAccommodationReservation.GuestId;
            guestGradeDTO.ReservationId = selectedAccommodationReservation.Id;
            GuestGradeService.Add(guestGradeDTO.ToGuestGrade());
            
        }


        private int cleannessRadio;
        public int CleannessRadio
        {
            get { return cleannessRadio; }
            set
            {
                if (cleannessRadio != value)
                {
                    cleannessRadio = value;
                    OnPropertyChanged("CleannessRadio");
                }
            }
        }

        private int followingTheRulesRadioButtonChecked;
        public int FollowingTheRulesRadio
        {
            get { return followingTheRulesRadioButtonChecked; }
            set
            {
                if (followingTheRulesRadioButtonChecked != value)
                {
                    followingTheRulesRadioButtonChecked = value;
                    OnPropertyChanged("FollowingTheRulesRadioButtonChecked");
                }
            }
        }

        private string comment;
        public string Comments
        {
            get { return comment; }
            set
            {
                if (comment != value)
                {
                    comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }

    }
}
