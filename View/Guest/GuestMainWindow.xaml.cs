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

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for GuestMainWindow.xaml
    /// </summary>
    public partial class GuestMainWindow : Window
    {
        private readonly AccommodationRepository accommodationRepository;
        private readonly LocationRepository locationRepository;
        //accommodationType?
        public ObservableCollection<AccommodationDTO> AllAccommodations { get; set; }
        //public ObservableCollection<LanguageDTO> Languages { get; set; } //ovo miuslim da ne treba,da samo arijani treba

        public AccommodationDTO SelectedAccommodation { get; set; }


        public GuestMainWindow()
        {
            InitializeComponent();
            DataContext = this;
            accommodationRepository = new AccommodationRepository();
            locationRepository = new LocationRepository();
            AllAccommodations = new ObservableCollection<AccommodationDTO>();


            Update();
        }
        public void Update()
        {
            AllAccommodations.Clear();
            foreach (Accommodation accommodation in accommodationRepository.GetAll())
            {
                //ovo proveri,ne valja
                AllAccommodations.Add(new AccommodationDTO(accommodation)); //vidi jel jos nesto prosledjujes kao parametar

            }
            // Languages.Clear();- ovo jel treba


            /* foreach (Language language in languageRepository.GetAll())
             {-proveri
                 Languages.Add(new LanguageDTO(language));
             }*/

        }
        private void SearchClick(object sender, RoutedEventArgs e)
        {
            //parsiras uneti string
            bool parseGuestNumberSuccess = int.TryParse(NumberOfGuestsTextBox.Text, out int numberOfGuestsParsed);
            bool parseStayDaysSuccess = double.TryParse(NumberOfDaysToStayTextBox.Text, out double stayDaysParsed); //da li promenim ovo u kraci naziv number of?
            string name = NameTextBox.Text;
            string city = CityTextBox.Text;
            string country = CountryTextBox.Text;
            string type = (TypeComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

            AccommodationDTO accommodation = new AccommodationDTO(); //ne puca kad napravis instancu ali to ne valaj to popravi

            var filteredAccommodations = AllAccommodations
                    .Where(accommodation =>
                        (string.IsNullOrEmpty(name) || accommodation.Name.Contains(name)) &&
                        (string.IsNullOrEmpty(city) || accommodation.Location.City.Contains(city)) &&
                        (string.IsNullOrEmpty(country) || accommodation.Location.Country.Contains(country)) &&
                        (string.IsNullOrEmpty(type) || accommodation.AccommodationType.ToString() == type) &&
                        (numberOfGuestsParsed == 0 || accommodation.Capacity >= numberOfGuestsParsed) &&
                        (stayDaysParsed == 0 || accommodation.MinStayDays >= stayDaysParsed)
                    )
                    .ToList();

            // Ažuriranje DataGrid-a sa filtriranim smeštajima
            ToursDataGrid.ItemsSource = filteredAccommodations;

        }



        private void BookAccommodationButton(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodation == null)
            {


                MessageBox.Show("Please select the accommodation you wish to book.");

                return;
            }
            // AccommodationReservation accommodationReservation = new AccommodationReservation(SelectedTour);?
            // accommodationReservation.Show();
        }


        private void ButtonCancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}