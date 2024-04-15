using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class GradeDetailsVM : ViewModelBase
    {
        public AccommodationGradeDTO AccommodationGrade { get; set; }
        public ObservableCollection<ImageDTO> Images { get; set; }
        public ImageService imageService { get; set; }
        public int currentIndex = 0;
        public GradeDetailsVM(AccommodationGradeDTO accommodationGrade)
        {
            AccommodationGrade = accommodationGrade;
            Images = new ObservableCollection<ImageDTO>();
            imageService = new ImageService();
            UpdateImages();
            UpdateDisplayedImage();
        }

        public void UpdateImages()
        {
            var allImages = imageService.GetImagesForEntityType(EntityType.GUEST);
            var matchingImages = new ObservableCollection<ImageDTO>(imageService.GetImagesByAccommodation(AccommodationGrade.Id, allImages));
            AccommodationGrade.Images = matchingImages;
        }

        public void PreviousClick()
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                UpdateDisplayedImage();
            }
        }
        public void NextClick()
        {
            if (currentIndex < AccommodationGrade.Images.Count - 1)
            {
                currentIndex++;
                UpdateDisplayedImage();
            }
        }
        private void UpdateDisplayedImage()
        {
            if (AccommodationGrade.Images.Count > 0 && currentIndex < AccommodationGrade.Images.Count){
                CurrentImage = AccommodationGrade.Images[currentIndex];
            }else {
                CurrentImage = null;
            }
        }

        private ImageDTO currentImage;
        public ImageDTO CurrentImage
        {
            get { return currentImage; }
            set
            {
                currentImage = value;
                OnPropertyChanged("CurrentImage");
            }
        }
    }
}
