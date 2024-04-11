using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class GuestMainWindowVM : ViewModelBase
    {
        private readonly AccommodationRepository accommodationRepository;
        private readonly LocationRepository locationRepository;
        private readonly ImageRepository imageRepository;

        public ObservableCollection<AccommodationDTO> AllAccommodations { get; set; }
        public ObservableCollection<ImageDTO> Images { get; set; }
        public AccommodationDTO SelectedAccommodation { get; set; }
        public ObservableCollection<AccommodationType> Types { get; set; }
        public string NumberOfGuests { get; set; }
        public string NumberOfDaysToStay { get; set; }
        public AccommodationType? SelectedType { get; set; } // Dodao sam SelectedType kao član klase

        private readonly GuestService guestService;
        private readonly LocationService locationService;
        private readonly ImageService imageService;

        public GuestMainWindowVM()
        {
            accommodationRepository = new AccommodationRepository();
            locationRepository = new LocationRepository();
            imageRepository = new ImageRepository();
            guestService = new GuestService();
            locationService = new LocationService();
            imageService = new ImageService();

            AllAccommodations = new ObservableCollection<AccommodationDTO>();
            Images = new ObservableCollection<ImageDTO>();
            //Types = new ObservableCollection<AccommodationType>(Enum.GetValues(typeof(AccommodationType)).Cast<AccommodationType>());

            Update();
        }

        private void Update()
        {
            AllAccommodations.Clear();
            var allImages = imageRepository.GetAll()
                                    .Where(img => img.EntityType == EntityType.ACCOMMODATION)
                                    .Select(img => new ImageDTO(img))
                                    .ToList();
            foreach (var accommodation in accommodationRepository.GetAll())
            {
                var matchingImages = new ObservableCollection<ImageDTO>(allImages.Where(img => img.EntityId == accommodation.Id).ToList());
                Location location = locationRepository.GetById(accommodation.IdLocation);
                AllAccommodations.Add(new AccommodationDTO(accommodation, location)
                {
                    Images = matchingImages
                });
            }
        }

        public void Search()
        {
            bool parseGuestNumberSuccess = int.TryParse(NumberOfGuests, out int numberOfGuestsParsed);
            bool parseStayDaysSuccess = double.TryParse(NumberOfDaysToStay, out double stayDaysParsed);

            var filteredAccommodations = FilterAccommodations();
            // Implementirati kako želite prikaz filtriranih podataka
        }

        private List<AccommodationDTO> FilterAccommodations()
        {
            string nameFilter = ""; // Implementirati kako želite da se postavi ovaj podatak
            string cityFilter = ""; // Implementirati kako želite da se postavi ovaj podatak
            string countryFilter = ""; // Implementirati kako želite da se postavi ovaj podatak

            return AllAccommodations
                .Where(accommodation =>
                    IsAccommodationValid(accommodation, nameFilter, SelectedType) &&
                    IsLocationValid(accommodation, cityFilter, countryFilter) &&
                    IsAccommodationOccupancyValid(accommodation)
                )
                .ToList();
        }

        private bool IsAccommodationValid(AccommodationDTO accommodation, string nameFilter, AccommodationType? selectedType)
        {
            return IsNameValid(accommodation, nameFilter) && IsTypeValid(accommodation, selectedType);
        }

        private bool IsNameValid(AccommodationDTO accommodation, string nameFilter)
        {
            return string.IsNullOrEmpty(nameFilter) || accommodation.Name.ToLower().Contains(nameFilter);
        }

        private bool IsLocationValid(AccommodationDTO accommodation, string cityFilter, string countryFilter)
        {
            return IsCityValid(accommodation, cityFilter) && IsCountryValid(accommodation, countryFilter);
        }

        private bool IsCityValid(AccommodationDTO accommodation, string cityFilter)
        {
            return string.IsNullOrEmpty(cityFilter) || accommodation.Location.City.ToLower().Contains(cityFilter);
        }

        private bool IsCountryValid(AccommodationDTO accommodation, string countryFilter)
        {
            return string.IsNullOrEmpty(countryFilter) || accommodation.Location.Country.ToLower().Contains(countryFilter);
        }

        private bool IsTypeValid(AccommodationDTO accommodation, AccommodationType? selectedType)
        {
            return selectedType == null || accommodation.AccommodationType == selectedType;
        }

        private bool IsAccommodationOccupancyValid(AccommodationDTO accommodation)
        {
            return true; // Implementirati logiku za proveru broja gostiju i trajanja boravka
        }

        public void BookAccommodation()
        {
            // Implementirati logiku za rezervaciju smeštaja
        }

        public void OpenReservations()
        {
            // Implementirati logiku za otvaranje rezervacija
        }
    }
}
