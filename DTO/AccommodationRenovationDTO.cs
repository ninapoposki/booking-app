using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
   public  class AccommodationRenovationDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        
        private int accommodationId;
        public int AccommodationId
        {
            get { return accommodationId; }
            set { if (value != accommodationId) { accommodationId = value; OnPropertyChanged("AccommodationId"); } }
        }
        private DateTime initialDate;
        public DateTime InitialDate
        {
            get { return initialDate; }
            set { if (value != initialDate) { initialDate = value; OnPropertyChanged("InitialDate"); } }
        }
        private DateTime endDate = DateTime.Now;
        public DateTime EndDate
        {
            get { return endDate; }
            set { if (value != endDate) { endDate = value; OnPropertyChanged("EndDate"); } }
        }

        private int duration;
        public int Duration
        {
            get { return duration; }
            set { if (value != duration) { duration = value; OnPropertyChanged("Duration"); } }
        }
        private string description;
        public string Description
        {
            get { return description; }
            set { if (value != description) { description = value; OnPropertyChanged("Description"); } }
        }
        public AccommodationRenovationDTO() { }
        public AccommodationRenovation ToAccommodationRenovation()
        {
            var accommodationrenovation = new AccommodationRenovation();
            accommodationrenovation.Id = this.Id;
            accommodationrenovation.AccommodationId = this.AccommodationId;
   
            accommodationrenovation.InitialDate = this.InitialDate;
            accommodationrenovation.EndDate = this.EndDate;
            accommodationrenovation.Duration = this.Duration;
            accommodationrenovation.Description = this.Description;

            return accommodationrenovation;
        }
        public AccommodationRenovationDTO(AccommodationRenovation renovation)
        {
            Id = renovation.Id;
           
            AccommodationId = renovation.AccommodationId;
            
            InitialDate = renovation.InitialDate;
            EndDate = renovation.EndDate;
            Duration = renovation.Duration;
            Description = renovation.Description;
          
        }
       
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(name)); }
        }
    }
}
