using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class AccommodationDetailsVM : ViewModelBase
    {
        public AccommodationDTO AccommodationDTO { get; set; }
        public ObservableCollection<ImageDTO> Images { get; set; }
        public ImageService imageService { get; set; }
        public int currentIndex = 0;
        public MyICommand PreviousPicture { get; private set; }
        public MyICommand NextPicture { get; private set; }
        public AccommodationDetailsVM(AccommodationDTO selectedAccommodation)
        {
            AccommodationDTO = selectedAccommodation;
            Images = new ObservableCollection<ImageDTO>();
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            UpdateImages();
            UpdateDisplayedImage();
            
            PreviousPicture = new MyICommand(PreviousImage);
            NextPicture = new MyICommand(NextImage, CanNextImage);
            UpdateIconPath(selectedAccommodation);
            CanNext = CanNextImage();
            CanPrevious = CanPreviousImage();

           

            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "currentIndex")
                {
                    CanNext = CanNextImage(); // Ažuriranje CanNextImage kada currentIndex promeni vrednost
                }
            };
        }
        
        public void UpdateImages()
        {
            var allImages = imageService.GetImagesForEntityType(EntityType.ACCOMMODATION);
            var matchingImages = new ObservableCollection<ImageDTO>(imageService.GetImagesByAccommodation(AccommodationDTO.Id, allImages));
            AccommodationDTO.Images = matchingImages;
            NoButton = true;
            if (matchingImages.Count > 0)
            {
                NoButton = false;
            }
        }

        public void PreviousImage()
        {
            
            
            if (currentIndex > 0)
            {
                currentIndex--;
                UpdateDisplayedImage();
                CanNext = CanNextImage();
                CanPrevious = CanPreviousImage();
                
            }
            
        }
        public void NextImage()
        {
            if (currentIndex < AccommodationDTO.Images.Count - 1)
            {
                currentIndex++;
                UpdateDisplayedImage();
                CanNext = CanNextImage();
                CanPrevious = CanPreviousImage();
            }
          
        }
        private bool CanNextImage()
        {
            return currentIndex < AccommodationDTO.Images.Count - 1;
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
        private bool noButton;
        public bool NoButton
        {
            get { return noButton; }
            set
            {
                noButton = value;
                OnPropertyChanged("NoButton");
            }
        }
        private void UpdateDisplayedImage()
        {
            if (AccommodationDTO.Images.Count > 0 && currentIndex < AccommodationDTO.Images.Count)
            {
                CurrentImage = AccommodationDTO.Images[currentIndex];
            }
            else
            {
                CurrentImage = null;
            }
        }
        public void UpdateIconPath(AccommodationDTO accommodationDTO)
        {
            if(accommodationDTO.AccommodationType.ToString() == "CABIN")
            {
                Path = @"..\..\..\Resources\Images\Owner\icon_cottage.png";
            }else if(accommodationDTO.AccommodationType.ToString() == "APARTMENT")
            {
                Path = @"..\..\..\Resources\Images\Owner\icon_hotel1.png";
            }
            else
            {
                Path = @"..\..\..\Resources\Images\Owner\icon_house.png";
            }
            
        }
        private string path;
        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                OnPropertyChanged("Path");
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
