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
    class RenovationRecommendationDTO: INotifyPropertyChanged
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
        private string recommendationComment = "";
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
        public AccommodationReservationDTO AccommodationReservation { get; set; }
        public RenovationRecommendationDTO() { }
        public RenovationRecommendationDTO(RenovationRecommendation renovationRecommendation)
        {
            this.Id = renovationRecommendation.Id;
            this.ReservationId = renovationRecommendation.ReservationId;
            this.RecommendationLevel = renovationRecommendation.RecommendationLevel;
            this.RecommendationComment = renovationRecommendation.RecommendationComment;
        }
        public RenovationRecommendation ToRenovationRecommendation()
        {
            var renovationRecommendation = new RenovationRecommendation();
            renovationRecommendation.Id = this.Id;
            renovationRecommendation.ReservationId = this.ReservationId;
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
