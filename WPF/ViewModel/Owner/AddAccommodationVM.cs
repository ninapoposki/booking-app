using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using BookingApp.Services;
using System.Security.Cryptography.Xml;
using BookingApp.Domain.IRepositories;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class AddAccommodationVM : ViewModelBase
    {
        private ImageService imageService;
        private AccommodationService accommodationService;
        private LocationService locationService;
        private UserService userService;
        public ObservableCollection<AccommodationType> Types { get; set; }
        public ObservableCollection<ImageDTO> Images { get; set; }
        public List<LocationDTO> LocationComboBox { get; set; }
        public List<String> CityComboBox { get; set; }
        public HashSet<String> CountryComboBox { get; set; }
        public AccommodationDTO accommodationDTO { get; set; }
        private ImageDTO SelectedImage;
        public string SelectedCountry { get; set; }
        
        public AddAccommodationVM(string currentUserUsername)
        {
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            accommodationService = new AccommodationService();
            locationService = new LocationService(Injector.Injector.CreateInstance<ILocationRepository>());
            userService = new UserService(Injector.Injector.CreateInstance<IUserRepository>());
            LocationComboBox = new List<LocationDTO>();
            CityComboBox = new List<String>();
            CountryComboBox = new HashSet<String>();
            accommodationDTO = new AccommodationDTO();

            var type = Enum.GetValues(typeof(AccommodationType)).Cast<AccommodationType>();
            Types = new ObservableCollection<AccommodationType>(type);
            Images = new ObservableCollection<ImageDTO>();
            SelectedImage = new ImageDTO();
            userService.UpdateUser(accommodationDTO, currentUserUsername);
            LoadCountries();
            LoadCity();

        }
        public void CountryChanged()
        {
            LoadCity();
        }
        
        private void LoadCountries()
        {
            CountryComboBox.Clear();
            foreach (String country in locationService.GetAllCountries()) CountryComboBox.Add(country);
        }

        private void LoadCity()
        {
            string selectedCountry = SelectedCountry;
            if (SelectedCountry != null)
            {
                CityComboBox.Clear();
                foreach (String city in locationService.GetAllCities(SelectedCountry)) CityComboBox.Add(city);
            }
        }

        public void AddAccommodationButtonClick()
        {
            UpdateImages();
            String selectedCountry = SelectedCountry;
            if (SelectedCity != null && selectedCountry != null) accommodationDTO.IdLocation = locationService.GetLocationId(SelectedCity, selectedCountry);

            if (accommodationDTO.IsValid) {
                MessageBox.Show("Accommodation added successfully!");
                accommodationService.Add(accommodationDTO.ToAccommodation());

            } else MessageBox.Show("Accommodation cannot be created. Not all field are valid.");
        }

        public void UpdateImages()
        { 
            int id = accommodationService.GetCurrentId();
            foreach (ImageDTO image in Images)
            {
                imageService.UpdateAccommodation(image, id);
            }
        }

        public void BrowseImageClick()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = imageService.FilterImages();
            openFileDialog.InitialDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\Images"));
            openFileDialog.ShowDialog();
            AddImage(openFileDialog.FileName);
        }
        public void AddImage(string absolutePath)
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
        private void RemoveImageClick(object sender, RoutedEventArgs e)
        {
            if (SelectedImage != null)
            {
                Images.Remove(SelectedImage);
            }
        }
      
        private List<string> cities;
        public List<string> Cities
        {
            get { return cities; }
            set
            {
                if (cities != value)
                {
                    cities = value;
                    OnPropertyChanged("Cities");
                }
            }
        }

        private string selectedCity;
        public string SelectedCity
        {
            get { return selectedCity; }
            set
            {
                if (selectedCity != value)
                {
                    selectedCity = value;
                    OnPropertyChanged(nameof(SelectedCity)); 
                }
            }
        }
    }
}
