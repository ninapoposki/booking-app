using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class TourDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value != name)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }

            }
        }

        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (value != description)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }

            }
        }

        public Language Language { get; set; }
        public Location Location { get; set; }
       public int LocationId { get; set; }
        public int LanguageId {  get; set; }
       
        private int capacity;
        public int Capacity
        {
            get
            {
                return capacity;
            }
            set
            {
                if (value != capacity)
                {
                    capacity = value;
                    OnPropertyChanged("Capacity");
                }

            }
        }

        private double duration;
        public double Duration
        {
            get
            {
                return duration;
            }
            set
            {
                if (value != duration)
                {
                    duration = value;
                    OnPropertyChanged("Duration");
                }

            }
        }



        public TourDTO()
        {

        }
        public TourDTO(Tour tour, LocationRepository locationRepository, LanguageRepository languageRepository)
        {
            Id = tour.Id;
            Name = tour.Name;
            Description = tour.Description;
            LanguageId=tour.LanguageId;

            LocationId=tour.LocationId;

            Capacity = tour.Capacity;
            Duration = tour.Duration;

            Location = locationRepository.GetById(LocationId);
            Language=languageRepository.GetById(LanguageId);
         


        }
       public Tour ToTour()
       {
            return new Tour(Id, name, description, LanguageId, LocationId, capacity, duration);
           
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
