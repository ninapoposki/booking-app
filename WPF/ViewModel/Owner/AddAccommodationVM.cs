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
using System.IO;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class AddAccommodationVM : ViewModelBase{
        private ImageService imageService;
        private AccommodationService accommodationService;
        public ObservableCollection<AccommodationType> Types { get; set; }
        public ObservableCollection<ImageDTO> Images { get; set; }
        public List<LocationDTO> LocationComboBox { get; set; }
        public List<String> CityComboBox { get; set; }
        public HashSet<String> CountryComboBox { get; set; }
        public AccommodationDTO accommodationDTO { get; set; }
        public string SelectedCountry { get; set; }
        
        public AddAccommodationVM(int currentUserId) {
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            accommodationService = new AccommodationService(Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>());
            LocationComboBox = new List<LocationDTO>();
            CityComboBox = new List<String>();
            CountryComboBox = new HashSet<String>();
            accommodationDTO = new AccommodationDTO();
            var type = Enum.GetValues(typeof(AccommodationType)).Cast<AccommodationType>();
            Types = new ObservableCollection<AccommodationType>(type);
            Images = new ObservableCollection<ImageDTO>();
            accommodationDTO.OwnerId = currentUserId;
            LoadCountries();
            LoadCity();
        }
        public void CountryChanged() {   LoadCity(); }
        private void LoadCountries() {
            CountryComboBox.Clear();
            foreach (String country in accommodationService.locationService.GetAllCountries()) CountryComboBox.Add(country);
        }
        private void LoadCity(){
            if (SelectedCountry != null) {
                CityComboBox.Clear();
                foreach (String city in accommodationService.locationService.GetAllCities(SelectedCountry)) CityComboBox.Add(city);
            }
        }
        public void AddAccommodationButtonClick() {
            UpdateImages();
            if (SelectedCity != null && SelectedCountry != null) accommodationDTO.IdLocation = accommodationService.locationService.GetLocationId(SelectedCity, SelectedCountry);
                MessageBox.Show("Accommodation added successfully!");
                accommodationService.Add(accommodationDTO.ToAccommodation());
        }
        public void UpdateImages(){ 
            foreach (ImageDTO image in Images) {
                accommodationService.imageService.UpdateAccommodation(image, accommodationService.GetCurrentId());
            }
        }
        public void BrowseImageClick() {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = imageService.FilterImages();
            openFileDialog.InitialDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Resources\Images"));// Konvertuje relativnu u apsolutnu putanju
            if (openFileDialog.ShowDialog() == true) { AddImage(openFileDialog.FileName); }  
        }
        public void AddImage(string absolutePath) {
            string referencePath = "../../../Resources/Images/";
            string[] pathPieces = absolutePath.Split('\\');
            string relativePath = (referencePath + pathPieces[pathPieces.Length - 1]);
            Images.Add(imageService.GetByPath(relativePath));
        }
        private string selectedCity;
        public string SelectedCity{
            get { return selectedCity; }
            set { if (selectedCity != value) { selectedCity = value; OnPropertyChanged(nameof(SelectedCity)); } }
        }
    }
}