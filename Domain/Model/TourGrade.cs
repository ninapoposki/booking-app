using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class TourGrade : ISerializable
    {
        public int Id { get; set; }
        public int TourReservationId { get; set; }
        public int GuideKnowledge { get; set; }

        public int LanguageKnowledge { get; set; }  

        public int TourAtrractions { get; set; }

        public string Comment { get; set; }

        public TourGrade() { }

        public TourGrade(int id, int tourReservationId, int guideKnowledge, int languageKnowledge, int tourAtrractions, string comment)
        {
            Id = id;
            TourReservationId = tourReservationId;
            GuideKnowledge = guideKnowledge;
            LanguageKnowledge = languageKnowledge;
            TourAtrractions = tourAtrractions;
            Comment = comment;
         
        }
    
        public string[] ToCSV()
        {
            string[] csvValues =
            {

                Id.ToString(),
                TourReservationId.ToString(),
                GuideKnowledge.ToString(),
                LanguageKnowledge.ToString(),
                TourAtrractions.ToString(),
                Comment

            };
            return csvValues;

        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            TourReservationId= int.Parse(values[1]);
            GuideKnowledge= int.Parse(values[2]);
            LanguageKnowledge= int.Parse(values[3]);
            TourAtrractions= int.Parse(values[4]);
            Comment = values[5];
        }
    }
}
