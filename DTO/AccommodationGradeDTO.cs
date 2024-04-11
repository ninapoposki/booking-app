using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class AccommodationGradeDTO: INotifyPropertyChanged
    {
        //ili klasicno sa image uradi 
        public ObservableCollection<ImageDTO> Images { get; set; } = new ObservableCollection<ImageDTO>();

        public int Id { get; set; }

        //acciommodationReservationDTO accommodationReservation
        private int reservationId;
        public int ReservationId
        {
            get
            {
                return reservationId;
            }
            set
            {
                if (value != reservationId)
                {
                    reservationId = value;
                    OnPropertyChanged("ReservationId");
                }
            }
        }

        private int ownerId;

        public int OwnerId
        {
            get
            {
                return ownerId;
            }
            set
            {
                if (value != ownerId)
                {
                    ownerId = value;
                    OnPropertyChanged("OwnerId");
                }
            }
        }

        private int cleanliness;
        public int Cleanliness
        {
            get
            {
                return cleanliness;
            }
            set
            {
                if (value != cleanliness)
                {
                    cleanliness = value;
                    OnPropertyChanged("Cleanliness");
                }

            }
        }


        private int correctness;
        public int Correctness
        {
            get
            {
                return correctness;
            }
            set
            {
                if (value != correctness)
                {
                    correctness = value;
                    OnPropertyChanged("Correctness");
                }

            }
        }

        private string comment;
        public string Comment
        {
            get
            {
                return comment;
            }
            set
            {
                if (value != comment)
                {
                    comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }
        public AccommodationGradeDTO()
        {

        }
        public AccommodationGradeDTO(AccommodationGrade accommodationGrade)
        {
            this.Id = accommodationGrade.Id;
            this.ReservationId = accommodationGrade.ReservationId;
            this.OwnerId = accommodationGrade.OwnerId;
            this.Cleanliness = accommodationGrade.Cleanliness;
            this.Correctness = accommodationGrade.Correctness;
            this.Comment = accommodationGrade.Comment;
            //Image  ne ide


        }

        public AccommodationGrade ToAccommodationGrade()
        {
            var accommodationGrade = new AccommodationGrade();

            accommodationGrade.Id = this.Id;
            accommodationGrade.ReservationId = this.ReservationId;
            accommodationGrade.OwnerId = this.OwnerId;
            accommodationGrade.Cleanliness = this.Cleanliness;
            accommodationGrade.Correctness = this.Correctness;
            accommodationGrade.Comment = this.Comment;

            return accommodationGrade;
        }




        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
