using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    class RenovationRecommendation:ISerializable
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        //AccommodationId-mislimd aje lakse ReservationId

        public AccommodationReservation AccommodationReservation { get; set; }
        public int RecommendationLevel { get; set; }
        public string RecommendationComment { get; set; }

        public RenovationRecommendation()
        {
            AccommodationReservation = new AccommodationReservation();
        }

        public RenovationRecommendation(int id, int reservationId, int recommendationLevel, string recommendationComment)
        {
            Id = id;
            ReservationId = reservationId;
            RecommendationLevel = recommendationLevel;
            RecommendationComment = recommendationComment;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
             {
                  Id.ToString(),
                  ReservationId.ToString(),
                  RecommendationLevel.ToString(),
                  RecommendationComment

              };
            return csvValues;

        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            ReservationId = int.Parse(values[1]);
            RecommendationLevel = int.Parse(values[2]);
            RecommendationComment = values[3];
        }
    }
}
