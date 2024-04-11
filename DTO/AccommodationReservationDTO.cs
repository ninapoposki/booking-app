using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.DTO
{
    public class AccommodationReservationDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }

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

        private int accommodationId; 
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

        private int guestId;

        public int GuestId
        {
            get
            {
                return guestId;
            }
            set
            {
                if (value != guestId)
                {
                    guestId = value;
                    OnPropertyChanged("GuestId");
                }
            }
        }

        public GuestDTO Guest { get; set; }
        public Accommodation Accommodations{ get; set; }
        public Owner Owner { get; set; } 
        public Location Location { get; set; }


        private DateTime initialDate=DateTime.Now;
        public DateTime InitialDate
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
     

        private DateTime endDate=DateTime.Now;
        public DateTime EndDate
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

        public AccommodationReservation ToAccommodationReservation()
        {
            var accommodationReservation = new AccommodationReservation();

            accommodationReservation.Id = this.Id;
            accommodationReservation.AccommodationId= this.AccommodationId;
            accommodationReservation.GuestId = this.GuestId;
            accommodationReservation.GuestId = this.GuestId;
            accommodationReservation.InitialDate = this.InitialDate;
            accommodationReservation.EndDate= this.EndDate;
            accommodationReservation.DaysToStay= this.DaysToStay;
            accommodationReservation.NumberOfGuests= this.NumberOfGuests;

            return accommodationReservation;
        }

        public AccommodationReservationDTO(AccommodationReservation reservation) 
        {
            Id= reservation.Id;
            GuestId = reservation.GuestId;
            AccommodationId=reservation.AccommodationId;
            GuestId=reservation.GuestId;
            InitialDate= reservation.InitialDate;
            EndDate= reservation.EndDate;
            DaysToStay = reservation.DaysToStay;   
            NumberOfGuests = reservation.NumberOfGuests;
        }
        //ovo proveri jel uopste sme ovako
        public AccommodationReservationDTO(AccommodationReservation reservation,Accommodation accommodation,Location location,Owner owner)
        {
            Id = reservation.Id;
            GuestId = reservation.GuestId;
            AccommodationId = reservation.AccommodationId;
            Accommodations = accommodation;
            GuestId = reservation.GuestId;
            InitialDate = reservation.InitialDate;
            EndDate = reservation.EndDate;
            DaysToStay = reservation.DaysToStay;
            NumberOfGuests = reservation.NumberOfGuests;
            Location = location;
            Owner = owner;
           // Owner = owner;
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
