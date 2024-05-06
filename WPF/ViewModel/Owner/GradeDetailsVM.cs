using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
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
        public MyICommand PreviousPicture {  get; private set ; }
        public MyICommand NextPicture { get; private set; }
        public GradeDetailsVM(AccommodationGradeDTO accommodationGrade)
        {
            AccommodationGrade = accommodationGrade;
            Images = new ObservableCollection<ImageDTO>();
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            UpdateImages();
            UpdateDisplayedImage();
            CanNext = CanNextImage();
            CanPrevious = CanPreviousImage();
            PreviousPicture = new MyICommand(PreviousImage);
            NextPicture = new MyICommand(NextImage);
        }

        public void UpdateImages()
        {
            var allImages = imageService.GetImagesForEntityType(EntityType.GUEST);
            var matchingImages = new ObservableCollection<ImageDTO>(imageService.GetImagesByAccommodation(AccommodationGrade.Id, allImages));
            AccommodationGrade.Images = matchingImages;
        }

        public void PreviousImage()
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                UpdateDisplayedImage();
                CanPrevious = CanPreviousImage();
                CanNext = CanNextImage();
            }
        }
        public void NextImage()
        {
            if (currentIndex < AccommodationGrade.Images.Count - 1)
            {
                currentIndex++;
                UpdateDisplayedImage();
                CanPrevious = CanPreviousImage();
                CanNext = CanNextImage();
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
        private bool CanNextImage()
        {
            return currentIndex < AccommodationGrade.Images.Count - 1;
        }
        private bool CanPreviousImage()
        {
            return currentIndex > 0; // Dugme za prethodnu sliku je omogućeno ako nismo na prvoj slici
        }
        private bool canNext;
        public bool CanNext
        {
            get { return canNext; }
            set
            {
                canNext = value;
                OnPropertyChanged("CanNext");
            }
        }
        private bool canPrevious;
        public bool CanPrevious
        {
            get { return canPrevious; }
            set
            {
                canPrevious = value;
                OnPropertyChanged("CanPrevious");
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
