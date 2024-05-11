using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace BookingApp.DTO
{
    public class TourRequestDTO:INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public Location Location { get; set; }
        public int LanguageId { get; set; }
        public Language Language { get; set; }
        private string description;
        public string Description
        {
            get { return description; }          
            set
            {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }
        private int numberOfTourists;
        public int NumberOfTourists
        {
            get { return numberOfTourists; }         
            set
            {
                if (value != numberOfTourists)
                {
                    numberOfTourists = value;
                    OnPropertyChanged("MumberOfTourists");
                }
            }
        }
        private string startDate;
        public string StartDate
        {
            get { return startDate; }
            set
            {
                if (value != startDate)
                {
                    startDate = value;
                    OnPropertyChanged("StartDate");
                }
            }
        }
        private string endDate;
        public string EndDate
        {
            get { return endDate; }
            set
            {
                if (value != endDate)
                {
                    endDate = value;
                    OnPropertyChanged("EndDate");
                }
            }
        }
        private string choosenDate;
        public string ChoosenDate
        {
            get { return choosenDate; }
            set
            {
                if (value != choosenDate)
                {
                    choosenDate = value;
                    OnPropertyChanged("ChoosenDate");
                }
            }
        }
        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                if (value != username)
                {
                    username = value;
                    OnPropertyChanged("Username");
                }
            }
        }
        private State state;
        public State State
        {
            get { return state; }
            set
            {
                if (state != value)
                {
                    state = value;
                    OnPropertyChanged("State");
                }
            }
        }
        private bool isNotified;
        public bool IsNotified
        {
            get { return isNotified; }
            set
            {
                if (isNotified != value)
                {
                    isNotified = value;
                    OnPropertyChanged("IsNotified");
                }
            }
        }
     
        public TourRequestDTO() { }
        public TourRequestDTO(TourRequest tourRequest, Location location, Language language)
        {
            Id = tourRequest.Id;
            LocationId = tourRequest.LocationId;
            Location = location;
            Description = tourRequest.Description;
            LanguageId = tourRequest.LanguageId;
            Language = language;
            NumberOfTourists = tourRequest.NumberOfTourists;
            StartDate = tourRequest.StartDate.ToString("dd/MM/yyyy");
            EndDate = tourRequest.EndDate.ToString("dd/MM/yyyy");
            ChoosenDate = tourRequest.ChoosenDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            IsNotified = tourRequest.IsNotified;
            State = tourRequest.State;
        }
        public TourRequest ToTourRequest()
        {
            return new TourRequest(Id, LocationId, LanguageId, description, numberOfTourists, state, DateOnly.ParseExact(startDate, "dd/MM/yyyy"), DateOnly.ParseExact(endDate, "dd/MM/yyyy"), DateTime.ParseExact(choosenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture), isNotified);
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
