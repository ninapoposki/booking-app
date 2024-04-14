using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class CancelledReservations:ISerializable
    {

        public int Id { get; set; }
        public Accommodation Accommodation { get; set; } //ne znam da li treba
        public int AccommodationId { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime EndDate { get; set; }
       

        public CancelledReservations()
        {
           // Accommodation = new Accommodation();
        }

        public CancelledReservations(int id, int acommodationId, DateTime initDate, DateTime endD)
        {
            Id = id;
            AccommodationId = acommodationId;
            InitialDate = initDate;
            EndDate = endD;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AccommodationId = int.Parse(values[1]);
            InitialDate = DateTime.Parse(values[2]);
            EndDate = DateTime.Parse(values[3]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                InitialDate.ToString(),
                EndDate.ToString()

            };
            return csvValues;
        }
    }
}
