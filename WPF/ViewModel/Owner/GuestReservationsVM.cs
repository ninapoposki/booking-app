using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Services;
using BookingApp.WPF.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Navigation;
using BookingApp.Utilities;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class GuestReservationsVM : ViewModelBase {
        public  GuestGradeService guestGradeService;
        public ImageService imageService;
        private AccommodationReservationService accommodationReservationService;
        public ObservableCollection<AccommodationReservationDTO> AllAccommodationReservations { get; set; }
        public int currentUserId;
        public AccommodationReservationDTO SelectedAccommodationReservation { get; set; }
        public MyICommand<AccommodationReservationDTO> GradeGuestCommand { get; private set; }
        public GuestReservationsVM(NavigationService navigation, int loggedInUserId) {
            guestGradeService = new GuestGradeService(Injector.Injector.CreateInstance<IGuestGradeRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>());
            accommodationReservationService = new AccommodationReservationService(Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
                           Injector.Injector.CreateInstance<IGuestRepository>(),
                           Injector.Injector.CreateInstance<IUserRepository>(),
                           Injector.Injector.CreateInstance<IAccommodationRepository>(),
                           Injector.Injector.CreateInstance<IImageRepository>(),
                           Injector.Injector.CreateInstance<ILocationRepository>(),
                           Injector.Injector.CreateInstance<IOwnerRepository>());
            AllAccommodationReservations = new ObservableCollection<AccommodationReservationDTO>();
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            SelectedAccommodationReservation = new AccommodationReservationDTO();
            currentUserId = loggedInUserId;

            Update();
            GradeGuestCommand = new MyICommand<AccommodationReservationDTO>(GradeGuest);
        }
        public void Update() {
            AllAccommodationReservations.Clear();
            var allImages = imageService.GetImagesForEntityType(EntityType.ACCOMMODATION);
            foreach (AccommodationReservationDTO accommodationReservationDTO in accommodationReservationService.GetAll())  {
                var updatedDTO = accommodationReservationService.GetOneReservation(accommodationReservationDTO);
                var matchingImages = new ObservableCollection<ImageDTO>(imageService.GetImagesByAccommodation(updatedDTO.AccommodationId, allImages));
                if (matchingImages.Count > 0)
                {
                    if (updatedDTO.Images == null)
                    {
                        updatedDTO.Images = new ObservableCollection<ImageDTO>();
                    }
                    updatedDTO.Images.Add(matchingImages[0]);
                } else
                {
                    updatedDTO.Images = new ObservableCollection<ImageDTO>();
                    updatedDTO.Images.Add(new ImageDTO { Path = @"\Resources\Icons\Owner\accommodation_placeholder.jpg" }); 
                }
                GuestDataGrid(updatedDTO);
                if (updatedDTO.Owner.UserId == currentUserId)
                {
                    AllAccommodationReservations.Add(updatedDTO) ; 
                }
                
            }     
        }
        
        public void GuestDataGrid(AccommodationReservationDTO selectedAccommodationReservation) {
                int reservationId = guestGradeService.GetReservationId(selectedAccommodationReservation);
                
                if (IsGuestGraded(reservationId)) {
                selectedAccommodationReservation.Message = "Already graded!";
                selectedAccommodationReservation.CanGradeGuest = false;
            }
            else{
                    AreDatesValid(selectedAccommodationReservation);
                }
            
        }/*

        public void GuestDataGridSelectionChanged() {
            if (SelectedAccommodationReservation != null){
                GuestDataGrid(SelectedAccommodationReservation);
            }
        }*/
        private void AreDatesValid(AccommodationReservationDTO accommodationReservationDTO) {
            if (accommodationReservationDTO.EndDate > DateTime.Now){
                //MessageBox.Show("Guest stay has not finished yet!");
                accommodationReservationDTO.Message = "Stay has not finished yet!";
                accommodationReservationDTO.CanGradeGuest = false;
            } else{
                if (accommodationReservationService.IsOverFiveDays(accommodationReservationDTO.ToAccommodationReservation()) && !IsGuestGraded(accommodationReservationDTO.Id)) {
                    // GradeGuestWindow gradeGuestWindow = new GradeGuestWindow(accommodationReservationDTO);
                    // gradeGuestWindow.ShowDialog();
                    accommodationReservationDTO.Message = "Not graded yet!";
                    accommodationReservationDTO.CanGradeGuest = true;
                } else {  //MessageBox.Show("Grading is not possible, it has been more than 5 days.");

                    accommodationReservationDTO.Message = "It has been more than 5 days!";
                    accommodationReservationDTO.CanGradeGuest = false;
                }
            }
        }
        private bool IsGuestGraded(int reservationId) {
            return guestGradeService.IsGuestGraded(reservationId); 
        }

        public void GradeGuest(AccommodationReservationDTO reservation)
        {
            
            GradeGuestWindow gradeGuest = new GradeGuestWindow(reservation);
            gradeGuest.ShowDialog();
            Update();
        }


        
    }
}