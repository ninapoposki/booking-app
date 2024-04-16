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
    public class GuestDTO
    {
        public int id;
        public int Id
        {
            get { return id; }
            set { if (id != value) { id = value; OnPropertyChanged("Id"); } }
        }
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set { if (value != firstName) { firstName = value; OnPropertyChanged("FirstName"); } }
        }
        public string lastName;
        public string LastName
        {
            get { return lastName; }
            set { if (value != lastName){ lastName = value;OnPropertyChanged("LastName"); } }
        }
        public string phoneNumber;
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { if (value != phoneNumber){ phoneNumber = value; OnPropertyChanged("PhoneNumber");} }
        }
        public string email;
        public string Email
        {
            get { return email; }
            set { if (value != email){ email = value; OnPropertyChanged("Email"); } }
        }
        private User user { get; set; }
        public User User{
            get { return user; }
            set { if (value != user){ user = value; OnPropertyChanged("User");} }
        }
        private int userId { get; set; }
        public int UserId
        {
            get { return userId; }
            set {  if (value != userId){ userId = value; OnPropertyChanged("UserId");} }
        }
        public GuestDTO() { }
        public GuestDTO(Guest guest){
            Id = guest.Id;
            FirstName = guest.FirstName;
            LastName=guest.LastName;
            PhoneNumber = guest.PhoneNumber;
            Email = guest.Email;
            User=guest.User; 
            UserId = guest.UserId;
        }
        public Guest ToGuest(){
            var guest = new Guest();
            guest.Id = this.Id;
            guest.FirstName = this.FirstName;
            guest.LastName = this.LastName;
            guest.PhoneNumber = this.PhoneNumber;
            guest.Email = this.Email;
            guest.UserId = this.UserId;
            return guest;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null){
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}