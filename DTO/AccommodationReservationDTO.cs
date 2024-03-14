using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApp.Model; //obrisli posle kad doca prebaci u dto folder 

namespace BookingApp.DTO
{
    public class AccommodationReservationDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        //User
        private AccommodationDTO accommodation;
        public AccommodationDTO Accommodation
        {
            get
            {
                return accommodation;
            }
            set
            {
                if (value != accommodation)
                {
                    accommodation=value;
                    OnPropertyChanged("Accommodation");
                }

            }

        }

        private int accommodationId; //da li je neophodno?
        public int AccommodationId
        {
            get
            {
                return accommodationId;
            }
            set
            {
                if(value != accommodationId)
                {
                    accommodationId=value;
                    OnPropertyChanged("AccommodationId");
                }
            }
        }
        
        private DateOnly initialDate;
        public DateOnly InitialDate
        {
            get
            {
                return initialDate;
            }
            set
            {
                if (value != initialDate)
                {
                    initialDate = value;
                    OnPropertyChanged("InitialDate");
                }

            }

        }
        private DateOnly endDate;
        public DateOnly EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                if (value != endDate)
                {
                    endDate = value;
                    OnPropertyChanged("EndDate");
                }

            }
        }

        private int daysToStay;
        public int DaysToStay
        {
            get
            {
                return daysToStay;
            }
            set
            {
                if (value != daysToStay)
                {
                    daysToStay = value;
                    OnPropertyChanged("DaysToStay");
                }

            }
        }
        private int numberOfGuests;
        public int NumberOfGuests
        {
            get
            {
                return numberOfGuests;
            }
            set
            {
                if (value != numberOfGuests)
                {
                    numberOfGuests = value;
                    OnPropertyChanged("NumberOfGuests");
                }

            }
        }

        public AccommodationReservationDTO()
        { 

        }

        public AccommodationReservation ToAccommodationReservationDTO()
        {
            var accommodationReservation = new AccommodationReservation();

            accommodationReservation.Id = this.Id;
            accommodationReservation.AccommodationId= this.AccommodationId;
            // accommodationReservation.Accommodation = this.Accommodation;-ne ide ovo,samo id
            accommodationReservation.InitialDate = this.InitialDate;
            accommodationReservation.EndDate= this.EndDate;
            accommodationReservation.DaysToStay= this.DaysToStay;


            return accommodationReservation;
        }

        public AccommodationReservationDTO(AccommodationReservation reservation) //ili je bolje samo accommodationReservation
        {
            Id= reservation.Id;
            AccommodationId=reservation.AccommodationId;
            InitialDate= reservation.InitialDate;
            EndDate= reservation.EndDate;
            DaysToStay = reservation.DaysToStay;    

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }

        }


    }
}
