using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
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
        public ObservableCollection<AccommodationType> Types { get; set; }
        public List<LocationDTO> LocationComboBox { get; set; }

        private ImageDTO selectedImage;
        public List<ImageDTO> Images { get; set; }
        public AddAccommodation(AccommodationRepository accommodationRepository)
        {
            InitializeComponent();
            DataContext = this;
            Accommodation = new AccommodationDTO();
            this.accommodationRepository = accommodationRepository;

            selectedImage = new ImageDTO();
            Images = new List<ImageDTO>();
            imageRepository = new ImageRepository();


            locationRepository = new LocationRepository();
            LocationComboBox = new List<LocationDTO>();

            var type = Enum.GetValues(typeof(AccommodationType)).Cast<AccommodationType>();
            Types = new ObservableCollection<AccommodationType>(type);
             LoadLocations();

           
    }

       
        private void LoadLocations()
        {
                   
            LocationComboBox.Clear();
            foreach (Location location in locationRepository.GetAll()) LocationComboBox.Add(new LocationDTO(location));
        }
    
   


    public delegate void AccommodationAddedEventHandler(object sender, EventArgs e);
        public event AccommodationAddedEventHandler AccommodationAdded;
        // Metoda koja se poziva kada se pritisne dugme "Add"
        private void AddAccommodationButton_Click(object sender, RoutedEventArgs e)
        {
            // Implementacija koda za dodavanje smještaja
            foreach (ImageDTO image in Images)
            {
                image.EntityId = accommodationRepository.GetCurrentId();
                image.EntityType = EntityType.ACCOMODATION;
                imageRepository.Update(image.ToImage());

            }

            LocationDTO selectedLocation = (LocationDTO)locationComboBox.SelectedItem;
            

            if (selectedLocation != null) Accommodation.IdLocation = selectedLocation.Id;
            else
            {
                MessageBox.Show("Choose a tour location!");
            }


            if (Accommodation.IsValid)
            {
                MessageBox.Show("Dodavanje smeštaja");
               
                accommodationRepository.Add(Accommodation.ToAccommodation());
                //this.DialogResult = true;
                AccommodationAdded?.Invoke(this, EventArgs.Empty);
                Close();
                
            }
            else
            {
                MessageBox.Show("Accommodation cannot be created. All fields are not valid.");
            }
        }

        // Metoda koja se poziva kada se pritisne dugme "Cancel"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Implementacija koda za otkazivanje dodavanja smještaja
            MessageBox.Show("Otkazivanje dodavanja smeštaja");
            this.DialogResult = false;
            this.Close();
        }

        /* private void AddImageButton_Click(object sender, RoutedEventArgs e)
         {
             AddAccommodationImage addAccommodationImageWindow = new AddAccommodationImage();
             addAccommodationImageWindow.ShowDialog();
         }*/
        private void BrowseAndLoadPictureClick(object sender, RoutedEventArgs e)
        {
            PictureBrowseWindow pictureBrowseWindow = new PictureBrowseWindow();
            pictureBrowseWindow.ShowDialog();
            selectedImage = pictureBrowseWindow.selectedImage;
            Images.Add(selectedImage);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
