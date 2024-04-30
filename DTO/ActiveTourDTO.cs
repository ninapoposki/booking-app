using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class ActiveTourDTO: INotifyPropertyChanged
    {
        public int Id { get; set; }
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
        private int numberOfTourists;
        public int NumberOfTourists
        {
            get { return numberOfTourists; }
            set
            {
                if (value != numberOfTourists)
                {
                    numberOfTourists = value;
                    OnPropertyChanged("NumberOfTourists");
                }
            }
        }
        public Language Language { get; set; }
        public Location Location { get; set; }
        public int LocationId { get; set; }
        public int LanguageId { get; set; }
        private string checkPointName;
        public string CheckPointName
        {
            get { return checkPointName; }
            set
            {
                if (checkPointName != value)
                {
                    checkPointName = value;
                    OnPropertyChanged("CheckPointName");
                }
            }
        }
        private string checkPointType;
        public string CheckPointType
        {
            get { return checkPointType; }
            set
            {
                if (checkPointType != value)
                {
                    checkPointType = value;
                    OnPropertyChanged("CheckPointType");
                }
            }
        }
        public ActiveTourDTO() {  }
        public ActiveTourDTO(TourDTO tourDTO)
        {
            Id=tourDTO.Id;
            Name=tourDTO.Name;
            Location=tourDTO.Location;
            LocationId=tourDTO.LocationId;
            LanguageId=tourDTO.LanguageId;
            Language=tourDTO.Language;

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
