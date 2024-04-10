using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class AccommodationGrade
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int ReservationId { get; set; }
       // public AccommodationReservation Reservation; //mislim da nam ovo ne treba
       //Owner owner i slicno mislim da nam ne treba
        public int Cleanliness { get; set; }
        public int Correctness { get; set; }
        public string Comment { get; set; }

        List<Image> Images { get; set; }

        public AccommodationGrade()
        {

        }

        public AccommodationGrade(int id, int reservationId, int ownerId, int cleanliness, int correctness, string comment)
        {
            Id = id;
            ReservationId = reservationId;
            OwnerId = ownerId;
            Cleanliness = cleanliness;
            Correctness = correctness;
            Comment = comment;
            Images=new List<Image>();
        }

        public string[] ToCSV()
        {
            string[] csvValues =
             {
                  Id.ToString(),
                  ReservationId.ToString(),
                  OwnerId.ToString(),
                  Cleanliness.ToString(),
                  Correctness.ToString(),
                  Comment

              };
            return csvValues;

        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            ReservationId = int.Parse(values[1]);
            OwnerId = int.Parse(values[2]);
            Cleanliness = int.Parse(values[3]);
            Correctness = int.Parse(values[4]);
            Comment = values[5];
        }




    }
}
