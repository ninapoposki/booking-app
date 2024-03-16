using BookingApp.Observer;
using System;
using BookingApp.Repository;
using BookingApp.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
using System.Collections.ObjectModel;
using BookingApp.DTO;

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for TouristMainWindow.xaml
    /// </summary>
    public partial class TouristMainWindow : Window,IObserver
    {



        private readonly TourRepository tourRepository;
        private readonly LocationRepository locationRepository;
        private readonly LanguageRepository languageRepository;
        private readonly TourStartDateRepository tourStartDateRepository;
        private readonly ImageRepository imageRepository;
        public ObservableCollection<TourDTO> AllTours { get; set; }
        public ObservableCollection<ImageDTO> Images { get; set; }
        public ObservableCollection<LanguageDTO> Languages { get; set; }

        public TourDTO SelectedTour { get; set; } 

        public TouristMainWindow()
        {
            InitializeComponent();
            DataContext = this;
            tourRepository= new TourRepository();
            locationRepository = new LocationRepository(); 
            languageRepository = new LanguageRepository();
            imageRepository = new ImageRepository();
            tourStartDateRepository = new TourStartDateRepository();
            AllTours = new ObservableCollection<TourDTO>();
            Images = new ObservableCollection<ImageDTO>();
            Languages = new ObservableCollection<LanguageDTO>();
            tourRepository.subject.Subscribe(this);
            

            Update();
            

        }

        public void Update()
        {
            GetTours();
            GetLanguages();
        }

        private void GetTours()
        {
            AllTours.Clear();
            var allImages = imageRepository.GetAll()
                                    .Where(img => img.EntityType == EntityType.TOUR)
                                    .Select(img => new ImageDTO(img))
                                    .ToList();


            foreach (var tour in tourRepository.GetAll())
            {
                var matchingImages = new ObservableCollection<ImageDTO>(allImages.Where(img => img.EntityId == tour.Id).ToList());
                var tourDTO = new TourDTO(tour, locationRepository.GetById(tour.LocationId), languageRepository.GetById(tour.LanguageId))
                {
                    DateTimes = new ObservableCollection<TourStartDateDTO>(UpdateDate(tour.Id)),
                    Images = matchingImages
                };
                AllTours.Add(tourDTO);
            }
        }

        private void GetLanguages()
        {
            Languages.Clear();
            foreach (var language in languageRepository.GetAll())
            {
                Languages.Add(new LanguageDTO(language));
            }
        }

        private IEnumerable<TourStartDateDTO> UpdateDate(int tourId)
        {
            var dateTimesForTour = new List<TourStartDateDTO>();
            foreach (var startTime in tourStartDateRepository.GetByTourId(tourId))
            {
                dateTimesForTour.Add(new TourStartDateDTO(startTime));
            }
            return dateTimesForTour;
        }




        private void CancelTour(object sender, RoutedEventArgs e)
        {

            this.Close();
        }


        private void SearchTour(object sender, RoutedEventArgs e)
        {
            int peopleCount = ParsePeopleCount(PeopleTextBox.Text);
            double duration = ParseDuration(DaysTextBox.Text);
            string selectedLanguage = GetSelectedLanguage();

            List<TourDTO> filteredTours = FilterTours(peopleCount, duration, selectedLanguage);

            UpdateTourDataGrid(filteredTours);
        }

        private int ParsePeopleCount(string text)
        {
            int result;
            if (int.TryParse(text, out result))
            {
                return result;
            }
            else
            {
                return -1; 
            }
        }

        private double ParseDuration(string text)
        {
            double result;
            if (double.TryParse(text, out result))
            {
                return result;
            }
            else
            {
                return -1.0; 
            }
        }

        private string GetSelectedLanguage()
        {
            if (LanguageComboBox.SelectedItem is LanguageDTO selectedLanguage)
            {
                return selectedLanguage.Name;
            }
            return string.Empty;
        }

        private List<TourDTO> FilterTours(int peopleCount, double duration, string selectedLanguage)
        {
            return AllTours.Where(tour =>
                IsMatchCity(tour) &&
                IsMatchCountry(tour) &&
                IsMatchDuration(tour, duration) &&
                IsMatchLanguage(tour, selectedLanguage) &&
                IsMatchPeopleCount(tour, peopleCount)
            ).ToList();
        }

        private bool IsMatchCity(TourDTO tour)
        {
            return string.IsNullOrWhiteSpace(CityTextBox.Text) || tour.Location.City.Equals(CityTextBox.Text, StringComparison.OrdinalIgnoreCase);
        }

        private bool IsMatchCountry(TourDTO tour)
        {
            return string.IsNullOrWhiteSpace(CountryTextBox.Text) || tour.Location.Country.Equals(CountryTextBox.Text, StringComparison.OrdinalIgnoreCase);
        }

        private bool IsMatchDuration(TourDTO tour, double duration)
        {
            return duration < 0 || Math.Abs(tour.Duration - duration) < 0.01;
        }

        private bool IsMatchLanguage(TourDTO tour, string selectedLanguage)
        {
            return string.IsNullOrEmpty(selectedLanguage) || tour.Language.Name.Equals(selectedLanguage, StringComparison.OrdinalIgnoreCase);
        }

        private bool IsMatchPeopleCount(TourDTO tour, int peopleCount)
        {
            return peopleCount < 0 || (tour.Capacity >= peopleCount && peopleCount > 0);
        }

        private void UpdateTourDataGrid(List<TourDTO> tours)
        {
            ToursDataGrid.ItemsSource = tours;
        }
        private void BookTour(object sender, RoutedEventArgs e)
        {
            if (SelectedTour == null)
            {
                ShowMessage("Molimo Vas da odaberete turu koju želite da rezervišete.");
                return;
            }

            if (SelectedTour.Capacity > 0)
            {
                StartTourReservation(SelectedTour);
                return;
            }

            ShowNoSpaceAvailableMessage();
            ShowAvailableToursOnSameLocation();
        }

        private void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        private void StartTourReservation(TourDTO tour)
        {
            TourReservationWindow tourReservationWindow = new TourReservationWindow(tour);
            tourReservationWindow.Show();
        }

        private void ShowNoSpaceAvailableMessage()
        {
            ShowMessage("Nažalost, na ovoj turi nema više slobodnih mesta, ali nudimo vam ostale!");
        }

        private void ShowAvailableToursOnSameLocation()
        {
            var locationId = SelectedTour.LocationId;
            var availableTours = AllTours.Where(tour => tour.LocationId == locationId && tour.Capacity > 0).ToList();

            if (availableTours.Count == 0)
            {
                ShowMessage("Nema dostupnih tura na ovoj lokaciji.");
                return;
            }


            var availableTourWindow = new AvailableTourWindow(availableTours);
            availableTourWindow.Show();

        }



    }
}
