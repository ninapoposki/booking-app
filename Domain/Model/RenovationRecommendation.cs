﻿using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class RenovationRecommendation:ISerializable
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }
        public int ReservationId { get; set; }
        public AccommodationReservation AccommodationReservation { get; set; }
        public int RecommendationLevel { get; set; }
        public string RecommendationComment { get; set; }

        public RenovationRecommendation()
        {
            Owner = new Owner();
            AccommodationReservation = new AccommodationReservation();
        }

        public RenovationRecommendation(int id, int reservationId, int ownerId, int recommendationLevel, string recommendationComment)
        {
            Id = id;
            ReservationId = reservationId;
            OwnerId= ownerId;
            RecommendationLevel = recommendationLevel;
            RecommendationComment = recommendationComment;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
             {
                  Id.ToString(),
                  ReservationId.ToString(),
                  OwnerId.ToString(),
                  RecommendationLevel.ToString(),
                  RecommendationComment

              };
            return csvValues;

        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            ReservationId = int.Parse(values[1]);
            OwnerId = int.Parse(values[2]);
            RecommendationLevel = int.Parse(values[3]);
            RecommendationComment = values[4];
        }
    }
}
