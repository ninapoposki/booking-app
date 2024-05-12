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
    public class RenovationRecommendationDTO: INotifyPropertyChanged
    {
        public int Id { get; set; }
        private int reservationId;
        public int ReservationId
        {
            get { return reservationId; }
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
            get { return ownerId; }
            set
            {
                if (value != ownerId)
                {
                    ownerId = value;
                    OnPropertyChanged("OwnerId");
                }
            }
        }
        private int recommendationLevel;
        public int RecommendationLevel
        {
            get { return recommendationLevel; }
            set
            {
                if (value != recommendationLevel)
                {
                    recommendationLevel = value;
                    OnPropertyChanged("RecommendationLevel");
                }
            }
        }
        private string recommendationComment;
        public string RecommendationComment
        {
            get { return recommendationComment; }
            set
            {
                if (value != recommendationComment)
                {
                    recommendationComment = value;
                    OnPropertyChanged("RecommendationComment");
                }
            }
        }
        //mislimd a i ovo dvoje ne treba
        public OwnerDTO Owner { get; set; }
        public AccommodationReservationDTO AccommodationReservation { get; set; }
        public RenovationRecommendationDTO() { }
        public RenovationRecommendationDTO(RenovationRecommendation renovationRecommendation)
        {
            this.Id = renovationRecommendation.Id;
            this.ReservationId = renovationRecommendation.ReservationId;
            this.OwnerId = renovationRecommendation.OwnerId;
            this.RecommendationLevel = renovationRecommendation.RecommendationLevel;
            this.RecommendationComment = renovationRecommendation.RecommendationComment;
        }
        public RenovationRecommendation ToRenovationRecommendation()
        {
            var renovationRecommendation = new RenovationRecommendation();
            renovationRecommendation.Id = this.Id;
            renovationRecommendation.ReservationId = this.ReservationId;
            renovationRecommendation.OwnerId = this.OwnerId;
            renovationRecommendation.RecommendationLevel = this.RecommendationLevel;
            renovationRecommendation.RecommendationComment = this.RecommendationComment;
            return renovationRecommendation;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
