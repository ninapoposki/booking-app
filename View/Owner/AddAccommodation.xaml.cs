﻿using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.View.Guide;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
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
using static System.Net.Mime.MediaTypeNames;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for AddAccommodation.xaml
    /// </summary>
    public partial class AddAccommodation : Window, INotifyPropertyChanged
    {
        public AccommodationDTO Accommodation { get; set; }
        public AccommodationRepository accommodationRepository { get; set; }
        private ImageRepository imageRepository {  get; set; }
        private LocationRepository locationRepository {  get; set; }

        private UserRepository userRepository { get; set; }
        public ObservableCollection<AccommodationType> Types { get; set; }
        public List<LocationDTO> LocationComboBox { get; set; }

        private ImageDTO SelectedImage;
        public ObservableCollection<ImageDTO> Images { get; set; }

        private User currentUser;
        public AddAccommodation(AccommodationRepository accommodationRepository, string currentUserUsername)
        {
            InitializeComponent();
            DataContext = this;
            Accommodation = new AccommodationDTO();
            this.accommodationRepository = accommodationRepository;

            Images = new ObservableCollection<ImageDTO>();
            SelectedImage = new ImageDTO();
            imageRepository = new ImageRepository();


            locationRepository = new LocationRepository();
            LocationComboBox = new List<LocationDTO>();

            var type = Enum.GetValues(typeof(AccommodationType)).Cast<AccommodationType>();
            Types = new ObservableCollection<AccommodationType>(type);
             LoadLocations();

           userRepository = new UserRepository();
           // currentUser = userRepository.GetByUserId(currentUserId);
          // currentUser.Id = currentUserId;
          currentUser = userRepository.GetByUsername(currentUserUsername);

        }

       
        private void LoadLocations()
        {
                   
            LocationComboBox.Clear();
            foreach (Location location in locationRepository.GetAll()) LocationComboBox.Add(new LocationDTO(location));
        }
    
   


    public delegate void AccommodationAddedEventHandler(object sender, EventArgs e);
        public event AccommodationAddedEventHandler AccommodationAdded;
        
        private void AddAccommodationButtonClick(object sender, RoutedEventArgs e)
        {

            UpdateImages();

            LocationDTO selectedLocation = (LocationDTO)locationComboBox.SelectedItem;

            if (selectedLocation != null) Accommodation.IdLocation = selectedLocation.Id;

            Accommodation.OwnerId = currentUser.Id;

            if (Accommodation.IsValid) {
                MessageBox.Show("Dodavanje smeštaja");
               
                accommodationRepository.Add(Accommodation.ToAccommodation());
                //this.DialogResult = true;
                Close();
                
            } else MessageBox.Show("Accommodation cannot be created. Not all field are valid.");
            
        }

        
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Otkazivanje dodavanja smeštaja");
            this.DialogResult = false;
            this.Close();
        }

        private void UpdateImages()
        {
            int id = accommodationRepository.GetCurrentId();
            foreach (ImageDTO image in Images)
            {
                image.EntityId = id;
                image.EntityType = EntityType.ACCOMMODATION;
                imageRepository.Update(image.ToImage());
            }
        }


        private void BrowseImageClick(object sender, RoutedEventArgs e)
        {
            

            OpenFileDialog openFileDialog = new OpenFileDialog();
            string filter = "Image files|";//(*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
            foreach (Model.Image image in imageRepository.FilterImages())
            {
                filter += image.Path.Split("\\")[5] + ";";
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


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
