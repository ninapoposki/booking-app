using BookingApp.Domain.IRepositories;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.WPF.View.Guest;
using BookingApp.WPF.View.Owner;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class MyReservationsVM : ViewModelBase
    {
        private readonly AccommodationReservationService accommodationReservationService;
        private readonly AccommodationGradeService accommodationGradeService;
        private readonly CancelledReservationsService cancelledReservationsService;
        private readonly AccommodationService accommodationService;
        private readonly ReservationRequestService reservationRequestService;
        public ObservableCollection<AccommodationReservationDTO> AllReservations { get; }
        private AccommodationReservationDTO _selectedReservation;
        public AccommodationReservationDTO SelectedReservation{
            get => _selectedReservation;
            set{
                if (_selectedReservation != value){
                    _selectedReservation = value;
                    OnPropertyChanged(nameof(SelectedReservation)); }
            }
        }
        public MyReservationsVM(){
            accommodationReservationService = new AccommodationReservationService(Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
                           Injector.Injector.CreateInstance<IGuestRepository>(),
                           Injector.Injector.CreateInstance<IUserRepository>(),
                           Injector.Injector.CreateInstance<IAccommodationRepository>(),
                           Injector.Injector.CreateInstance<IImageRepository>(),
                           Injector.Injector.CreateInstance<ILocationRepository>(),
                           Injector.Injector.CreateInstance<IOwnerRepository>()); 
            accommodationGradeService = new AccommodationGradeService(Injector.Injector.CreateInstance<IAccommodationGradeRepository>(),
                Injector.Injector.CreateInstance<IUserRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>(),
                Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>());
            cancelledReservationsService = new CancelledReservationsService(Injector.Injector.CreateInstance<ICancelledReservationsRepository>(),
                Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
                Injector.Injector.CreateInstance<IGuestRepository>(),
                Injector.Injector.CreateInstance<IUserRepository>(),
                Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>(),
                Injector.Injector.CreateInstance<IReservationRequestRepository>());
            accommodationService = new AccommodationService(Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>());
            reservationRequestService = new ReservationRequestService(Injector.Injector.CreateInstance<IReservationRequestRepository>(),
                Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
                Injector.Injector.CreateInstance<IGuestRepository>(),
                Injector.Injector.CreateInstance<IUserRepository>(),
                Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>());
            AllReservations = new ObservableCollection<AccommodationReservationDTO>();
            Update();
        }

        public void Update(){
            AllReservations.Clear();
            foreach (AccommodationReservationDTO accommodationReservationDTO in accommodationReservationService.GetAll())
                AllReservations.Add(accommodationReservationService.GetOneReservation(accommodationReservationDTO));
        }
        public void AccommodationDataGrid(AccommodationReservationDTO selectedReservation) {
            int reservationId = accommodationGradeService.GetReservationId(selectedReservation.ToAccommodationReservation());
            if (!accommodationGradeService.IsReservationGraded(reservationId))
                AreDatesValid(selectedReservation);
            else
                MessageBox.Show("Accommodation is already graded.");
        }
        private void AreDatesValid(AccommodationReservationDTO accommodationReservationDTO){
            if (accommodationReservationDTO.EndDate > DateTime.Now)
                MessageBox.Show("Your days to stay has not finished yet!");
            else if (accommodationReservationService.IsOverFiveDays(accommodationReservationDTO.ToAccommodationReservation())){
                var dialog = new GradeAccommodation(accommodationReservationDTO);
                dialog.ShowDialog();
            }
            else
                MessageBox.Show("Grading is not possible, it has been more than 5 days.");
        }
        public void RateAccommodationClick() => AccommodationDataGrid(SelectedReservation);
        public void CancelReservationClick() {
             var accommodationDTO = accommodationService.GetAccommodation(SelectedReservation.AccommodationId);
            if (cancelledReservationsService.IsCancellationPeriodValid(accommodationDTO.ToAccommodation(), SelectedReservation.ToAccommodationReservation())){
                cancelledReservationsService.CancelReservation(accommodationService.GetById(accommodationDTO.Id), SelectedReservation.ToAccommodationReservation());
                MessageBox.Show("Your reservation has been successfully cancelled!");
                Update();
            }
            else if (cancelledReservationsService.IsReservationPassed(SelectedReservation.InitialDate))
                MessageBox.Show("You can't cancel reservation now!");
            else
                MessageBox.Show("The deadline for canceling the reservation has passed.");
        }
        public void ChangeReservationClick(){
            if (SelectedReservation != null) {
                if (cancelledReservationsService.IsReservationPassed(SelectedReservation.InitialDate)){
                    MessageBox.Show("You can't change reservation now!");
                } else {
                    var dates = reservationRequestService.GenerateNewDateRange(SelectedReservation.DaysToStay);
                    SelectedReservation = accommodationReservationService.GetOneReservation(SelectedReservation);
                    var dialog = new ChangeReservation(dates, SelectedReservation);
                    dialog.ShowDialog(); }
            } else
                MessageBox.Show("You didn't choose reservation!");
        }
    }
}
