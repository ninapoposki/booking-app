using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
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
            LanguageDTO selectedLanguage = (LanguageDTO)languageComboBox.SelectedItem;
            LocationDTO selectedLocation = (LocationDTO)locationComboBox.SelectedItem;
            if (selectedLanguage != null) TourDTO.LanguageId = selectedLanguage.Id;
            else
            {
                MessageBox.Show("Choose a tour language!");
                
            }

            if (selectedLocation != null) TourDTO.LocationId = selectedLocation.Id;
            else
            {
                MessageBox.Show("Choose a tour location!");
            }
           
            tourRepository.Add(TourDTO.ToTour());
            AddCheckPoints(tourRepository.GetCurrentId());
            
            foreach(DateTime tourDate in TourStartDates)
            {
                TourStartDate tourDates = new TourStartDate(tourRepository.GetCurrentId(), tourDate);
                tourStartDateRepository.Add(tourDates);
            }
            if (Images.Count != 0)
            {
                foreach (ImageDTO image in Images)
                {
                    image.EntityId = tourRepository.GetCurrentId();

                    image.EntityType = EntityType.TOUR;
                    imageRepository.Update(image.ToImage());

                }

            }

            Close();

            
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddCheckPoints(int tourId)
        {

            if (string.IsNullOrEmpty(startPointTextBox.Text) || string.IsNullOrEmpty(endPointTextBox.Text)) MessageBox.Show("Start and end point are mendatory");

            CheckPoint startingPoint = new CheckPoint(startPointTextBox.Text, tourId,"START");
            checkPointRepository.Add(startingPoint);

            if (!string.IsNullOrEmpty(stopsTextBox.Text))
            {
                AddStops(tourId);
            }
            CheckPoint endPoint = new CheckPoint(endPointTextBox.Text, tourId, "END");
            checkPointRepository.Add(endPoint);
        }

        private void AddStops(int tourId)
        {
            var stopNames=stopsTextBox.Text.Split(';').Select(n => n.Trim()).Where(n => !string.IsNullOrEmpty(n)); 
            foreach(var name in stopNames)
            {
                CheckPoint stop = new CheckPoint(name, tourId, "STOP");
                checkPointRepository.Add(stop);
            }
        }

        private void AddDateClick(object sender, RoutedEventArgs e)
        {
            var selectedDate = datePicker.SelectedDate;
            var time = TryTimeParse(timeTextBox.Text);
            if (selectedDate.HasValue && time!=null)
            {
                var dateTime = selectedDate.Value.Add(time.Value);

                TourStartDates.Add(dateTime);


                datesListView.ItemsSource = null;
                datesListView.ItemsSource = TourStartDates.Select(date => 
                new
                { DisplayDate = date.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) }).ToList(); 


                datePicker.SelectedDate = null;
                timeTextBox.Text = "HH:mm";
               

            }
            else
            {
                MessageBox.Show("Please select date in good format");
            }
        }

        private TimeSpan? TryTimeParse(string input)
        {
            if(TimeSpan.TryParse(input, out var time))   return time; 
            return null;
        }

        private void BrowseAndLoadPictureClick(object sender, RoutedEventArgs e)
        {
            PictureBrowseWindow pictureBrowseWindow = new PictureBrowseWindow();
            pictureBrowseWindow.ShowDialog();
            selectedImage = pictureBrowseWindow.selectedImage;
            Images.Add(selectedImage);
        }

        private void LiveTourClick(object sender, RoutedEventArgs e)
        {
            LiveTour liveTour=new LiveTour();
            liveTour.ShowDialog();

        }
    }
}
