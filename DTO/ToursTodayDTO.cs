using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class ToursTodayDTO : INotifyPropertyChanged
    {
        public int Id {  get; set; }
        private string path;
        public string Path
        {
            get { return path; }
            set
            {
                if (value != path)
                {
                    path = value;
                    OnPropertyChanged("Path");
                }
            }
        }
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }

            }
        }
        private TourStartDateDTO tourDateTime;
        public TourStartDateDTO TourDateTime
        {
            get { return tourDateTime; }
            set
            {
                if (tourDateTime != value)
                {
                    tourDateTime = value;
                    OnPropertyChanged("TourDateTime");
                }
            }
        }
        private double duration;
        public double Duration
        {
            get { return duration; }
            set
            {
                if (value != duration)
                {
                    duration = value;
                    OnPropertyChanged("Duration");
                }

            }
        }
        private TimeSpan timeUntilStart;
        public TimeSpan TimeUntilStart
        {
            get { return timeUntilStart; }
            set
            {
                if (value != timeUntilStart)
                {
                    timeUntilStart = value;
                    OnPropertyChanged("TimeUntilStart");
                }

            }
        }
        public Language Language { get; set; }
        public int LanguageId { get; set; }

        public ToursTodayDTO() { }
        public ToursTodayDTO(Tour tour, Language language)
        {
            Id = tour.Id;
            Name = tour.Name;
            Language = language;
            LanguageId = tour.LanguageId;
            Duration = tour.Duration;
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
