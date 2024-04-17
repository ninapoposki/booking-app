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
    public class OwnerDTO{
        public int id;
        public int Id {
            get { return id; }
            set { if (value != id) {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }
        private string firstName;
        public string FirstName   {
            get { return firstName; }
            set { if (value != firstName) {
                    firstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }
        public string lastName;
        public string LastName {
            get { return lastName; }
            set { if (value != lastName) {
                    lastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }
        public string phoneNumber;
        public string PhoneNumber{
            get { return phoneNumber; }
            set { if (value != phoneNumber) {
                    phoneNumber = value;
                    OnPropertyChanged("PhoneNumber");
                }
            }
        }
        private User user { get; set; }
        public User User {
            get { return user; }
            set { if (value != user){
                    user = value;
                    OnPropertyChanged("User");
                }
            }
        }
        private int userId { get; set; }
        public int UserId {
            get { return userId; }
            set { if (value != userId) {
                    userId = value;
                    OnPropertyChanged("UserId");
                }
            }
        }
        private string role;
        public string Role{
            get { return role; }
            set  {  if (value != role) {
                    role = value;
                    OnPropertyChanged("Role");
                }
            }
        }
        public OwnerDTO() { }
        public OwnerDTO(Owner owner)  {
            Id = owner.Id;
            FirstName = owner.FirstName;
            LastName = owner.LastName;
            PhoneNumber = owner.PhoneNumber;
            User = owner.User;
            UserId = owner.UserId;
            Role = owner.Role;
        }
        public Owner ToOwner() {
            var owner = new Owner();
            owner.Id = this.Id;
            owner.FirstName = this.FirstName;
            owner.LastName = this.LastName;
            owner.PhoneNumber = this.PhoneNumber;
            owner.UserId = this.UserId;
            owner.Role = this.Role;
            return owner;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null){
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
