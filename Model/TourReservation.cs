using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class TourReservation : ISerializable
    {
        public int Id { get; set; }
        public int TourId { get; set; }
        
        public DateOnly TourDateTime { get; set; }
        public Tour Tour { get; set; }
        public User User { get; set; }

        public int GuestsNumber { get; set; }   


        public TourReservation() { }

        public TourReservation(int id, int tourId, DateOnly tourDateTime, int guestsNumber)
        {
            Id = id;
            TourId = tourId;
            TourDateTime = tourDateTime;
            GuestsNumber = guestsNumber;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            TourId= Convert.ToInt32(values[1]);
            TourDateTime= DateOnly.Parse(values[2]);
            GuestsNumber= Convert.ToInt32(values[3]);

        }

        public string[] ToCSV()
        {
            string[] csvValues={


                Id.ToString(),
                TourId.ToString(),
                TourDateTime.ToString(),
                GuestsNumber.ToString()


            };

            return csvValues;
        }
    }
}
