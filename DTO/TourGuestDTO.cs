using BookingApp.Model;
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

        private int age { get; set; }   
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


        private User user { get; set; }

        public User User
        {


            get { return user; }


            set
            {



                if(value != user)
                {



                    user = value;
                    OnPropertyChanged("User");
                }
            }
        }

        public TourGuestDTO() { }

        public TourGuestDTO (TourGuest tourGuest)
        {

            Id=tourGuest.Id;
            FullName=tourGuest.FullName;
            Age=tourGuest.Age;
            User=tourGuest.User;


        }

        public TourGuest ToTourGuest()
        {


            return new TourGuest(Id, fullName, age);


        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
