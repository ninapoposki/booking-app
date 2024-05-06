using BookingApp.Domain.IRepositories;
using BookingApp.DTO;
using BookingApp.Services;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class GradeAccommodationVM : ViewModelBase
    {
        public AccommodationGradeService AccommodationGradeService;
        public ImageService imageService;
        public AccommodationReservationDTO selectedAccommodationReservation;
        public ObservableCollection<ImageDTO> Images { get; set; }
        public ImageDTO SelectedImage { get; set; }
        public AccommodationGradeDTO accommodationGradeDTO { get; set; }
        public int CleannessRadio { get; set; }
        public int CorrectnessRadio { get; set; }
        public string Comments { get; set; }
        public GradeAccommodationVM(AccommodationReservationDTO accommodationReservationDTO)
        {
            selectedAccommodationReservation = accommodationReservationDTO;
            AccommodationGradeService = new AccommodationGradeService(Injector.Injector.CreateInstance<IAccommodationGradeRepository>(),
                Injector.Injector.CreateInstance<IUserRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>(),
                Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>());
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            Images = new ObservableCollection<ImageDTO>();
            SelectedImage = new ImageDTO();
            accommodationGradeDTO = new AccommodationGradeDTO();
        }
        public void BrowseImageClick()
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = imageService.FilterImages();
            openFileDialog.InitialDirectory = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Resources\Images"));// Konvertuje relativnu u apsolutnu putanju
            if (openFileDialog.ShowDialog() == true) { AddImage(openFileDialog.FileName); }
        }
        public void AddImage(string absolutePath)
        {
            string referencePath = "../../../Resources/Images/";
            string[] pathPieces = absolutePath.Split('\\');
            string relativePath = (referencePath + pathPieces[pathPieces.Length - 1]);
            Images.Add(imageService.GetByPath(relativePath));
        }
       /* private string MakeRelativePath(string absolutePath)
        {
            string referencePath = "..\\..\\..\\Resources\\Images\\";
            string[] pathPieces = absolutePath.Split('\\');
            return referencePath + pathPieces[pathPieces.Length - 1];
        }*/

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
            var linkedAccommodationGradeDTO = AccommodationGradeService.GetOneAccommodationGrade(selectedAccommodationReservation, accommodationGradeDTO);
            AccommodationGradeService.Add(linkedAccommodationGradeDTO.ToAccommodationGrade());
        }
        private void UpdateImages()
        {
            int id = AccommodationGradeService.GetCurrentId();
            foreach (ImageDTO image in Images)
            {
                imageService.UpdateGuestImages(image, id);
            }
        }
    }
}