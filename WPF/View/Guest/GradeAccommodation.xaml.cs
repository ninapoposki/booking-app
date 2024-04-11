using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BookingApp.WPF.View.Guest
{
    /// <summary>
    /// Interaction logic for GradeAccommodation.xaml
    /// </summary>
    public partial class GradeAccommodation : Window
    {
        // public AccommodationGradeRepository accommodationGradeRepository { get; set; }
        public AccommodationGradeRepository accommodationGradeRepository=new AccommodationGradeRepository();
        public AccommodationRepository accommodationRepository = new AccommodationRepository();
        public OwnerRepository ownerRepository = new OwnerRepository();
        public LocationRepository locationRepository = new LocationRepository();
        private ImageRepository imageRepository=new ImageRepository();
        private ImageDTO SelectedImage;
        public ObservableCollection<ImageDTO> Images { get; set; }

        public AccommodationReservationDTO selectedAccommodationReservation;
        public AccommodationGradeDTO accommodationGradeDTO { get; set; }
        public AccommodationDTO accommodationDTO;
        //public GradeAccommodation(AccommodationGradeRepository accommodationGradeRepository, AccommodationReservationDTO accommodationReservationDTO
        public GradeAccommodation(AccommodationReservationDTO accommodationReservationDTO)
        {
            InitializeComponent();
            selectedAccommodationReservation = accommodationReservationDTO;
            DataContext = selectedAccommodationReservation;
            Images = new ObservableCollection<ImageDTO>();//dodato
            SelectedImage = new ImageDTO();
            //this.accommodationGradeRepository = new AccommodationGradeRepository();
            accommodationGradeDTO = new AccommodationGradeDTO();
            accommodationDTO=new AccommodationDTO(); //ne znamd a li ovo treba
        }

        private int GetSelectedRadioButtonValue(StackPanel panel)
        {
            foreach (var radioButton in panel.Children)
            {
                if (radioButton is RadioButton && ((RadioButton)radioButton).IsChecked == true)
                {
                    return int.Parse(((RadioButton)radioButton).Content.ToString());
                }
            }
            return 0;
        }


        private void ConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            UpdateImages();
            int cleanliness = GetSelectedRadioButtonValue(Cleanliness);
            int correctness = GetSelectedRadioButtonValue(Correctness);
            AccommodationRepository accommodationRepository = new AccommodationRepository();
            string comment = CommentsTextBox.Text;

            accommodationGradeDTO.Cleanliness = cleanliness;
            accommodationGradeDTO.Correctness = correctness;
            accommodationGradeDTO.Comment = comment;

            Accommodation accommodation=accommodationRepository.GetById(selectedAccommodationReservation.AccommodationId);
            BookingApp.Domain.Model.Owner owner = ownerRepository.GetById(accommodation.OwnerId);
            Location location = locationRepository.GetById(accommodation.IdLocation); 
            AccommodationReservationDTO accommodationReservationDTO = new AccommodationReservationDTO(selectedAccommodationReservation.ToAccommodationReservation(),accommodation,location, owner);

            accommodationGradeDTO.OwnerId = accommodation.OwnerId; //ili preko repoziturojuma probaj
            accommodationGradeDTO.ReservationId = selectedAccommodationReservation.Id;
            accommodationGradeRepository.Add(accommodationGradeDTO.ToAccommodationGrade());
            this.DialogResult = true;
            MessageBox.Show("You have successfully rated the accommodation!");
            Close();

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
            foreach (Domain.Model.Image image in imageRepository.FilterImages())
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
            Domain.Model.Image image = imageRepository.FindByPath(relativePath);
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

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
