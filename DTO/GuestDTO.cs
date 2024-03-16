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
    public class GuestDTO
    {
        public int id;
        public int Id
        {
            get { return id; }
            set
            {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        private int accommodationReservationId;
        public int AccommodationReservationId
        {
            get { return accommodationReservationId; }
            set
            {
                if (value != accommodationReservationId)
                {
                    accommodationReservationId = value;
                    OnPropertyChanged("AccommodationReservationId");
                }
            }
        }

        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (value != firstName)
                {
                    firstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }
        public string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                if (value != lastName)
                {
                    lastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }
        public string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                if (value != phoneNumber)
                {
                    phoneNumber = value;
                    OnPropertyChanged("PhoneNumber");
                }
            }
        }

        public string email;
        public string Email
        {
            get { return email; }
            set
            {
                if (value != email)
                {
                    email = value;
                    OnPropertyChanged("Email");
                }
            }
        }




        private User user { get; set; }

        public User User
        {
            get { return user; }
            set
            {
                if (value != user)
                {
                    user = value;
                    OnPropertyChanged("User");
                }
            }
        }

        public GuestDTO() { }

        //d ali GuestDTO da prosledjujem i User user
        public GuestDTO(Guest guest)
        {

            Id = guest.Id;
            FirstName = guest.FirstName;
            LastName=guest.LastName;
            PhoneNumber = guest.PhoneNumber;
            Email = guest.Email;
            User=guest.User; //ovde proveri da li se prosledjuje ili ovako,ako se prosledjueje onda kao po lokaciji radis

        }
        public Guest ToGuest()
        {
            var guest = new Guest();

            guest.Id = this.Id;
            guest.FirstName = this.FirstName;
            // accommodationReservation.Accommodation = this.Accommodation;-ne ide ovo,samo id
            guest.LastName = this.LastName;
            guest.PhoneNumber = this.PhoneNumber;
            guest.Email = this.Email;


            return guest;
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

