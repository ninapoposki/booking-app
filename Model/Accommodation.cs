using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public enum AccommodationType { APARTMENT, HOUSE, CABIN  };
    public class Accommodation : ISerializable
    {
        public int Id {  get; set; }
        public string Name {  get; set; }
        public Location Location {  get; set; } 
        public int IdLocation {  get; set; }
        public AccommodationType AccommodationType {  get; set; }
        public int Capacity {  get; set; }
        public int MinStayDays {  get; set; }
        public int CancellationPeriod {  get; set; }
        public List<Image> Images {  get; set; }

        public Accommodation() {
            Images = new List<Image>();
        }

        public Accommodation(int id, string name, Location location, int idLocation, AccommodationType type, int capacity, int minStayDays, int cancellationPeriod, List<Image> images)
        {
            Id = id;
            Name = name;
            Location = location;
            IdLocation = idLocation;
            AccommodationType = type;
            Capacity = capacity;
            MinStayDays = minStayDays;
            CancellationPeriod = cancellationPeriod;
            Images = images;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
             {
                Id.ToString(),
                Name,
                IdLocation.ToString(),
                AccommodationType.ToString(),
                Capacity.ToString(),
                MinStayDays.ToString(),
                CancellationPeriod.ToString()

            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Name = values[1];
            IdLocation = int.Parse(values[2]);
            if (values[3] == "APARTMENT")
            {
                AccommodationType = AccommodationType.APARTMENT;
            }
            else if (values[3] == "HOUSE")
            {
                AccommodationType = AccommodationType.HOUSE;
            }
            else if (values[3] == "CABIN")
            {
                AccommodationType = AccommodationType.CABIN;
            }
            Capacity = int.Parse(values[4]);
            MinStayDays = int.Parse(values[5]);
            CancellationPeriod = int.Parse(values[6]);

        }
    }
}
