using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.WPF.View.Guide;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using BookingApp.Services;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class MakeTourVM : ViewModelBase
    {
        public TourDTO TourDTO { get; set; }
        private TourService tourService;
        private LocationService locationService;
        private LanguageService languageService;
        private CheckPointService checkPointService;
        private TourStartDateService tourStartDateService;
        private ImageService imageService;
        public List<LanguageDTO> LanguageComboBox { get; set; }
        public LanguageDTO SelectedLanguage { get; set; }
        public List<LocationDTO> LocationComboBox { get; set; }
        public ObservableCollection<DateTime> TourStartDates { get; set; }
        public DateTime SelectedDate { get; set; }
        public ObservableCollection<ImageDTO> Images { get; set; }
        public ImageDTO SelectedImage { get; set; }
        public LocationDTO SelectedCity { get; set; }
        public CheckPointDTO SelectedCheckPoint { get; set; }
        public ObservableCollection<CheckPointDTO> CheckPoints { get; set; }
        public MakeTourVM(int userId)
        {
            tourService = new TourService();
            tourStartDateService = new TourStartDateService();
            checkPointService = new CheckPointService();
            imageService = new ImageService();
            languageService = new LanguageService();
            locationService = new LocationService();
            LanguageComboBox = new List<LanguageDTO>();
            LocationComboBox = new List<LocationDTO>();
            SelectedLanguage = new LanguageDTO();
            TourStartDates = new ObservableCollection<DateTime>();
            SelectedDate = DateTime.Now;
            TourDTO = new TourDTO();
            TourDTO.UserId=userId;
            SelectedCity = new LocationDTO();
            Images = new ObservableCollection<ImageDTO>();
            SelectedImage = new ImageDTO();
            CheckPoints = new ObservableCollection<CheckPointDTO>();
            SelectedCheckPoint = new CheckPointDTO();
            LoadLanguagesAndCities();
        }
        private void LoadLanguagesAndCities()
        {
            languageService.GetAll(LanguageComboBox);
            locationService.GetAll(LocationComboBox);
        }
        private void LoadCountry()
        {
            Country=locationService.GetCountry(SelectedCity);
        }
        public void CityChanged()
        {
            LoadCountry();
        }
        public void AddClick()
        {
            GetTourLocation();
            GetTourLanguage();
            tourService.Add(TourDTO.ToTour());
            AddCheckPoints(tourService.GetCurrentId());
            AddTourStartDates(tourService.GetCurrentId());
            UpdateImages();
        }
        private void GetTourLocation()
        {
            if (SelectedCity == null) { MessageBox.Show("Choose a tour city!"); return; }
            TourDTO.LocationId = SelectedCity.Id;
        }
        private void GetTourLanguage()
        {
            if (SelectedLanguage == null) { MessageBox.Show("Choose a tour language!"); return; }
            TourDTO.LanguageId = SelectedLanguage.Id;
        }
        public void AddCheckPointClick()
        {
            CheckPointDTO newCheckPoint = new CheckPointDTO { Name = CheckPointName, Type = "STOP" };
            CheckPoints.Add(newCheckPoint);
            CheckPointName = "";
            UpdateCheckPointTypes();
        }

        private void UpdateCheckPointTypes()
        { 
            for (int i = 0; i < CheckPoints.Count; i++)
            {
                if (i == 0) { CheckPoints[i].Type = "START"; }
                else if (i == CheckPoints.Count - 1) { CheckPoints[i].Type = "END"; }
                else { CheckPoints[i].Type = "STOP"; }
            }
        }
        public void RemoveCheckPointClick()
        {
            if (SelectedCheckPoint != null)
            {
                CheckPoints.Remove(SelectedCheckPoint);
                UpdateCheckPointTypes();
            }
        }
        private void AddCheckPoints(int tourId)
        {
            foreach (CheckPointDTO checkPoint in CheckPoints) { checkPointService.Add(checkPoint, tourId); }
        }
        public void AddDate()
        {
            var time = TryTimeParse(Time);
            if (time == null || SelectedDate==null)
            {
                MessageBox.Show("Please select date and time in good format");
                ResetDateInput();
                return;
            }
            DateTime dateTime = SelectedDate.Date;
            dateTime=dateTime.Add(time.Value);
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
            Time= "HH:mm";
        }
        public void RemoveDate()
        {
            TourStartDates.Remove(SelectedDate);
        }
        private void AddTourStartDates(int tourId)
        {
            foreach (DateTime tourDate in TourStartDates) { tourStartDateService.Add(tourDate, tourId); }
        }
        private void UpdateImages()
        {
            int id = tourService.GetCurrentId();
            foreach (ImageDTO image in Images) { imageService.Update(image, id); }
        }
        public void BrowseImageClick()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = imageService.FilterImages();
            openFileDialog.InitialDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Images"));
            openFileDialog.ShowDialog();
            AddImage(openFileDialog.FileName);
        }
        private void AddImage(string absolutePath)
        {
            string relativePath = MakeRelativePath(absolutePath);
            Images.Add(imageService.GetByPath(relativePath));
        }
        private string MakeRelativePath(string absolutPath)
        {
            string referencePath = "..\\..\\..\\Resources\\Images\\";
            string[] pathPieces = absolutPath.Split('\\');
            string relativePath = referencePath + pathPieces[pathPieces.Length - 1];
            return relativePath.Replace("/", "\\");
        }
        public void RemoveImageClick()
        {
            if (SelectedImage != null) { Images.Remove(SelectedImage); }
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
    }
}
