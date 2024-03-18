using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xaml.Schema;

namespace BookingApp.View.Guide
{
    /// <summary>
    /// Interaction logic for MakeTour.xaml
    /// </summary>
    public partial class MakeTour : Window
    {
        public TourDTO TourDTO { get; set; }

        private TourRepository tourRepository;
        private LocationRepository locationRepository;
        private LanguageRepository languageRepository;
        private CheckPointRepository checkPointRepository;
        private TourStartDateRepository tourStartDateRepository;
        private ImageRepository imageRepository;

        public List<LanguageDTO> LanguageComboBox { get; set; }
        public List<LocationDTO> LocationComboBox { get; set; }

        public List<DateTime> TourStartDates { get; set; }

        private ImageDTO selectedImage;
        public List<ImageDTO> Images { get; set; }
       
        public MakeTour()
        {
            InitializeComponent();
            DataContext = this;
            this.WindowStartupLocation=WindowStartupLocation.CenterScreen;
            tourRepository=new TourRepository();
            languageRepository=new LanguageRepository();
            locationRepository=new LocationRepository();
            checkPointRepository=new CheckPointRepository();
            tourStartDateRepository=new TourStartDateRepository();
            imageRepository=new ImageRepository();
            LanguageComboBox = new List<LanguageDTO>();
            LocationComboBox = new List<LocationDTO>();
            TourStartDates = new List<DateTime>();
            TourDTO = new TourDTO();
            selectedImage = new ImageDTO();
            Images = new List<ImageDTO>();

            LoadLanguagesAndLocations();

        }
        private void LoadLanguagesAndLocations()
        {
            LanguageComboBox.Clear();
            foreach (Language language in languageRepository.GetAll()) LanguageComboBox.Add(new LanguageDTO(language));
            LocationComboBox.Clear();
            foreach (Location location in locationRepository.GetAll()) LocationComboBox.Add(new LocationDTO(location));
        }
        private void AddClick(object sender, RoutedEventArgs e)
        {  
            GetTourLocation();
            GetTourLanguage();

            if (!IsAdditionPossible())
            {
                MessageBox.Show("All fields must be filled properly before adding the tour");
                return;
            }
              
               tourRepository.Add(TourDTO.ToTour());
               AddCheckPoints(tourRepository.GetCurrentId());
               AddTourStartDates(tourRepository.GetCurrentId());
               UpdateImages();
        }
        private void GetTourLocation()
        {
            LocationDTO selectedLocation = (LocationDTO)locationComboBox.SelectedItem;
            if (selectedLocation==null)
            {
                MessageBox.Show("Choose a tour location!");
                return;

            }
            TourDTO.LocationId = selectedLocation.Id;
        }
        private void GetTourLanguage()
        {
            LanguageDTO selectedLanguage = (LanguageDTO)languageComboBox.SelectedItem;
            if (selectedLanguage == null)
            {
                MessageBox.Show("Choose a tour language!");
                return;
            }
            TourDTO.LanguageId = selectedLanguage.Id;

        }
        private bool IsAdditionPossible()
        {
            return Images.Count() > 0 && TourStartDates.Count() > 0 && AreCheckPointsSpecified() && AreStopsSpecified();
        }
        private bool AreCheckPointsSpecified()
        {
            if (string.IsNullOrEmpty(startPointTextBox.Text) || string.IsNullOrEmpty(endPointTextBox.Text))
            {
                MessageBox.Show("Start and end point are mendatory");
                return false;
            }
            return true;
        }
        private bool AreStopsSpecified()
        {
            if (!String.IsNullOrEmpty(stopsTextBox.Text))
            {
                string pattern = @"^([a-zA-Z0-9]+;*)+$";
                if (!Regex.IsMatch(stopsTextBox.Text, pattern))
                {
                    MessageBox.Show("Not good format for stops(it should be TEXT;TEXT;TEXT..)");
                    return false;
                }
            }
            return true;
        }

        private void AddCheckPoints(int tourId)
        {
            AddStartStop(tourId);
            if (!string.IsNullOrEmpty(stopsTextBox.Text))
            {
                AddStops(tourId);
            }
            AddEndStop(tourId);
        }
        private void AddStartStop(int id)
        {
            CheckPoint startingPoint = new CheckPoint(startPointTextBox.Text, id, "START");
            checkPointRepository.Add(startingPoint);
        }
        private void AddEndStop(int id)
        {
            CheckPoint endPoint = new CheckPoint(endPointTextBox.Text, id, "END");
            checkPointRepository.Add(endPoint);
        }
        private void AddStops(int tourId)
        {
            var stopNames = stopsTextBox.Text.Split(';').Select(n => n.Trim()).Where(n => !string.IsNullOrEmpty(n));
            foreach (var name in stopNames)
            {
                CheckPoint stop = new CheckPoint(name, tourId, "STOP");
                checkPointRepository.Add(stop);
            }
        }
        private void AddTourStartDates(int tourId)
        {
            foreach (DateTime tourDate in TourStartDates)
            {
                TourStartDate tourDates = new TourStartDate(tourId, tourDate);
                tourStartDateRepository.Add(tourDates);
            }
        }
        private void AddDateClick(object sender, RoutedEventArgs e)
        {
            var selectedDate = datePicker.SelectedDate;
            var time = TryTimeParse(timeTextBox.Text);
            if (!selectedDate.HasValue || time == null)
            {
                MessageBox.Show("Please select date and time in good format");
                ResetDateInput();
                return;
            }
            var dateTime = selectedDate.Value.Add(time.Value);
            TourStartDates.Add(dateTime);
            ShowAddedDates();
            ResetDateInput();
        }
        private void ShowAddedDates()
        {
            datesListView.ItemsSource = null;
            datesListView.ItemsSource = TourStartDates.Select(date =>
            new
            { DisplayDate = date.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) }).ToList();

        }
        private void ResetDateInput()
        {
            datePicker.SelectedDate = null;
            timeTextBox.Text = "HH:mm";
        }
        private TimeSpan? TryTimeParse(string input)
        {
            if (TimeSpan.TryParse(input, out var time)) return time;
            return null;
        }
        private void UpdateImages()
        {
            int id = tourRepository.GetCurrentId();
            foreach (ImageDTO image in Images)
            {
                image.EntityId = id;
                image.EntityType = EntityType.TOUR;
                imageRepository.Update(image.ToImage());
            }
        }
        private void BrowseAndLoadPictureClick(object sender, RoutedEventArgs e)
        {
            PictureBrowseWindow pictureBrowseWindow = new PictureBrowseWindow();
            pictureBrowseWindow.Owner = this;
            pictureBrowseWindow.WindowStartupLocation=WindowStartupLocation.CenterScreen;
            pictureBrowseWindow.ShowDialog();
            selectedImage = pictureBrowseWindow.selectedImage;
            Images.Add(selectedImage);
        }

        private void LiveTourClick(object sender, RoutedEventArgs e)
        {
            LiveTour liveTour=new LiveTour();
            liveTour.Owner = this;
            liveTour.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            liveTour.ShowDialog();

        }
        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
