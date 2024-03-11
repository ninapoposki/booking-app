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
    public class TourReservationDTO : INotifyPropertyChanged
    {

        public int Id { get; set; }

        public int TourId { get; set; }
        private string tourDateTime;

        public string TourDateTime
        {




            get { return tourDateTime; }

            set {

                if (value != tourDateTime)
                {
                        tourDateTime = value;
                        OnPropertyChanged("TourDateTime");


                }

                     
            
            
            }
        }


        private Tour tour;

        public Tour Tour
        {
            get { return tour; }

            set
            {



                if (value != tour)
                {
                    tour= value;
                    OnPropertyChanged("Tour");


                }
            }
        }

        private User user;

        public User User
        {



            get { return user; }

            set
            {



                if (value != user)
                {


                    user= value;
                    OnPropertyChanged("User");
                }
            }
        }

        private int guestsNumber;
        public int GuestsNumber
        {




            get { return guestsNumber; }
            set
            {



                if(value != guestsNumber)
                {


                    guestsNumber = value;
                    OnPropertyChanged("GuestsNumber");
                }
            }



        }
            public TourReservationDTO() { }

            public TourReservationDTO(TourReservation tourReservation)
            {

                Id=tourReservation.Id;
                TourId=tourReservation.TourId;
                TourDateTime=tourReservation.TourDateTime.ToString();
                Tour=tourReservation.Tour;
                User= tourReservation.User;
                GuestsNumber=tourReservation.GuestsNumber;




            }

            public TourReservation ToTourReservation()
            {



                return new TourReservation(Id,TourId,DateOnly.Parse(tourDateTime),tour,guestsNumber);
            }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

       
    }
}
