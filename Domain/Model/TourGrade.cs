using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public enum Validity { YES, NO };   
    public class TourGrade : ISerializable
    {
        public int Id { get; set; }
        public int TourReservationId { get; set; }
        public int GuideKnowledge { get; set; }

        public int LanguageKnowledge { get; set; }  
        public Validity Validity { get; set; }
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
            Validity=Validity.YES;
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
                Comment,
                Validity.ToString(),
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
            if (values[6] == "YES") { Validity= Validity.YES; }
            else {  Validity= Validity.NO; }
        }
    }
}
