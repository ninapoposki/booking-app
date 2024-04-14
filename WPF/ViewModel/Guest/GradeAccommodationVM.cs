using BookingApp.DTO;
using BookingApp.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class GradeAccommodationVM:ViewModelBase
    {
        public AccommodationGradeService AccommodationGradeService;
        public ImageService imageService;
        public AccommodationReservationDTO selectedAccommodationReservation;
        public ObservableCollection<ImageDTO> Images { get; set; }
        public ImageDTO SelectedImage { get; set; }
        public AccommodationGradeDTO accommodationGradeDTO { get; set; }
        public GradeAccommodationVM(AccommodationReservationDTO accommodationReservationDTO)
        {
            selectedAccommodationReservation = accommodationReservationDTO;
            AccommodationGradeService = new AccommodationGradeService();
            imageService = new ImageService();
            Images = new ObservableCollection<ImageDTO>();
            SelectedImage = new ImageDTO();
            accommodationGradeDTO = new AccommodationGradeDTO();
        }
        private void UpdateImages()
        {
            int id = AccommodationGradeService.GetCurrentId(); 
            foreach (ImageDTO image in Images)
            {
              imageService.UpdateGuestImages(image, id);
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
            if (SelectedImage != null)
            {
                Images.Remove(SelectedImage);
            }
        }



        public void ConfirmButtonClick(int cleanness, int followingrules)
        {
            UpdateImages();
            CleannessRadio = cleanness;
            CorrectnessRadio = followingrules;
            accommodationGradeDTO.Cleanliness = CleannessRadio;
            accommodationGradeDTO.Correctness = CorrectnessRadio;
            accommodationGradeDTO.Comment = Comments;
            // accommodationGradeDTO.OwnerId = selectedAccommodationReservation.OwnerId; smisli??
            //dodaj ownera nekako sad iz rezervacije preko accommodationa
            // accommodationGradeDTO.ReservationId = selectedAccommodationReservation.Id;
            var linkedAccommodationGradeDTO = AccommodationGradeService.GetOneAccommodationGrade(selectedAccommodationReservation, accommodationGradeDTO);
            AccommodationGradeService.Add(linkedAccommodationGradeDTO.ToAccommodationGrade());

        }


        private int cleannessRadio;
        public int CleannessRadio
        {
            get { return cleannessRadio; }
            set
            {
                if (cleannessRadio != value)
                {
                    cleannessRadio = value;
                    OnPropertyChanged("CleannessRadio");
                }
            }
        }

        private int correctnessRadioButtonChecked;
        public int CorrectnessRadio
        {
            get { return correctnessRadioButtonChecked; }
            set
            {
                if (correctnessRadioButtonChecked != value)
                {
                    correctnessRadioButtonChecked = value;
                    OnPropertyChanged("CorrectnessRadioButtonChecked");
                }
            }
        }

        private string comment;
        public string Comments
        {
            get { return comment; }
            set
            {
                if (comment != value)
                {
                    comment = value;
                    OnPropertyChanged("Comment");
                }
            }
        }

    }
}
