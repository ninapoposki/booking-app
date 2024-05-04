using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class TourGuestDTO : INotifyPropertyChanged
    {


        public int Id { get; set; }

        private string fullName { get; set; }

        public string FullName {
            
            get {  return fullName; }
            
            
            set { 
                
                if(value!=fullName) {

                    fullName = value;
                    OnPropertyChanged("FullName");
                }
                
            }
        }

        private string checkPointName { get; set; }

        public string CheckPointName
        {

            get { return checkPointName; }


            set
            {

                if (value != checkPointName)
                {

                    checkPointName = value;
                    OnPropertyChanged("CheckPointName");
                }

            }
        }
        private string profileImagePath;
        public string ProfileImagePath
        {
            get { return profileImagePath; }
            set
            {
                if (profileImagePath != value)
                {
                    profileImagePath = value;
                    OnPropertyChanged("ProfileImagePath");
                }
            }
        }
        private Gender gender;
        public Gender Gender
        {
            get { return gender; }
            set
            {
                if (value != gender)
                {

                    gender = value;
                    OnPropertyChanged("Gender");
                }
            }
        }
        private int age;
        public int Age
        {


            get { return age; }



            set
            {

                if (value != age)
                {

                    age = value;
                    OnPropertyChanged("Age");


                }

            }
        }
        public int CheckPointId { get; set; }
        private bool hasArrived;
        public bool HasArrived
        {



            get { return hasArrived; }



            set
            {

                if (value != hasArrived)
                {

                    hasArrived = value;
                    OnPropertyChanged("HasArrived");


                }

            }
        }
        public int TourReservationId {  get; set; }
        public TourGuestDTO() { }

        public TourGuestDTO (TourGuest tourGuest)
        {

            Id=tourGuest.Id;
            FullName=tourGuest.FullName;
            Age=tourGuest.Age;
            TourReservationId = tourGuest.TourReservationId;
            CheckPointId = tourGuest.CheckPointId;
            HasArrived = tourGuest.HasArrived;
            Gender = tourGuest.Gender;

        }

        public TourGuest ToTourGuest()
        {
            TourGuest guest = new TourGuest(Id, fullName, age, TourReservationId,gender);
            guest.CheckPointId = this.CheckPointId; 
            guest.HasArrived = this.HasArrived;  
            return guest;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
