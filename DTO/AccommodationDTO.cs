using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;

namespace BookingApp.DTO
{
        public class AccommodationDTO : INotifyPropertyChanged, IDataErrorInfo
    {
        

        public int id = -1;
        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }


        private String name;
        public String Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }


        private Location location;
        public Location Location
        {
            get { return location; }
            set
            {
                if (location != value)
                {
                    location = value;
                    OnPropertyChanged("Location");
                }
            }
        }
        

        private int idLocation;
        public int IdLocation
        {
            get { return idLocation; }
            set
            {
                if (idLocation != value)
                {
                    idLocation = value;
                    OnPropertyChanged("IdLocation");
                }
            }
        }

        private AccommodationType accommodationType;
        public AccommodationType AccommodationType
        {
            get { return accommodationType; }
            set
            {
                if (accommodationType != value)
                {
                    accommodationType = value;
                    OnPropertyChanged("AccommodationType");
                }
            }
        }

        private int capacity;
        public int Capacity
        {
            get { return capacity; }
            set
            {
                if (capacity != value)
                {
                    capacity = value;
                    OnPropertyChanged("Capacity");
                }
            }
        }


        private int minStayDays;
        public int MinStayDays
        {
            get { return minStayDays; }
            set
            {
                if (minStayDays != value)
                {
                    minStayDays = value;
                    OnPropertyChanged("MinStayDays");
                }
            }
        }

        private int cancellationPeriod;
        public int CancellationPeriod
        {
            get { return cancellationPeriod; }
            set
            {
                if (cancellationPeriod != value)
                {
                    cancellationPeriod = value;
                    OnPropertyChanged("CancellationPeriod");
                }
            }
        }

       
        //imagedto
        private List<Image> images;

        public List<Image> Images
        {
            get { return images; }
            set
            {
                if (images != value)
                {
                    images = value;
                    OnPropertyChanged("Images");
                }
            }
        }

        public AccommodationDTO()
        {

        }
     
        public AccommodationDTO(Accommodation accommodation,Location location)
        {
            this.Id = accommodation.Id;
            this.Name = accommodation.Name;
            this.IdLocation = accommodation.IdLocation;
            this.Location= location;
            this.AccommodationType = accommodation.AccommodationType;
            this.Capacity = accommodation.Capacity;
            this.MinStayDays = accommodation.MinStayDays;
            this.CancellationPeriod = accommodation.CancellationPeriod;
        }

        public Accommodation ToAccommodation()
        {
            var accommodation = new Accommodation();

            accommodation.Id = this.Id;
            accommodation.Name = this.Name;
            accommodation.IdLocation = this.IdLocation;
            accommodation.AccommodationType = this.AccommodationType;
            accommodation.Capacity = this.Capacity;
            accommodation.MinStayDays = this.MinStayDays;
            accommodation.CancellationPeriod = this.CancellationPeriod;

            return accommodation;
        }

        public string Error => null;
        private Regex _BrojRegex = new Regex("[0-9]+");
        public string this[string columnName]
        {
            get
            {
                if (columnName == "Name")
                {
                    if (string.IsNullOrEmpty(Name))
                        return "You must enter accommodation name!";
                }
                else if (columnName == "Capacity")
                {
                    if (Capacity <= 0)
                        return "Capacity cannot be negative number!";

                    Match match = _BrojRegex.Match(Capacity.ToString());
                    if (!match.Success)
                        return "You must enter number";
                }
                else if (columnName == "MinStayDays")
                {
                    if (MinStayDays <= 0)
                        return "Min Stay Days cannot be negative number!";

                    Match match = _BrojRegex.Match(MinStayDays.ToString());
                    if (!match.Success)
                        return "You must enter number";
                }
                else if (columnName == "CancellationPeriod")
                {
                    if (CancellationPeriod <= 0)
                        return "Cancellation Period cannot be negative number!";

                    Match match = _BrojRegex.Match(CancellationPeriod.ToString());
                    if (!match.Success)
                        return "You must enter number";
                }

                return null;
            }
        }


        private readonly string[] _validatedProperties = { "Name", "Capacity", "MinStayDays", "CancellationPeriod" };


        public bool IsValid
        {
            get
            {
                foreach (var property in _validatedProperties)
                {
                    if (this[property] != null)
                        return false;
                }

                return true;
            }
        }



        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
