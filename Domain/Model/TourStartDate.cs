using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;



namespace BookingApp.Domain.Model
{
    public enum TourStatus { INACTIVE, ACTIVE, FINISHED, CANCELED };

    public class TourStartDate : ISerializable
    {
        public int Id {  get; set; }
        public int TourId { get; set; }
        public DateTime StartTime { get; set; }

        public TourStatus TourStatus { get; set; }
        public int CurrentCheckPointId {  get; set; }
        public TourStartDate() { }

        public TourStartDate(int id,int tourId, DateTime startTime,TourStatus tourStatus,int currentCheckPointId)
        {
            Id = id;
            TourId = tourId;
            StartTime = startTime;
            TourStatus = tourStatus;
            CurrentCheckPointId = currentCheckPointId;
        }

        public TourStartDate( int tourId, DateTime startTime)
        {
            TourId = tourId;
            StartTime = startTime;
            TourStatus = TourStatus.INACTIVE;
            CurrentCheckPointId = -1;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            StartTime = DateTime.ParseExact(values[1], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            TourId= Convert.ToInt32(values[2]);
            if (values[3] == "INACTIVE") { TourStatus = TourStatus.INACTIVE; }
            else if (values[3] == "ACTIVE") { TourStatus = TourStatus.ACTIVE; }
            else if (values[3] == "FINISHED") { TourStatus = TourStatus.FINISHED; }
            else { TourStatus= TourStatus.CANCELED; }
            CurrentCheckPointId= Convert.ToInt32(values[4]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                StartTime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                TourId.ToString(),
                TourStatus.ToString(),
                CurrentCheckPointId.ToString()
            };

            return csvValues;
        }
    }
}
