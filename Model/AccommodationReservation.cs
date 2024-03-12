using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BookingApp.Model
{
    public class AccommodationReservation:ISerializable
    {
        public int Id {  get; set; }
        public Accommodation Accommodation { get; set; } //ili direktno iz accommodation
        public int AccommodationId { get; set; }//nisam sigurna da li je potrebno,svakako korisnik to ne sme da unese,ali zbog uvezivanja
        public DateOnly InitialDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int DaysToStay { get; set; }

        public AccommodationReservation() 
        {
            Accommodation = new Accommodation(); //nisam sig jel treba ovo
        
        }

        public AccommodationReservation(int id,Accommodation accommodation,int acommodationId,DateOnly initDate,DateOnly endD,int stayDays)
        {
            Id = id;
            Accommodation = accommodation; //da li ti ovo treba ovde ili da radis sve preko id-a
            AccommodationId= acommodationId;
            InitialDate = initDate;
            EndDate = endD;
            DaysToStay = stayDays;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AccommodationId = int.Parse(values[1]); //ilir adi direktno po id ili accommodation id
            //ili samo Accommodation.Id.Parse?
            InitialDate = DateOnly.Parse(values[2]);
            EndDate= DateOnly.Parse(values[3]);
            DaysToStay = int.Parse(values[4]);
        }

      public string[] ToCSV()
      {
            string[] csvValues =
            {
                Id.ToString(),
                AccommodationId.ToString(),
                InitialDate.ToString(),
                EndDate.ToString(),
                DaysToStay.ToString()

            };
            return csvValues;   
      }
       


    }
 
}
