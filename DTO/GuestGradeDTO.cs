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
    public class GuestGradeDTO : INotifyPropertyChanged
    {
        public int id = -1;
         public int Id  {
            get { return id; }
            set { if (id != value){
                    id = value;
                    OnPropertyChanged("Id");
                  }
            }
        }
        private int reservationId;
        public int ReservationId {
            get { return reservationId; }
            set { if (reservationId != value) {
                    reservationId = value;
                    OnPropertyChanged("ReservationId");
                  }
            }
        }
        private int guestId;
        public int GuestId{
            get { return guestId; }
            set { if (guestId != value) {
                    guestId = value;
                    OnPropertyChanged("GuestId");
                  }
            }
        }
        private int cleanless;
        public int Cleanless{
            get { return cleanless; }
            set { if (cleanless != value) {
                    cleanless = value;
                    OnPropertyChanged("Cleanless");
                  }
            }
        }
        private int rulesFollowing;
        public int RulesFollowing{
            get { return rulesFollowing; }
            set{ if (rulesFollowing != value) {
                    rulesFollowing = value;
                    OnPropertyChanged("RulesFollowing");
                 }
            }
        }
        private String comment;
        public String Comment {
            get { return comment; }
            set { if (comment != value) {
                    comment = value;
                    OnPropertyChanged("Comment");
                   }
            }
        }
        public AccommodationReservationDTO AccommodationReservation { get; set; }
        public GuestGradeDTO(){  }
        public GuestGradeDTO(GuestGrade guestGrade){
            this.Id = guestGrade.Id;
            this.ReservationId = guestGrade.ReservationId;
            this.GuestId = guestGrade.GuestId;
            this.Cleanless = guestGrade.Cleanless;
            this.RulesFollowing = guestGrade.RulesFollowing;
            this.Comment = guestGrade.Comment;
        }
        public GuestGrade ToGuestGrade() {
            return new GuestGrade(id, reservationId, guestId, cleanless, rulesFollowing, comment);
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}