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
    public class TourReservationDTO : INotifyPropertyChanged
    {

        public int Id { get; set; }

        public int TourStartDateId { get; set; }

        public int UserId {  get; set; }
       

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
                TourStartDateId = tourReservation.TourStartDateId;
                UserId= tourReservation.UserId;
                GuestsNumber=tourReservation.GuestsNumber;
                

            }

            public TourReservation ToTourReservation()
            {
                return new TourReservation(Id,TourStartDateId,UserId,guestsNumber);
            }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

       
    }
}
