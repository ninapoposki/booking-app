using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class TourReservation : ISerializable
    {
        public int Id { get; set; }
       
        public int TourStartDateId {  get; set; } 
        public int UserId { get; set; }

        public int GuestsNumber { get; set; }   

        public TourReservation() { }

        public TourReservation(int id, int tourStartDateId,int userId, int guestsNumber)
        {
            Id = id;
            TourStartDateId = tourStartDateId;
            UserId = userId;
            GuestsNumber = guestsNumber;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourStartDateId= Convert.ToInt32(values[1]);
            UserId = Convert.ToInt32(values[2]);
            GuestsNumber= Convert.ToInt32(values[3]);

        }

        public string[] ToCSV()
        {
            string[] csvValues={


                Id.ToString(),
                TourStartDateId.ToString(),
                UserId.ToString(),
                GuestsNumber.ToString()


            };

            return csvValues;
        }
    }
}
