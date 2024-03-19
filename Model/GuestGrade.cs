using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class GuestGrade : ISerializable
    {
        public int Id { get; set; }

        public int GuestId { get; set; } //??

        public int ReservationId {  get; set; }
        public AccommodationReservation Reservation;
        public int MaxDays {  get; set; }

        public int Cleanless {  get; set; }
        public int RulesFollowing {  get; set; }
        //public static readonly string[] CategoryNames = { "Cleanness", "Following the rules" };
        public string Comment {  get; set; }

        //public Dictionary<string, int> Grades;
        //public List<int>Grades { get; set; }


        public GuestGrade()
        {
           
        }

        public GuestGrade(int id, int reservationId, int guestId, int cleanless, int rulesFollowing, string comment)
        {
            Id = id;
            ReservationId = reservationId;
            GuestId = guestId;
            Cleanless = cleanless;
            RulesFollowing = rulesFollowing;
            Comment = comment;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
             {
                  Id.ToString(),
                  ReservationId.ToString(),
                  GuestId.ToString(),
                  Cleanless.ToString(),
                  RulesFollowing.ToString(),
                  Comment

              };
            return csvValues;

        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            ReservationId = int.Parse(values[1]);
            GuestId = int.Parse(values[2]);
            Cleanless = int.Parse(values[3]);
            RulesFollowing = int.Parse(values[4]);
            Comment = values[5];
        }


    }
}
