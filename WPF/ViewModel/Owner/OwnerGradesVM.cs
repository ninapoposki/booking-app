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
using System.Windows;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class OwnerGradesVM : BindableBase, INotifyPropertyChanged{
        private AccommodationGradeService accommodationGradeService;
        private GuestGradeService guestGradeService;
        public AccommodationReservationService accommodationReservationService;
        public NavigationService NavigationService { get; set; }
        public ObservableCollection<AccommodationGradeDTO> AllOwnerGrades { get; set; }
        public AccommodationGradeDTO SelectedAccommodationGrade { get; set; }
        public int ownerId;
        private int currentUserId;
        public int CurrentUserId
        {
            get { return currentUserId; }
            set { SetProperty(ref currentUserId, value); }
        }

        public MyICommand<AccommodationGradeDTO> GradeDetails { get; private set; }
        //public AddAccommodationVM AddAccommodationVM { get; set; }
        //public BindableBase CurrentVM { get; set; }
        public OwnerGradesVM(NavigationService navigationService, int currentUserId)
        { 
            NavigationService = navigationService;
            guestGradeService = new GuestGradeService(Injector.Injector.CreateInstance<IGuestGradeRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>());
            accommodationGradeService = new AccommodationGradeService(Injector.Injector.CreateInstance<IAccommodationGradeRepository>(),
                Injector.Injector.CreateInstance<IUserRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>(),
                Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>());
            accommodationReservationService = new AccommodationReservationService(Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
                           Injector.Injector.CreateInstance<IGuestRepository>(),
                           Injector.Injector.CreateInstance<IUserRepository>(),
                           Injector.Injector.CreateInstance<IAccommodationRepository>(),
                           Injector.Injector.CreateInstance<IImageRepository>(),
                           Injector.Injector.CreateInstance<ILocationRepository>(),
                           Injector.Injector.CreateInstance<IOwnerRepository>());
            AllOwnerGrades = new ObservableCollection<AccommodationGradeDTO>();
            SelectedAccommodationGrade = new AccommodationGradeDTO();
            ownerId = guestGradeService.GetByUserId(currentUserId).Id;// sad zakom
           // CurrentUserId = SharedData.Instance.CurrentUserId;
            //ownerId = CurrentUserId;
            GradeDetails = new MyICommand<AccommodationGradeDTO>(OwnersGradeDetails);
            Update();
        }
        public void Update(){
            AllOwnerGrades.Clear();
            foreach (AccommodationGradeDTO accommodationGradeDTO in accommodationGradeService.GetAll()){
                if(ownerId == accommodationGradeDTO.OwnerId && guestGradeService.IsGuestGraded(accommodationGradeDTO.ReservationId)) { 
                    var updatedDTO = accommodationGradeDTO;
                    updatedDTO.Grade = GetAverageGrade(accommodationGradeDTO);
                    updatedDTO.AccommodationReservation = GetReservation(accommodationGradeDTO.ReservationId);
                    AllOwnerGrades.Add(updatedDTO);
                }
            }
        }
        public AccommodationReservationDTO GetReservation(int reservationId) {
           // var reservation = accommodationReservationService.GetById(reservationId);
            AccommodationReservationDTO accommodationReservationDTO = new AccommodationReservationDTO(accommodationReservationService.GetById(reservationId));
            //accommodationReservationDTO.Accommodation = accommodationReservationService.accommodationService.GetByIdDTO(reservation.AccommodationId); ;
            // accommodationReservationDTO.Guest = accommodationReservationService.guestService.GetByIdDTO(reservation.GuestId);
            accommodationReservationDTO.Accommodation = accommodationReservationService.accommodationService.GetByIdDTO(accommodationReservationDTO.AccommodationId); ;
             accommodationReservationDTO.Guest = accommodationReservationService.guestService.GetByIdDTO(accommodationReservationDTO.GuestId);
            return accommodationReservationDTO;
        }
        public void OwnersGradeDetails(AccommodationGradeDTO selectedGrade)
        {
            if (selectedGrade != null)
            {
                GradeDetails details = new GradeDetails(selectedGrade);
                details.ShowDialog();
            }
        }
        public int GetAverageGrade(AccommodationGradeDTO gradeDTO)
        {
            double gradeSum = gradeDTO.Cleanliness + gradeDTO.Correctness;
            double averageGrade = gradeSum / 2.0;
            return (int)averageGrade;
        }

        public event PropertyChangedEventHandler? PropertyChanged;


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
