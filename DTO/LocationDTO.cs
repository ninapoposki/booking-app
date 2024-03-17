using BookingApp.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.DTO
{
    public class LocationDTO : INotifyPropertyChanged
    {
       
        public int Id { get; set; }

        private string city;
        public string City
        {
            get { return city; }

            set
            {

                if(value!=city)
                {


                    city = value;
                    OnPropertyChanged("City");
                }
            }
        }


        private string country; 
        public string Country
        {




            get { return country; }
            set
            {



                if(value!=country)
                {


                    country = value;
                    OnPropertyChanged("Country");
                }
            }
        }

        public Location ToLocation()
        {


            return new Location(Id,city,country);
        }
        
        public LocationDTO() { }


        public LocationDTO (Location location)
        {
            Id= location.Id;
            City= location.City;
            Country= location.Country;
        }
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
