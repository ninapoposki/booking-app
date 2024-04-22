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
    public class UserDTO {
        public int id;
        public int Id{
            get { return id; }
            set {
                if (value != id)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }
        private string username;
        public string Username {
            get { return username; }
            set {
                if (value != username)
                {
                    username = value;
                    OnPropertyChanged("Username");
                }
            }
        }
        public string password;
        public string Password {
            get { return password; }
            set {
                if (value != password)
                {
                    password = value;
                    OnPropertyChanged("Password");
                }
            }
        }
        public string phoneNumber;
        public string PhoneNumber {
            get { return phoneNumber; }
            set {
                if (value != phoneNumber)
                {
                    phoneNumber = value;
                    OnPropertyChanged("PhoneNumber");
                }
            }
        }
        private UserType userType { get; set; }
        public UserType UserType{
            get { return userType; }
            set{
                if (value != userType)
                {
                    userType = value;
                    OnPropertyChanged("UserType");
                }
            }
        }
        public UserDTO() { }
        public UserDTO(User user){
            Id = user.Id;
            Username = user.Username;
            Password = user.Password;
            UserType = user.UserType;
        }
        public User ToUser(){
            var user = new User();
            user.Id = this.Id;
            user.Username = this.Username;
            user.Password = this.Password;
            user.UserType = this.UserType;
            return user;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}