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

        public bool HasStarted {  get; set; }

        public bool HasFinished {  get; set; }
        public TourStartDate() { }

        public TourStartDate(int id,int tourId, DateTime startTime)
        {
            Id = id;
            TourId = tourId;
            StartTime = startTime;
        }

        public TourStartDate( int tourId, DateTime startTime)
        {
            
            TourId = tourId;
            StartTime = startTime;
            HasStarted = false;
            HasFinished = false;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            StartTime = DateTime.ParseExact(values[1], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            TourId= Convert.ToInt32(values[2]);
            HasStarted = Convert.ToBoolean(values[3]);
            HasFinished = Convert.ToBoolean(values[4]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                StartTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                TourId.ToString(),
                HasStarted.ToString(),
                HasFinished.ToString()
            };

            return csvValues;
        }
    }
}
