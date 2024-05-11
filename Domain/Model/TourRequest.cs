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
        public int NumberOfTourists {  get; set; }
        public State State { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set;}
        public DateTime ChoosenDate { get; set; }
        public bool IsNotified {  get; set; }

        public TourRequest() { }

        public TourRequest(int id, int locationId, int languageId, string description,int numberOfTourists, State state, DateOnly startDate, DateOnly endDate,DateTime choosenDate,bool isNotified)
        {
            Id = id;
            LocationId = locationId;
            LanguageId = languageId;
            Description = description;
            NumberOfTourists = numberOfTourists;
            State = state;
            StartDate = startDate;
            EndDate = endDate;
            ChoosenDate = choosenDate;
            IsNotified = isNotified;
        }
        public TourRequest(int locationId, int languageId, string description, int numberOfTourists, DateOnly startDate, DateOnly endDate)
        {
            LocationId = locationId;
            LanguageId = languageId;
            Description = description;
            NumberOfTourists = numberOfTourists;
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
            NumberOfTourists = Convert.ToInt32(values[4]);
            StartDate = DateOnly.ParseExact(values[5], "dd/MM/yyyy");
            EndDate = DateOnly.ParseExact(values[6], "dd/MM/yyyy");
            ChoosenDate = DateTime.ParseExact(values[7],"dd/MM/yyyy HH:mm",CultureInfo.InvariantCulture);
            IsNotified = Convert.ToBoolean(values[8]);
            if (values[9] == "ACCEPTED") { State = State.ACCEPTED; }
            else if (values[9] == "PENDING") { State = State.PENDING; }
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
                NumberOfTourists.ToString(),
                StartDate.ToString("dd/MM/yyyy"),
                EndDate.ToString("dd/MM/yyyy"),
                ChoosenDate.ToString("dd/MM/yyyy HH:mm",CultureInfo.InvariantCulture),
                IsNotified.ToString(),
                State.ToString()
            };
            return csvValues;
        }
    }
}
