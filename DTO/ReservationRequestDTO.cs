using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class ReservationRequestDTO : INotifyPropertyChanged {
        public int id;
        public int Id{
            get { return id; }
            set {if (id != value) {  id = value;  OnPropertyChanged("Id"); }
            }
        }
        private int reservationId;
        public int ReservationId {
            get { return reservationId; }
            set { if (reservationId != value) {  reservationId = value;  OnPropertyChanged("ReservationId"); }
            }
        }
        private DateTime newInitialDate = DateTime.Now;
        public DateTime NewInitialDate {
            get {  return newInitialDate;  }
            set { if (value != newInitialDate) {  newInitialDate = value;   OnPropertyChanged("NewInitialDate");   }
            }
        }
        private DateTime newEndDate = DateTime.Now;
        public DateTime NewEndDate {
            get { return newEndDate; }
            set {if (value != newEndDate) { newEndDate = value; OnPropertyChanged("NewEndDate"); }
            }
        }
        private RequestStatus requestStatus;
        public RequestStatus RequestStatus {
            get { return requestStatus; }
            set { if (requestStatus != value) { requestStatus = value; OnPropertyChanged("RequestStatus"); }
            }
        }
        private String comment="";
        public String Comment {
            get { return comment; }
            set{ if (comment != value) { comment = value; OnPropertyChanged("Comment"); }
            }
        }
        public ObservableCollection<ImageDTO> Images { get; set; } = new ObservableCollection<ImageDTO>();
        public string Message { get; set; }
        public AccommodationReservationDTO AccommodationReservation { get; set; }
        private AccommodationDTO accommodation;
        public AccommodationDTO Accommodation {
            get{ return accommodation; }
            set{if (value != accommodation) { accommodation = value; OnPropertyChanged("Accommodation"); }
            }
        }
        public ReservationRequestDTO() { }
        public ReservationRequestDTO(ReservationRequest reservationRequest) {
            this.Id = reservationRequest.Id;
            this.ReservationId = reservationRequest.ReservationId;
            this.NewInitialDate = reservationRequest.NewInitialDate;
            this.newEndDate = reservationRequest.NewEndDate;
            this.RequestStatus = reservationRequest.RequestStatus;
            this.Comment = reservationRequest.Comment;
        }
        public ReservationRequest ToReservationRequest() {
            var reservationRequest = new ReservationRequest();
            reservationRequest.Id = this.Id; 
            reservationRequest.ReservationId = this.ReservationId;
            reservationRequest.NewInitialDate = this.NewInitialDate;
            reservationRequest.NewEndDate = this.NewEndDate;
            reservationRequest.RequestStatus = this.RequestStatus;
            reservationRequest.Comment = this.Comment;
            return reservationRequest;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null){
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}