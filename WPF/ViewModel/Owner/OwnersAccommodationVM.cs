using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel.Owner
{
    
    public class OwnersAccommodationVM : ViewModelBase
    {
        
        public ImageService imageService;
        private AccommodationService accommodationService;
        public ObservableCollection<AccommodationDTO> AllAccommodations { get; set; }
        public int currentUserId;
        public AccommodationDTO SelectedAccommodation { get; set; }

        public OwnersAccommodationVM(int loggedInUserId)
        {
            accommodationService = new AccommodationService(Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>());
            AllAccommodations = new ObservableCollection<AccommodationDTO>();
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            SelectedAccommodation = new AccommodationDTO();
            currentUserId = loggedInUserId;
            Update();
        }

        public void Update()
        {
            AllAccommodations.Clear();
            var allImages = imageService.GetImagesForEntityType(EntityType.ACCOMMODATION);
            foreach (AccommodationDTO accommodationDTO in accommodationService.GetAll())
            {
                var updatedDTO = accommodationService.GetAccommodation(accommodationDTO.Id);
                var matchingImages = new ObservableCollection<ImageDTO>(imageService.GetImagesByAccommodation(updatedDTO.Id, allImages));
                if (matchingImages.Count > 0)
                {
                    if (updatedDTO.Images == null)
                    {
                        updatedDTO.Images = new ObservableCollection<ImageDTO>();
                    }
                    updatedDTO.Images.Add(matchingImages[0]);
                }
                
                if (updatedDTO.OwnerId == currentUserId)
                {
                    AllAccommodations.Add(updatedDTO);
                }

            }
        }
    }
}
