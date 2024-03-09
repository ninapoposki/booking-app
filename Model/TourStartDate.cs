using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;


namespace BookingApp.Model
{
    public class TourStartDate : ISerializable
    {
        public int Id {  get; set; }
        public int TourId { get; set; }
        public DateTime StartTime { get; set; }
        public TourStartDate() { }

        public TourStartDate(int id, int tourId, DateTime startTime)
        {
            Id = id;
            TourId = tourId;
            StartTime = startTime;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            StartTime = DateTime.ParseExact(values[1], "dd/MM/yyyy hh:mm", CultureInfo.InvariantCulture);
            TourId= Convert.ToInt32(values[2]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                StartTime.ToString("dd/MM/yyyy hh:mm"),
                TourId.ToString(),
            };

            return csvValues;
        }
    }
}
