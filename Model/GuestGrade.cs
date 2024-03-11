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
       public int GuestID {  get; set; }
       public int MaxDays {  get; set; }
       public int Cleanless {  get; set; }
       public int RulesFollowing {  get; set; }
       public string Comment {  get; set; }

        public GuestGrade() { } 
        public GuestGrade(int id, int guestID, int maxDays, int cleanless, int rulesFollowing, string comment)
        {
            Id = id;
            GuestID = guestID;
            MaxDays = maxDays;
            Cleanless = cleanless;
            RulesFollowing = rulesFollowing;
            Comment = comment;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                GuestID.ToString(),
                MaxDays.ToString(),
                Cleanless.ToString(),
                RulesFollowing.ToString(),
                Comment

            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            GuestID = int.Parse(values[1]);
            MaxDays = int.Parse(values[2]);
            Cleanless = int.Parse(values[3]);
            RulesFollowing = int.Parse(values[4]);
            Comment = values[5];
        }
    }
}
