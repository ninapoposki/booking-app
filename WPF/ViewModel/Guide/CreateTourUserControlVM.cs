using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using BookingApp.WPF.View.Guide;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class CreateTourUserControlVM : ViewModelBase
    {
        public BreadCrumbsVM BreadCrumbsVM { get; set; }    
        public MyICommand AddCheckPointCommand { get; }
        public MyICommand<CheckPointDTO> DeleteCheckPointCommand { get; }
        public MyICommand BrowseImagesCommand { get; }
        public MyICommand<ImageDTO> DeleteImageCommand { get; }
        public MyICommand AddDateCommand { get; }
        public MyICommand<DateTime> DeleteDateCommand { get; }
        public MyICommand AddTourCommand { get; }
        public NavigationService NavigationService { get; set; }
        private int userId;
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
        public ObservableCollection<CheckPointDTO> CheckPoints { get; set; }
        public CreateTourUserControlVM(NavigationService navigationService,int userId,ObservableCollection<BreadcrumbItem> breadcrumbs)
        {
            BreadCrumbsVM=new BreadCrumbsVM(breadcrumbs);
            NavigationService = navigationService;
            this.userId = userId;
            AddCheckPointCommand = new MyICommand(OnAddCheckPoint);
            DeleteCheckPointCommand = new MyICommand<CheckPointDTO>(OnDeleteCheckPoint);
            BrowseImagesCommand = new MyICommand(OnBrowseImage);
            DeleteImageCommand = new MyICommand<ImageDTO>(OnDeleteImage);
            AddDateCommand = new MyICommand(OnAddDate);
            DeleteDateCommand = new MyICommand<DateTime>(OnDeleteDate);
            AddTourCommand = new MyICommand(OnAddTour,CanAddTour);
            tourService = new TourService(Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            tourStartDateService = new TourStartDateService(Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            checkPointService = new CheckPointService(Injector.Injector.CreateInstance<ICheckPointRepository>());
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            languageService = new LanguageService(Injector.Injector.CreateInstance<ILanguageRepository>());
            locationService = new LocationService(Injector.Injector.CreateInstance<ILocationRepository>());
            LanguageComboBox = new List<LanguageDTO>();
            LocationComboBox = new List<LocationDTO>();
            SelectedLanguage = new LanguageDTO();
            TourStartDates = new ObservableCollection<DateTime>();
            SelectedDate = DateTime.Now;
            TourDTO = new TourDTO();
            TourDTO.UserId = userId;
            Images = new ObservableCollection<ImageDTO>();
            CheckPoints = new ObservableCollection<CheckPointDTO>();
            LoadLanguagesAndCities();
        }

        private void LoadLanguagesAndCities()
        {
            languageService.GetAll(LanguageComboBox);
            locationService.GetAll(LocationComboBox);
        }
        private void LoadCountry()
        {
            Country = locationService.GetCountry(SelectedCity);
        }
        public void OnAddTour()
        {
            GetTourLocation();
            GetTourLanguage();
            tourService.Add(TourDTO.ToTour());
            AddCheckPoints(tourService.GetCurrentId());
            AddTourStartDates(tourService.GetCurrentId());
            UpdateImages();
            MessageBox.Show("Added succesfully");
            NavigationService.Navigate(new GuideHomeUserControl(NavigationService, userId,BreadCrumbsVM.Breadcrumbs));
            BreadCrumbsVM.ResetBreadcrumbs();
        }
        public bool CanAddTour()
        {
            return Images.Count > 0 && CheckPoints.Count >= 2 && TourStartDates.Count>0;
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
        public void OnAddCheckPoint()
        {
            CheckPointDTO newCheckPoint = new CheckPointDTO { Name = CheckPointName, Type = "STOP" };
            CheckPoints.Add(newCheckPoint);
            AddTourCommand.RaiseCanExecuteChanged();
            CheckPointName = "";
            UpdateCheckPointTypes();
        }
        private void OnDeleteCheckPoint(CheckPointDTO checkPoint)
        {
            CheckPoints.Remove(checkPoint);
            AddTourCommand.RaiseCanExecuteChanged();
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
        private void AddCheckPoints(int tourId)
        {
            foreach (CheckPointDTO checkPoint in CheckPoints) { checkPointService.Add(checkPoint, tourId); }
        }
        public void OnAddDate()
        {
            var time = TryTimeParse(Time);
            DateTime dateTime = SelectedDate.Date;
            dateTime = dateTime.Add(time.Value);
            TourStartDates.Add(dateTime);
            AddTourCommand.RaiseCanExecuteChanged();
            Time = "HH:mm";
        }
        private TimeSpan? TryTimeParse(string input)
        {
            if (TimeSpan.TryParse(input, out var time)) return time;
            return null;
        }
        public void OnDeleteDate(DateTime date)
        {
            TourStartDates.Remove(date);
            AddTourCommand.RaiseCanExecuteChanged();
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
        public void OnBrowseImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = imageService.FilterImages();
            openFileDialog.InitialDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Resources\Images"));
            if (openFileDialog.ShowDialog() == true) { AddImage(openFileDialog.FileName); }
        }
        private void AddImage(string absolutePath)
        {
            string relativePath = MakeRelativePath(absolutePath);
            Images.Add(imageService.GetByPath(relativePath));
            AddTourCommand.RaiseCanExecuteChanged();
        }
        private string MakeRelativePath(string absolutPath)
        {
            string referencePath = "../../../Resources/Images/";
            string[] pathPieces = absolutPath.Split('\\');
            string relativePath = referencePath + pathPieces[pathPieces.Length - 1];
            return relativePath;
        }
        public void OnDeleteImage(ImageDTO image)
        {
            Images.Remove(image);
            AddTourCommand.RaiseCanExecuteChanged();
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
        private LocationDTO selectedCity;
        public LocationDTO SelectedCity
        {
            get { return selectedCity; }
            set
            {
                if (selectedCity != value)
                {
                    selectedCity = value;
                    OnPropertyChanged("SelectedCity");
                    LoadCountry();
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
