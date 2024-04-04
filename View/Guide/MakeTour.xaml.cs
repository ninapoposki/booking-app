using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.View.Guide
{
    /// <summary>
    /// Interaction logic for MakeTour.xaml
    /// </summary>
    public partial class MakeTour : Window, INotifyPropertyChanged
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
        public ObservableCollection<DateTime> TourStartDates { get; set; }
        public DateTime SelectedDate { get; set; }
        public ObservableCollection<ImageDTO> Images { get; set; }
        public ImageDTO SelectedImage {  get; set; }
        public LocationDTO SelectedCity { get; set; }
        public CheckPointDTO SelectedCheckPoint { get; set; }
        public ObservableCollection<CheckPointDTO> CheckPoints { get; set; }
        public MakeTour()
        {
            InitializeComponent();
            DataContext = this;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            tourRepository = new TourRepository();
            languageRepository = new LanguageRepository();
            locationRepository = new LocationRepository();
            checkPointRepository = new CheckPointRepository();
            tourStartDateRepository = new TourStartDateRepository();
            imageRepository = new ImageRepository();

            LanguageComboBox = new List<LanguageDTO>();
            LocationComboBox = new List<LocationDTO>();
            
            TourStartDates = new ObservableCollection<DateTime>();
            SelectedDate = new DateTime();

            TourDTO = new TourDTO();
            SelectedCity = new LocationDTO();
    
            Images = new ObservableCollection<ImageDTO>();
            SelectedImage = new ImageDTO();
            
            CheckPoints = new ObservableCollection<CheckPointDTO>();
            SelectedCheckPoint = new CheckPointDTO();

            LoadLanguagesAndCities();

        }
        private void LoadLanguagesAndCities()
        {
            LanguageComboBox.Clear();
            foreach (Language language in languageRepository.GetAll()) LanguageComboBox.Add(new LanguageDTO(language));
            LocationComboBox.Clear();
            foreach (Location location in locationRepository.GetAll()) LocationComboBox.Add(new LocationDTO(location));
        }
        private void LoadCountry()
        {
            foreach (Location location in locationRepository.GetAll())
            {
                if (SelectedCity != null && SelectedCity.Id == location.Id)
                {
                    Country = location.Country;
                }
            }
        }
        private void CityChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadCountry();
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
            MessageBox.Show("Tour added successfully");
        }
        private void GetTourLocation()
        {
            LocationDTO selectedLocation = (LocationDTO)cityComboBox.SelectedItem;
            if (selectedLocation == null)
            {
                MessageBox.Show("Choose a tour city!");
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
            return Images.Count() > 0 && TourStartDates.Count() > 0;
        }

        //kljucne tacke
        private void AddCheckPointClick(object sender, RoutedEventArgs e)
        {
            CheckPointDTO newCheckPoint = new CheckPointDTO { Name = CheckPointName, Type = "STOP" };
            CheckPoints.Add(newCheckPoint);
            CheckPointName = "";

            UpdateCheckPointTypes();
        }

        private void UpdateCheckPointTypes()
        {
         
            for (int i = 0; i < CheckPoints.Count; i++) {
                if (i == 0)
                {
                    CheckPoints[i].Type = "START";
                }
                else if (i == CheckPoints.Count - 1)
                {
                    CheckPoints[i].Type = "END";
                }
                else
                {
                    CheckPoints[i].Type = "STOP";
                }
            }
        }
        private void RemoveCheckPointClick(object sender, RoutedEventArgs e)
        {
            if (SelectedCheckPoint != null)
            {
                CheckPoints.Remove(SelectedCheckPoint);
                UpdateCheckPointTypes();
            }
        }
       
        private void AddCheckPoints(int tourId)
        {
            foreach (CheckPointDTO checkPoint in CheckPoints)
            {
                CheckPoint newCheckPoint = new CheckPoint(checkPoint.Name, tourId, checkPoint.Type);
                checkPointRepository.Add(newCheckPoint);
            }
        }
    
        //Datumi
        private void AddDateClick(object sender, RoutedEventArgs e)
        {
            var selectedDate = datePicker.SelectedDate;
            var time = TryTimeParse(Time);
            if (!selectedDate.HasValue || time == null)
            {
                MessageBox.Show("Please select date and time in good format");
                ResetDateInput();
                return;
            }
            DateTime dateTime = selectedDate.Value.Add(time.Value);
            TourStartDates.Add(dateTime);
            ResetDateInput();
        }
        private TimeSpan? TryTimeParse(string input)
        {
            if (TimeSpan.TryParse(input, out var time)) return time;
            return null;
        }
        private void ResetDateInput()
        {
            datePicker.SelectedDate = null;
            timeTextBox.Text = "HH:mm";
        }
        private void RemoveDate(object sender, RoutedEventArgs e)
        {
                TourStartDates.Remove(SelectedDate);   
        }
        private void AddTourStartDates(int tourId)
        {
            foreach (DateTime tourDate in TourStartDates)
            {
                TourStartDate tourDates = new TourStartDate(tourId, tourDate);
                tourStartDateRepository.Add(tourDates);
            }
        }

        //Slike
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
        private void BrowseImageClick(object sender, RoutedEventArgs e)
        {
            List<Model.Image> images = imageRepository.FilterImages();
            if (images.Count()==0) return;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string filter = "Image files|";//(*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
            foreach (Model.Image image in images)
            {
                filter += image.Path.Split("\\")[5]+";";
            }
            filter = filter.TrimEnd(';');
            openFileDialog.Filter = filter;

            openFileDialog.InitialDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Images"));
            openFileDialog.ShowDialog();
            AddImage(openFileDialog.FileName);
        }
        private void AddImage(string absolutePath)
        {
            string relativePath = MakeRelativePath(absolutePath);
            Model.Image image = imageRepository.FindByPath(relativePath);
            Images.Add(new ImageDTO(image));

        }
        private string MakeRelativePath(string absolutPath)
        {
            string referencePath = "..\\..\\..\\Resources\\Images\\";
            string[] pathPieces=absolutPath.Split('\\');

            string relativePath = referencePath + pathPieces[pathPieces.Length-1];
            return relativePath.Replace("/", "\\");
        }
        private void RemoveImageClick(object sender, RoutedEventArgs e)
        {
            if (SelectedImage != null)
            {
                Images.Remove(SelectedImage);
               
            }
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
       

        
        private string country;

        public string Country
        {
            get { return country; }
            set
            {
                if (country != value)
                {
                    country = value;
                    OnPropertyChanged("Country");
                }
            }
        }
        private string checkPointName;
        public string CheckPointName
        {
            get { return checkPointName; }
            set
            {
                if (checkPointName != value)
                {
                    checkPointName = value;
                    OnPropertyChanged("CheckPointName");
                }
            }
        }
        private string time;
        public string Time
        {
            get { return time; }
            set
            {
                if (time != value)
                {
                    time = value;
                    OnPropertyChanged("Time");
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
