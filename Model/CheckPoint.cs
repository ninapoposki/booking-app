using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class CheckPoint : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int tourId { get; set; }
        public CheckPoint() { }

        public CheckPoint(int id, string name, int tourId)
        {
            Id = id;
            Name = name;
            this.tourId = tourId;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            tourId = Convert.ToInt32(values[2]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Name,
                tourId.ToString()
            };

            return csvValues;
        }
    }
}
