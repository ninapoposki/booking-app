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
using BookingApp.DTO;
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

        public AccommodationDTO SelectedAccommodation { get; set; }
        public ObservableCollection<AccommodationType> Types { get; set; }


        public GuestMainWindow()
        {
            InitializeComponent();
            DataContext = this;
            accommodationRepository = new AccommodationRepository();
            locationRepository = new LocationRepository();
            AllAccommodations = new ObservableCollection<AccommodationDTO>();
   
            var type=Enum.GetValues(typeof(AccommodationType)).Cast<AccommodationType>();
            Types = new ObservableCollection<AccommodationType>(type);

            Update();
        }
        public void Update()
        {
            AllAccommodations.Clear();
            foreach (Accommodation accommodation in accommodationRepository.GetAll())
            {
                Location location = locationRepository.GetById(accommodation.IdLocation);
                AllAccommodations.Add(new AccommodationDTO(accommodation,location)); 

            }
           

        }
        private void SearchClick(object sender, RoutedEventArgs e)
        {
        
            bool parseGuestNumberSuccess = int.TryParse(NumberOfGuestsTextBox.Text, out int numberOfGuestsParsed);
            bool parseStayDaysSuccess = double.TryParse(NumberOfDaysToStayTextBox.Text, out double stayDaysParsed); 
          
            AccommodationType? selectedType = TypeComboBox.SelectedItem as AccommodationType?;

            //probaj da nadjes neko optimalnije resenje
            var filteredAccommodations = AllAccommodations
                    .Where(accommodation =>
                        (string.IsNullOrEmpty(NameTextBox.Text) || accommodation.Name.ToLower().Contains(NameTextBox.Text.ToLower())) &&
                        (string.IsNullOrEmpty(CityTextBox.Text) || accommodation.Location.City.ToLower().Contains(CityTextBox.Text.ToLower())) &&
                        (string.IsNullOrEmpty(CountryTextBox.Text) || accommodation.Location.Country.ToLower().Contains(CountryTextBox.Text.ToLower())) &&
                        (!selectedType.HasValue || accommodation.AccommodationType == selectedType)&&
                        (numberOfGuestsParsed == 0 || accommodation.Capacity >= numberOfGuestsParsed) &&
                        (stayDaysParsed == 0 || accommodation.MinStayDays <= stayDaysParsed)
                    )
                    .ToList();
            ToursDataGrid.ItemsSource = filteredAccommodations;

        }



        private void BookAccommodationButton(object sender, RoutedEventArgs e)
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


        private void CancelButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}