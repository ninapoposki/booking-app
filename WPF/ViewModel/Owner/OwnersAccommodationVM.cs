using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using BookingApp.WPF.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Owner
{

    public class OwnersAccommodationVM :  ViewModelBase
    {
        
        public ImageService imageService;
        private AccommodationService accommodationService;
        public ObservableCollection<AccommodationDTO> AllAccommodations { get; set; }
        public int UserId;
        public AccommodationDTO SelectedAccommodation { get; set; }

        private int currentUserId;
        public int CurrentUserId
        {
            //  get { return currentUserId; }
            //  set { SetProperty(ref currentUserId, value); }
            get => currentUserId;
            set { currentUserId = value; OnPropertyChanged("CurrentUserId"); }
        }
        private string filter;
        public string Filter
        {
            get => filter;
            set { filter = value; OnPropertyChanged(nameof(Filter)); }
        }
       

        public MyICommand AddAccommodation { get; private set; }
        public AddAccommodationVM AddAccommodationVM { get; set; }
        public MyICommand RemoveAccommodation { get; private set; }
        public MyICommand SearchAccommodation {  get; private set; }
        public MyICommand<AccommodationDTO> AccommodationDetails { get; private set; }
        public MyICommand<AccommodationDTO> AccommodationStatistics {  get; private set; }
        public MyICommand<AccommodationDTO> AccommodationRenovations {  get; private set; }
        public BindableBase CurrentVM { get; set; }

        public OwnersAccommodationVM(NavigationService navigationService, int currentUserId)
        {
            accommodationService = new AccommodationService(Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>());
            AllAccommodations = new ObservableCollection<AccommodationDTO>();
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            SelectedAccommodation = new AccommodationDTO();
            // currentUserId = CurrentUserId;
            CurrentUserId = SharedData.Instance.CurrentUserId;

            Update();

            
            
            AddAccommodation = new MyICommand(add);
            RemoveAccommodation = new MyICommand(DeleteAccommodation);
            SearchAccommodation = new MyICommand(Search);
            AccommodationDetails = new MyICommand<AccommodationDTO>(OwnersAcommodationDetails);
            AccommodationStatistics = new MyICommand<AccommodationDTO>(GetStatistics);
            AccommodationRenovations = new MyICommand<AccommodationDTO>(GetRenovations);
        }
        public void DeleteAccommodation()
        {
            if (SelectedAccommodation != null)
            {
                accommodationService.Delete(SelectedAccommodation);
                foreach (var image in SelectedAccommodation.Images)
                {
                    
                        imageService.ResetImage(image);
                    
                   
                }
                Update(); 
            }
        }
        public void Update()
        {
            AllAccommodations.Clear();
            var allImages = imageService.GetImagesForEntityType(EntityType.ACCOMMODATION);
            foreach (AccommodationDTO accommodationDTO in accommodationService.GetAll())
            {
                
                var updatedDTO = accommodationService.GetAccommodation(accommodationDTO.Id);
                if(accommodationDTO.Capacity==1) { updatedDTO.GuestMessage= " guest";  }else { updatedDTO.GuestMessage = " guests"; }
                if (accommodationDTO.CancellationPeriod == 1) { updatedDTO.DaysMessage = " day"; } else { updatedDTO.DaysMessage = " days"; }
                if (accommodationDTO.MinStayDays == 1) { updatedDTO.MinDaysMessage = " day"; } else { updatedDTO.MinDaysMessage = " days"; }
                var matchingImages = new ObservableCollection<ImageDTO>(imageService.GetImagesByAccommodation(updatedDTO.Id, allImages));
                if (matchingImages.Count > 0)
                {
                    if (updatedDTO.Images == null)
                    {
                        updatedDTO.Images = new ObservableCollection<ImageDTO>();
                    }
                    updatedDTO.Images.Add(matchingImages[0]);
                }
                else
                {
                    updatedDTO.Images.Add(new ImageDTO { Path = @"\Resources\Icons\Owner\accommodation_placeholder.jpg" });
                }
                
                if (updatedDTO.OwnerId == currentUserId)
                {
                    AllAccommodations.Add(updatedDTO);
                }

            }
        }

        public void Search()
        {
            Update();
            var filteredAccommodations = FilterAccommodations();
            AllAccommodations.Clear();
            filteredAccommodations.ForEach(accommodation => AllAccommodations.Add(accommodation));
            Filter = string.Empty;
        }
        public int SearchParse(string Filter)
        {
            var number = -1;
            if (int.TryParse(Filter, out int num))
            {
                number = num;
            }
            return number;
        }
        private List<AccommodationDTO> FilterAccommodations()
        {
            
                return AllAccommodations
                .Where(accommodation =>
                    (string.IsNullOrEmpty(Filter) || accommodation.Name.ToLower().Contains(Filter.ToLower()) || accommodation.Location.City.ToLower().Contains(Filter.ToLower())) || accommodation.Location.Country.ToLower().Contains(Filter.ToLower()) || accommodation.Capacity.ToString() == Filter || accommodation.MinStayDays.ToString() == Filter || accommodation.CancellationPeriod.ToString() == Filter || accommodation.AccommodationType.ToString().ToLower() == Filter.ToLower() ).ToList(); 
                   
        }
        public void OwnersAcommodationDetails(AccommodationDTO selectedaccommodation)
        {
            if (selectedaccommodation != null)
            {
                AccommodationDetails details = new AccommodationDetails(selectedaccommodation);
                details.ShowDialog();
            }
        }
        public void GetStatistics(AccommodationDTO selectedaccommodation)
        {
            if(selectedaccommodation != null)
            {
                AccommodationStatistics statistics = new AccommodationStatistics(selectedaccommodation);
                statistics.ShowDialog();
            }
        }

       
        public void add()
        {
            AddAccommodation addAccommodationWindow = new AddAccommodation(CurrentUserId);
            addAccommodationWindow.ShowDialog();
            Update();
        }
        public void GetRenovations(AccommodationDTO accommodationDTO)
        {
            if(SelectedAccommodation != null)
            {
                Renovations renovations = new Renovations(accommodationDTO);
                renovations.ShowDialog();

            }
        }
        
    }
    
        
    
}
