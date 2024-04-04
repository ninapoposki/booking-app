using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BookingApp.Domain.Model
{
    public class AccommodationReservation:ISerializable
    {
        public int Id {  get; set; }
        public Accommodation Accommodation { get; set; } 
        public int AccommodationId { get; set; }
        public int GuestId {  get; set; }
        public Guest Guest { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DaysToStay { get; set; }
        public int NumberOfGuests { get; set; }

        public AccommodationReservation() 
        {
            Accommodation = new Accommodation(); 
            Guest=new Guest();
        
        }

        public AccommodationReservation(int id,int guestId,int acommodationId,DateTime initDate,DateTime endD,int stayDays,int numberOfGuests)
        {
            Id = id;
            GuestId=guestId;
            AccommodationId= acommodationId;
            InitialDate = initDate;
            EndDate = endD;
            DaysToStay = stayDays;
            NumberOfGuests = numberOfGuests;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AccommodationId = int.Parse(values[1]);
            GuestId = int.Parse(values[2]);
            InitialDate = DateTime.Parse(values[3]);
            EndDate = DateTime.Parse(values[4]);
            DaysToStay = int.Parse(values[5]);
            NumberOfGuests= int.Parse(values[6]);

        }

      public string[] ToCSV()
      {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                GuestId.ToString(),
                InitialDate.ToString(),
                EndDate.ToString(),
                DaysToStay.ToString(),
                NumberOfGuests.ToString()

            };
            return csvValues;   
      }
    }
}
