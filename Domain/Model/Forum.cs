using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public enum ActivationType { ACTIVE, DISABLED};
    public enum ForumStatusType { OLD, NEW};

    public class Forum:ISerializable
    {
        public int Id { get; set; }
        public int GuestId {  get; set; }
        public Guest Guest { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public List<ForumComment> Comments { get; set; }
       public ActivationType ActivationType { get; set; }
       public ForumStatusType ForumStatus { get; set; }

        public Forum()
        {
            Location=new Location();
            Guest=new Guest();
        }

        public Forum(int id, int guestId, int locationId, ActivationType activationType, ForumStatusType forumStatus, List<ForumComment> comments)
        {
            Id = id;
            GuestId = guestId;
            LocationId = locationId;
            ActivationType = activationType;
            ForumStatus = forumStatus;
            Comments = comments;
            
        }
        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            GuestId = int.Parse(values[1]);
            LocationId = int.Parse(values[2]);
            if (values[3] == "ACTIVE")
            {
                ActivationType = ActivationType.ACTIVE;
            }
            else
            {         
                ActivationType = ActivationType.DISABLED;
            }
            if (values[4] == "OLD")
            {
                ForumStatus = ForumStatusType.OLD;
            }
            else
            {          
                ForumStatus = ForumStatusType.NEW;
            }

        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                GuestId.ToString(),
                LocationId.ToString(),
                ActivationType.ToString(),
                ForumStatus.ToString()
            };
            return csvValues;
        }
    }
}
