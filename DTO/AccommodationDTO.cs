using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BookingApp.DTO;
using BookingApp.Domain.Model;
using BookingApp.Repository;

namespace BookingApp.DTO
{
        public class AccommodationDTO : INotifyPropertyChanged {
        public ObservableCollection<ImageDTO> Images { get; set; } = new ObservableCollection<ImageDTO>();
        public int id;
        public int Id {
            get { return id; }
            set { if (id != value) {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }
        private String name;
        public String Name{
            get { return name; }
            set { if (name != value)  {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        private Location location;
        public Location Location {
            get { return location; }
            set { if (location != value) {
                    location = value;
                    OnPropertyChanged("Location");
                }
            }
        }
        private Owner owner;
        public Owner Owner {
            get { return owner; }
            set { if (owner != value) {
                    owner = value;
                    OnPropertyChanged("Owner");
                }
            }
        }
        private int idLocation;
        public int IdLocation {
            get { return idLocation; }
            set{ if (idLocation != value) {
                    idLocation = value;
                    OnPropertyChanged("IdLocation");
                }
            }
        }
        private AccommodationType accommodationType;
        public AccommodationType AccommodationType{
            get { return accommodationType; }
            set{ if (accommodationType != value) {
                    accommodationType = value;
                    OnPropertyChanged("AccommodationType");
                }
            }
        }
        private int capacity;
        public int Capacity {
            get { return capacity; }
            set { if (capacity != value) {
                    capacity = value;
                    OnPropertyChanged("Capacity");
                }
            }
        }
        private int minStayDays;
        public int MinStayDays {
            get { return minStayDays; }
            set { if (minStayDays != value) {
                    minStayDays = value;
                    OnPropertyChanged("MinStayDays");
                }
            }
        }
        private int cancellationPeriod = 1; 
        public int CancellationPeriod {
            get { return cancellationPeriod; }
            set { if (cancellationPeriod != value){
                    cancellationPeriod = value;
                    OnPropertyChanged("CancellationPeriod");
                }
            }
        }
        private List<Image> image;
         public List<Image> Image {
             get { return image; }
             set {  if (image != value){
                     image = value;
                     OnPropertyChanged("Images");
                 }
             }
         }
        public int ownerId;
        public int OwnerId{
            get { return ownerId; }
            set{  if (ownerId != value){
                    ownerId = value;
                    OnPropertyChanged("OwnerId");
                }
            }
        }
        public AccommodationDTO(){  }
        public AccommodationDTO(Accommodation accommodation,Location location){
            this.Id = accommodation.Id;
            this.Name = accommodation.Name;
            this.IdLocation = accommodation.IdLocation;
            this.Location= location;
            this.AccommodationType = accommodation.AccommodationType;
            this.Capacity = accommodation.Capacity;
            this.MinStayDays = accommodation.MinStayDays;
            this.CancellationPeriod = accommodation.CancellationPeriod;
            this.OwnerId = accommodation.OwnerId;
        }
        public AccommodationDTO(Accommodation accommodation) {
            this.Id = accommodation.Id;
            this.Name = accommodation.Name;
            this.IdLocation = accommodation.IdLocation;
            this.AccommodationType = accommodation.AccommodationType;
            this.Capacity = accommodation.Capacity;
            this.MinStayDays = accommodation.MinStayDays;
            this.CancellationPeriod = accommodation.CancellationPeriod;
            this.OwnerId= accommodation.OwnerId;
        }
        public Accommodation ToAccommodation() {
            var accommodation = new Accommodation();
            accommodation.Id = this.Id;
            accommodation.Name = this.Name;
            accommodation.IdLocation = this.IdLocation;
            accommodation.AccommodationType = this.AccommodationType;
            accommodation.Capacity = this.Capacity;
            accommodation.MinStayDays = this.MinStayDays;
            accommodation.CancellationPeriod = this.CancellationPeriod;
            accommodation.OwnerId = this.OwnerId;
            return accommodation;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}