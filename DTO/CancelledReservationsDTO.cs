using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class CancelledReservationsDTO:INotifyPropertyChanged
    {
        public int Id { get; set; }
        private int accommodationId;
        public int AccommodationId
        {
            get{  return accommodationId; }
            set{
                if (value != accommodationId){
                    accommodationId = value;
                    OnPropertyChanged("AccommodationId");
                }
            }
        }
        public Accommodation Accommodations { get; set; }
        private DateTime initialDate; 
        public DateTime InitialDate
        {
            get{ return initialDate; }
            set{
                if (value != initialDate){
                    initialDate = value;
                    OnPropertyChanged("InitialDate");
                }
            }
        }
        private DateTime endDate;
        public DateTime EndDate
        {
            get{ return endDate; }
            set{
                if (value != endDate){
                    endDate = value;
                    OnPropertyChanged("EndDate");
                }
            }
        }
        public CancelledReservationsDTO(){ }
        public CancelledReservations ToCancelledReseervations()
        {
            var cancelledReservations = new CancelledReservations();
            cancelledReservations.Id = this.Id;
            cancelledReservations.AccommodationId = this.AccommodationId;
            cancelledReservations.InitialDate = this.InitialDate;
            cancelledReservations.EndDate = this.EndDate;
            return cancelledReservations;
        }
        public CancelledReservationsDTO(CancelledReservations cancelledReservation)
        {
            Id = cancelledReservation.Id;
            AccommodationId = cancelledReservation.AccommodationId;
            InitialDate = cancelledReservation.InitialDate;
            EndDate = cancelledReservation.EndDate;
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
