using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class TourStatisticsDTO:INotifyPropertyChanged
    {
        public TourDTO Tour {  get; set; }       
        private int totalNumberOfTourists;
        public int TotalNumberOfTourists
        {
            get { return totalNumberOfTourists; }
            set
            {
                if (value != totalNumberOfTourists)
                {
                    totalNumberOfTourists = value;
                    OnPropertyChanged("TotalNumberOfTourists");
                }
            }
        }
        private int youngVisitorsCount;
        public int YoungVisitorsCount
        {
            get { return youngVisitorsCount; }
            set
            {
                if (youngVisitorsCount != value)
                {
                    youngVisitorsCount = value;
                    OnPropertyChanged("YoungVisitorsCount");
                }
            }
        }
        private string path;
        public string Path
        {
            get { return path; }
            set
            {
                if (path != value)
                {
                    path = value;
                    OnPropertyChanged("Path");
                }
            }
        }
        public int Id {  get; set; }
        private int seniorVisitorsCount;
        public int SeniorVisitorsCount
        {
            get { return seniorVisitorsCount; }
            set
            {
                if (seniorVisitorsCount != value)
                {
                    seniorVisitorsCount = value;
                    OnPropertyChanged("SeniorVisitorsCount");
                }
            }
        }
        private int adultVisitorsCount;
        public int AdultVisitorsCount
        {
            get { return adultVisitorsCount; }
            set
            {
                if (adultVisitorsCount != value)
                {
                    adultVisitorsCount = value;
                    OnPropertyChanged("AdultVisitorsCount");
                }
            }
        }
        private TourStartDateDTO selectedDateTime;
        public TourStartDateDTO SelectedDateTime
        {
            get { return selectedDateTime; }
            set
            {
                if (selectedDateTime != value)
                {
                    selectedDateTime = value;
                    OnPropertyChanged(nameof(SelectedDateTime));
                }
            }
        }
        public TourStatisticsDTO() { }
        public TourStatisticsDTO(TourDTO tourDTO)
        {
            Tour=tourDTO;
            Id = tourDTO.Id;
            SelectedDateTime=tourDTO.SelectedDateTime;
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

