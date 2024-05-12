using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.Domain.Model
{
    public class AccommodationRenovation : ISerializable
    {
        public int Id { get; set; }
        public int AccommodationId {get; set;}
        public DateTime InitialDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }

        public AccommodationRenovation()
        {
            
        }

      public AccommodationRenovation(int id, int accommodationId, DateTime initialDate, DateTime endDate, int duration, string description)
        {
            Id = id;
            AccommodationId = accommodationId;
            InitialDate = initialDate;
            InitialDate = endDate;
            Duration = duration;
            Description = description;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
             {
                  Id.ToString(),
                  AccommodationId.ToString(),
                  InitialDate.ToString(),
                  EndDate.ToString(),
                  Duration.ToString(),
                  Description

              };
            return csvValues;

        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AccommodationId = int.Parse(values[1]);
            InitialDate = DateTime.Parse(values[2]);
            EndDate = DateTime.Parse(values[3]);
            Duration = int.Parse(values[4]);
            Description = values[5];
        }



    }
}
