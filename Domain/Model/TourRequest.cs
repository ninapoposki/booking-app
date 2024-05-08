using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace BookingApp.Domain.Model
{
    public enum State { PENDING,ACCEPTED,EXPIRED };
    public class TourRequest:ISerializable
    {
        public int Id { get; set; }
        public int LocationId {  get; set; }
        public int LanguageId {  get; set; }
        public string Description {  get; set; }
        public State State { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set;}
        public DateTime ChoosenDate { get; set; }
        public bool IsNotified {  get; set; }

        public TourRequest() { }

        public TourRequest(int id, int locationId, int languageId, string description, State state, DateOnly startDate, DateOnly endDate,DateTime choosenDate,bool isNotified)
        {
            Id = id;
            LocationId = locationId;
            LanguageId = languageId;
            Description = description;
            State = state;
            StartDate = startDate;
            EndDate = endDate;
            ChoosenDate = choosenDate;
            IsNotified = isNotified;
        }
        public TourRequest(int locationId, int languageId, string description, DateOnly startDate, DateOnly endDate)
        {
            LocationId = locationId;
            LanguageId = languageId;
            Description = description;
            State = State.PENDING;
            StartDate = startDate;
            EndDate = endDate;
            ChoosenDate = DateTime.Now;
            IsNotified = false;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            LanguageId = Convert.ToInt32(values[1]);
            LocationId = Convert.ToInt32(values[2]);
            Description = values[3];
            StartDate = DateOnly.ParseExact(values[4], "dd/MM/yyyy");
            EndDate = DateOnly.ParseExact(values[5], "dd/MM/yyyy");
            ChoosenDate = DateTime.ParseExact(values[6],"dd/MM/yyyy",CultureInfo.InvariantCulture);
            IsNotified = Convert.ToBoolean(values[7]);
            if (values[8] == "ACCEPTED") { State = State.ACCEPTED; }
            else if (values[8] == "PENDING") { State = State.PENDING; }
            else { State=State.EXPIRED; }
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                LanguageId.ToString(),
                LocationId.ToString(),
                Description,
                StartDate.ToString("dd/MM/yyyy"),
                EndDate.ToString("dd/MM/yyyy"),
                ChoosenDate.ToString("dd/MM/yyyy",CultureInfo.InvariantCulture),
                IsNotified.ToString(),
                State.ToString()
            };
            return csvValues;
        }
    }
}
