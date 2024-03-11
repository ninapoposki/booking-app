using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookingApp.Model
{
        public class AccommodationDTO : INotifyPropertyChanged
        {
        
        private int _Id = -1;
        public int Id
        {
            get { return _Id; }
            set
            {
                if (_Id != value)
                {
                    _Id = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private String _Name;
        public String Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /*
        private LocationDTO _Location;
        public LocationDTO Adresa_stanovanja
        {
            get { return _Location; }
            set
            {
                if (_Location != value)
                {
                    _Location = value;
                    NotifyPropertyChanged();
                }
            }
        }
        */

        private int _IdLocation;
        public int IdLocation
        {
            get { return _IdLocation; }
            set
            {
                if (_IdLocation != value)
                {
                    _IdLocation = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private AccommodationType _AccommodationType;
        public AccommodationType AccommodationType
        {
            get { return _AccommodationType; }
            set
            {
                if (_AccommodationType != value)
                {
                    _AccommodationType = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private int _Capacity;
        public int Capacity
        {
            get { return _Capacity; }
            set
            {
                if (_Capacity != value)
                {
                    _Capacity = value;
                    NotifyPropertyChanged();
                }
            }
        }


        private int _MinStayDays;
        public int MinStayDays
        {
            get { return _MinStayDays; }
            set
            {
                if (_MinStayDays != value)
                {
                    _MinStayDays = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private int _CancellationPeriod;
        public int CancellationPeriod
        {
            get { return _CancellationPeriod; }
            set
            {
                if (_CancellationPeriod != value)
                {
                    _CancellationPeriod = value;
                    NotifyPropertyChanged();
                }
            }
        }

       
        //imagedto
        private List<Image> _Images;

        public List<Image> Images
        {
            get { return _Images; }
            set
            {
                if (_Images != value)
                {
                    _Images = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public AccommodationDTO()
        {
            // images = new List<ImageDTO>();


        }
        public AccommodationDTO(Accommodation accommodation)
        {
            this.Id = accommodation.Id;
            this.Name = accommodation.Name;
            this.IdLocation = accommodation.IdLocation;
            this.AccommodationType = accommodation.AccommodationType;
            this.Capacity = accommodation.Capacity;
            this.MinStayDays = accommodation.MinStayDays;
            this.CancellationPeriod = accommodation.CancellationPeriod;
            //  images??
        }

        public Accommodation ToAccommodation()
        {
            var accommodation = new Accommodation();

            accommodation.Id = this.Id;
            accommodation.Name = this.Name;
            accommodation.IdLocation = this.IdLocation;
            accommodation.AccommodationType = this.AccommodationType;

            return accommodation;
        }



        public event PropertyChangedEventHandler PropertyChanged;


        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
