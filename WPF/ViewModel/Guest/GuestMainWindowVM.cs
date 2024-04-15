using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.WPF.View.Guest;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class GuestMainWindowVM : ViewModelBase
    {
        private readonly AccommodationService accommodationService;
        private readonly ImageService imageService;
        private readonly LocationService locationService;
        private readonly OwnerService ownerService;

        public ObservableCollection<AccommodationDTO> AllAccommodations { get; set; }
        public ObservableCollection<ImageDTO> Images { get; set; }
        public ObservableCollection<AccommodationType> Types { get; set; }

        private AccommodationDTO selectedAccommodation;
        public AccommodationDTO SelectedAccommodation
        {
            get => selectedAccommodation;
            set
            {
                selectedAccommodation = value;
                OnPropertyChanged(nameof(SelectedAccommodation));
            }
        }

        private string nameFilter;
        public string NameFilter
        {
            get => nameFilter;
            set
            {
                nameFilter = value;
                OnPropertyChanged(nameof(NameFilter));
            }
        }

        private string cityFilter;
        public string CityFilter
        {
            get => cityFilter;
            set
            {
                cityFilter = value;
                OnPropertyChanged(nameof(CityFilter));
            }
        }

        private string countryFilter;
        public string CountryFilter
        {
            get => countryFilter;
            set
            {
                countryFilter = value;
                OnPropertyChanged(nameof(CountryFilter));
            }
        }

        private AccommodationType? selectedType;
        public AccommodationType? SelectedType
        {
            get => selectedType;
            set
            {
                selectedType = value;
                OnPropertyChanged(nameof(SelectedType));
            }
        }

        private string numberOfGuests;
        public string NumberOfGuests
        {
            get => numberOfGuests;
            set
            {
                numberOfGuests = value;
                OnPropertyChanged(nameof(NumberOfGuests));
            }
        }

        private string numberOfDaysToStay;
        public string NumberOfDaysToStay
        {
            get => numberOfDaysToStay;
            set
            {
                numberOfDaysToStay = value;
                OnPropertyChanged(nameof(NumberOfDaysToStay));
            }
        }

        public GuestMainWindowVM()
        {
            accommodationService = new AccommodationService();
            imageService = new ImageService();
            locationService = new LocationService();
            ownerService = new OwnerService();
            AllAccommodations = new ObservableCollection<AccommodationDTO>();
            Images = new ObservableCollection<ImageDTO>();
            Types = new ObservableCollection<AccommodationType>(Enum.GetValues(typeof(AccommodationType)).Cast<AccommodationType>());
            Update();
        }

        /*0|Hilton|1|APARTMENT|5|2|6|1
1|Maria|1|CABIN|8|5|5|1
2|Plaza|3|HOUSE|5|5|20|1
3|Valamar|2|APARTMENT|8|4|0|5
4|Park|3|HOUSE|4|5|70|1
5|President|5|APARTMENT|6|2|2|5
6|Sunshine|1|APARTMENT|5|5|5|1
7|uefhab|3|HOUSE|5|8|2|1
8|fahiekga|2|HOUSE|5|5|15|1
9|gggggggggg|2|APARTMENT|5|5|1|1
10|ymkrykry6k|2|APARTMENT|5|5|1|1
11|fhsrfhx|1|APARTMENT|5|5|1|1
12|hdfnxb|2|APARTMENT|5|5|1|5
13|gaha|1|APARTMENT|5|5|1|1
14|tdgncrgsf|1|APARTMENT|5|8|1|1
15|he|5|HOUSE|5|5|1|1
16|DAJANA|6|APARTMENT|8|8|8|1
17|petak|2|APARTMENT|5|5|1|5*/

        public void Update()
        {
               AllAccommodations.Clear();
                var allImages = imageService.GetImagesForEntityType(EntityType.ACCOMMODATION);
                AddSuperOwnerAccommodation(allImages);
               AddOwnerAccommodation(allImages); 
        }
        public void AddSuperOwnerAccommodation(List<ImageDTO> allImages)
        {
            foreach (var accommodation in accommodationService.GetAll())
            {
                var owner = ownerService.GetByIdDTO(accommodation.OwnerId);
                if (owner.Role == "SUPEROWNER")
                {
                    var matchingImages = new ObservableCollection<ImageDTO>(imageService.GetImagesByAccommodation(accommodation.Id, allImages));
                    LocationDTO location = locationService.GetById(accommodation.IdLocation);
                    AllAccommodations.Add(new AccommodationDTO(accommodation.ToAccommodation(), location.ToLocation()) { Images = matchingImages });
                }
                else { continue; }
            }
        }
        public void AddOwnerAccommodation(List<ImageDTO> allImages)
        {
            foreach (var accommodation in accommodationService.GetAll())
            {
                var owner = ownerService.GetByIdDTO(accommodation.OwnerId);
                if (owner.Role == "OWNER")
                {
                    var matchingImages = new ObservableCollection<ImageDTO>(imageService.GetImagesByAccommodation(accommodation.Id, allImages));
                    LocationDTO location = locationService.GetById(accommodation.IdLocation);
                    AllAccommodations.Add(new AccommodationDTO(accommodation.ToAccommodation(), location.ToLocation()) { Images = matchingImages });
                }
                else { continue; }
            }
        }

        public void SearchClick()
        {
              Update();
              var filteredAccommodations = FilterAccommodations();
              AllAccommodations.Clear();
              foreach (var accommodation in filteredAccommodations)
              {
                  AllAccommodations.Add(accommodation);
              }
            
        }

        //REFAKTORISI!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!NEMOJ ZABORAVITI
        private List<AccommodationDTO> FilterAccommodations()
        {
            return AllAccommodations
                .Where(accommodation =>
                    (string.IsNullOrEmpty(NameFilter) || accommodation.Name.ToLower().Contains(NameFilter.ToLower())) &&
                    (string.IsNullOrEmpty(CityFilter) || accommodation.Location.City.ToLower().Contains(CityFilter.ToLower())) &&
                    (string.IsNullOrEmpty(CountryFilter) || accommodation.Location.Country.ToLower().Contains(CountryFilter.ToLower())) &&
                    (!SelectedType.HasValue || accommodation.AccommodationType == SelectedType.Value) &&
                     (string.IsNullOrEmpty(NumberOfGuests) || accommodation.Capacity >= int.Parse(NumberOfGuests)) &&
                     (string.IsNullOrEmpty(NumberOfDaysToStay) || accommodation.MinStayDays <= double.Parse(NumberOfDaysToStay))
                )
                .ToList();
        }

        public void BookAccommodationClick()
        {
            if (SelectedAccommodation != null)
            {
                var dialog = new ReserveAccommodation(SelectedAccommodation);
                dialog.ShowDialog();
            }
            else
            {
                MessageBox.Show("You didn't choose accommodation!");
            }
        }

        public void OpenReservationsClick()
        {
            var dialog = new MyReservationsWindow();
            dialog.ShowDialog();
        }
        public void OpenNotificationsClick()
        {
            var dialog = new GuestNotifications();
            dialog.ShowDialog();
        }



    }
}
