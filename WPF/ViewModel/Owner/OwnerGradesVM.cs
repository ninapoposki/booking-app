using BookingApp.Domain.IRepositories;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.WPF.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class OwnerGradesVM : ViewModelBase
    {
        public OwnerService ownerService;
        public UserService userService;
        private AccommodationGradeService accommodationGradeService;
        private GuestGradeService guestGradeService;
        public AccommodationService accommodationService;
        public AccommodationReservationService accommodationReservationService;
        public GuestService guestService;

        public ObservableCollection<AccommodationGradeDTO> AllOwnerGrades { get; set; }
        public AccommodationGradeDTO SelectedAccommodationGrade { get; set; }
        public int ownerId;

        public OwnerGradesVM(string username) {
            ownerService = new OwnerService();
            userService = new UserService(Injector.Injector.CreateInstance<IUserRepository>());
            guestGradeService = new GuestGradeService();
            accommodationGradeService = new AccommodationGradeService();
            accommodationService = new AccommodationService();
            accommodationReservationService = new AccommodationReservationService();
            guestService = new GuestService();
            AllOwnerGrades = new ObservableCollection<AccommodationGradeDTO>();
            SelectedAccommodationGrade = new AccommodationGradeDTO();
            ownerId = ownerService.GetByUserId(userService.GetByUsername(username).Id).Id;
            Update();
        }
        public void Update()
        {
            AllOwnerGrades.Clear();
           
            foreach (AccommodationGradeDTO accommodationGradeDTO in accommodationGradeService.GetAll())
            {
                if(ownerId == accommodationGradeDTO.OwnerId && guestGradeService.IsGuestGraded(accommodationGradeDTO.ReservationId)) { 
                   
                    var updatedDTO = accommodationGradeDTO;
                    updatedDTO.Owner = GetOwner(accommodationGradeDTO.OwnerId);
                    updatedDTO.AccommodationReservation = GetReservation(accommodationGradeDTO.ReservationId);
                    updatedDTO.AccommodationReservation.Guest = GetGuest(accommodationGradeDTO.AccommodationReservation.GuestId);
                    updatedDTO.AccommodationReservation.Accommodation = GetAccommodation(accommodationGradeDTO.AccommodationReservation.AccommodationId);

                    AllOwnerGrades.Add(updatedDTO);
                }
            }
        }
        public GuestDTO GetGuest(int guestId)
        {
            var guest = guestService.GetById(guestId);
            GuestDTO guestDTO = new GuestDTO(guest);

            return guestDTO;
        }
        public OwnerDTO GetOwner(int ownerId)
        {
            var owner = ownerService.GetById(ownerId);
            OwnerDTO ownerDTO = new OwnerDTO(owner);

            return ownerDTO;
        }
        public AccommodationReservationDTO GetReservation(int reservationId)
        {
            var reservation = accommodationReservationService.GetById(reservationId);
            AccommodationReservationDTO accommodationReservationDTO = new AccommodationReservationDTO(reservation);

            return accommodationReservationDTO;
        }

        public AccommodationDTO GetAccommodation(int accommodationId)
        {
            var accommodation = accommodationService.GetById(accommodationId);
            AccommodationDTO accommodationDTO = new AccommodationDTO(accommodation);

            return accommodationDTO;
        }

        public void GradeDetailsClick()
        {
            
            if (SelectedAccommodationGrade != null)
            {
                GradeDetails details = new GradeDetails(SelectedAccommodationGrade);
                details.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select an accommodation grade first.");
            }
        }


    }
}
