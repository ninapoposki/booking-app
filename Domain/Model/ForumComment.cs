using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BookingApp.Serializer;

namespace BookingApp.Domain.Model
{
    public class ForumComment : ISerializable
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public int ForumId { get; set; }
        public string Comment { get; set; }
        public int ReportNumber {  get; set; }
        public DateTime CreationDate { get; set; }


        public  ForumComment() { }

        public ForumComment(int id, int userId, int forumId, string comment, int reportNumber, DateTime creationDate)
        {
            Id = id;
            UserId = userId;
            ForumId = forumId;
            Comment = comment;
            ReportNumber = reportNumber;
            CreationDate = creationDate;
        }

        public string[] ToCSV()
        {
            string[] csvValues =
             {
                Id.ToString(),
                UserId.ToString(),
                ForumId.ToString(),
                Comment,
                ReportNumber.ToString(),
                CreationDate.ToString()

            };

            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            UserId = int.Parse(values[1]);
            ForumId = int.Parse(values[2]);
            Comment = values[3];
            ReportNumber = int.Parse(values[4]);
            CreationDate = DateTime.Parse(values[5]);
        }

    }
}
