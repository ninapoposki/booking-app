using BookingApp.Domain.IRepositories;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Services;
using BookingApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class GradeGuestVM : ViewModelBase
    {
       public GuestGradeService GuestGradeService { get; set; }
        public AccommodationReservationDTO SelectedAccommodationReservation {  get; set; }
        public GuestGradeDTO guestGradeDTO { get; set; }
        public MyICommand<object> SetCleannessCommand { get; private set; }
        public MyICommand<object> SetRulesCommand { get; private set; }
        public MyICommand CloseWindow {  get; private set; }
        public MyICommand GradeGuest {  get; private set; }
        public GradeGuestVM(AccommodationReservationDTO accommodationReservationDTO) {

            
            this.SelectedAccommodationReservation = accommodationReservationDTO;
            GuestGradeService = new GuestGradeService(Injector.Injector.CreateInstance<IGuestGradeRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>());
            guestGradeDTO = new GuestGradeDTO();
            SetCleannessCommand = new MyICommand<object>(SetCleanness);
            SetRulesCommand = new MyICommand<object>(SetRules);
            CloseWindow = new MyICommand(Close);
            GradeGuest = new MyICommand(AddGrade);
        }


        
        private void Close()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
            }
        }
        public void AddGrade()
        {
          //  CleannessRadio = cleanness;
           // FollowingTheRulesRadio = followingrules;
            guestGradeDTO.Cleanless = CleannessRadio;
            guestGradeDTO.RulesFollowing = FollowingTheRulesRadio;
            guestGradeDTO.Comment = Comments;
            guestGradeDTO.GuestId = SelectedAccommodationReservation.GuestId;
            guestGradeDTO.ReservationId = SelectedAccommodationReservation.Id;
            GuestGradeService.Add(guestGradeDTO.ToGuestGrade());
            MessageBox.Show("Grade added successfully!");
            Close();
        }
        private void SetCleanness(object parameter)
        {
            if (parameter != null && int.TryParse(parameter.ToString(), out int cleannessValue))
            {
                CleannessRadio = cleannessValue;
            }
        }
        private void SetRules(object parameter)
        {
            if (parameter != null && int.TryParse(parameter.ToString(), out int rulesValue))
            {
                FollowingTheRulesRadio = rulesValue;
            }
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
