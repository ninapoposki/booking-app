using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BookingApp.Observer;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Diagnostics.Metrics;
using System.Diagnostics.Eventing.Reader;

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for GuestMainWindow.xaml
    /// </summary>
    public partial class GuestMainWindow : Window
    {
        private readonly AccommodationRepository accommodationRepository;
        private readonly LocationRepository locationRepository;
        public ObservableCollection<AccommodationDTO> AllAccommodations { get; set; }
        public ObservableCollection<ImageDTO> Images { get; set; }
        private readonly ImageRepository imageRepository;
        public AccommodationDTO SelectedAccommodation { get; set; }
        public ObservableCollection<AccommodationType> Types { get; set; }
        private readonly AccommodationReservationRepository accommodationReservationRepository;
        public AccommodationDTO accommodationDTO;


        public GuestMainWindow()
        {
            InitializeComponent();
            DataContext = this;
            accommodationRepository = new AccommodationRepository();
            locationRepository = new LocationRepository();
            imageRepository = new ImageRepository();
            AllAccommodations = new ObservableCollection<AccommodationDTO>();
            Images = new ObservableCollection<ImageDTO>();
            accommodationDTO=new AccommodationDTO();
            var type=Enum.GetValues(typeof(AccommodationType)).Cast<AccommodationType>();
            Types = new ObservableCollection<AccommodationType>(type);

            Update();
        }
        public void Update()
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

        private void SearchClick(object sender, RoutedEventArgs e)
        {
            bool parseGuestNumberSuccess = int.TryParse(NumberOfGuestsTextBox.Text, out int numberOfGuestsParsed);
            bool parseStayDaysSuccess = double.TryParse(NumberOfDaysToStayTextBox.Text, out double stayDaysParsed);

            AccommodationType? selectedType = TypeComboBox.SelectedItem as AccommodationType?;

            var filteredAccommodations = FilterAccommodations();
            ToursDataGrid.ItemsSource = filteredAccommodations;
        }

        private List<AccommodationDTO> FilterAccommodations()
        {
            string nameFilter = NameTextBox.Text.ToLower();
            string cityFilter = CityTextBox.Text.ToLower();
            string countryFilter = CountryTextBox.Text.ToLower();
            bool hasSelectedType = TypeComboBox.SelectedItem != null;
            AccommodationType? selectedType = hasSelectedType ? (AccommodationType?)TypeComboBox.SelectedItem : null;

            return AllAccommodations
                .Where(accommodation =>
                    IsAccommodationValid(accommodation,nameFilter,selectedType)&&
                    IsLocationValid(accommodation,cityFilter,countryFilter) &&
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
            return IsGuestNumberValid(accommodation) && AreStayDaysValid(accommodation);
        }

        private bool IsGuestNumberValid(AccommodationDTO accommodation)
        {
            int.TryParse(NumberOfGuestsTextBox.Text, out int numberOfGuestsParsed);
            return numberOfGuestsParsed == 0 || accommodation.Capacity >= numberOfGuestsParsed;
        }
        private bool AreStayDaysValid(AccommodationDTO accommodation)
        {
            double.TryParse(NumberOfDaysToStayTextBox.Text, out double stayDaysParsed);
            return stayDaysParsed == 0 || accommodation.MinStayDays <= stayDaysParsed;
        }

        private void BookAccommodationClick(object sender, RoutedEventArgs e)
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


        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}