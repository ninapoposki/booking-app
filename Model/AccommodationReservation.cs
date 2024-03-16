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
        public int GuestId {  get; set; }
        public Guest Guest { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DaysToStay { get; set; }
        public int NumberOfGuests { get; set; }

        public AccommodationReservation() 
        {
            Accommodation = new Accommodation(); //nisam sig jel treba ovo
        
        }

        public AccommodationReservation(int id,int guestId,Accommodation accommodation,int acommodationId,DateTime initDate,DateTime endD,int stayDays,int numberOfGuests)
        {
            Id = id;
            GuestId = guestId;
            Accommodation = accommodation; //da li ti ovo treba ovde ili da radis sve preko id-a
            AccommodationId= acommodationId;
            InitialDate = initDate;
            EndDate = endD;
            DaysToStay = stayDays;
            NumberOfGuests = numberOfGuests;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            AccommodationId = int.Parse(values[1]); //ilir adi direktno po id ili accommodation id
            //ili samo Accommodation.Id.Parse?
            GuestId= int.Parse(values[2]);
            InitialDate = DateTime.Parse(values[3]);
            EndDate= DateTime.Parse(values[4]);
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
