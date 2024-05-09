using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using BookingApp.WPF.View.Guest;
using BookingApp.WPF.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class MyReservationsVM : ViewModelBase
    {
        //private readonly ImageService imageService; placeholder
        public ObservableCollection<AccommodationReservationDTO> PastReservations { get; private set; }
        public ObservableCollection<AccommodationReservationDTO> FutureReservations { get; private set; }

        private bool _showPastReservations = true; 
        public bool ShowPastReservations
        {
            get => _showPastReservations;
            set
            {
                if (_showPastReservations != value)
                {
                    _showPastReservations = value;
                    OnPropertyChanged(nameof(ShowPastReservations));
                    OnPropertyChanged(nameof(ShowFutureReservations));
                    OnPropertyChanged(nameof(PastVisibility));
                    OnPropertyChanged(nameof(FutureVisibility));
                    FilterReservations(); 
                }
            }
        }
        public bool ShowFutureReservations
        {
            get => !_showPastReservations;
            set
            {
                if (_showPastReservations == value)
                {
                    ShowPastReservations = !value; 

                }
            }
        }
        public Visibility PastVisibility => ShowPastReservations ? Visibility.Visible : Visibility.Collapsed;
        public Visibility FutureVisibility => ShowPastReservations ? Visibility.Collapsed : Visibility.Visible;
        private readonly AccommodationReservationService accommodationReservationService;
        private readonly AccommodationGradeService accommodationGradeService;
        private readonly CancelledReservationsService cancelledReservationsService;
        private readonly AccommodationService accommodationService;
        private readonly ReservationRequestService reservationRequestService;
        public  NavigationService navigationService { get; set; }
        public ObservableCollection<AccommodationReservationDTO> AllReservations { get; }
        private AccommodationReservationDTO _selectedReservation;
        public AccommodationReservationDTO SelectedReservation{
            get => _selectedReservation;
            set{
                if (_selectedReservation != value){
                    _selectedReservation = value;
                    OnPropertyChanged(nameof(SelectedReservation)); }
                    CancelReservationCommand.RaiseCanExecuteChanged();
                    ChangeReservationCommand.RaiseCanExecuteChanged();
                    RateReservationCommand.RaiseCanExecuteChanged();
            }
        }
        public MyICommand <AccommodationReservationDTO> ChangeReservationCommand { get; set; }
        public MyICommand <AccommodationReservationDTO> CancelReservationCommand { get; set; }
        public MyICommand <AccommodationReservationDTO> RateReservationCommand { get; set; }
        public MyReservationsVM(NavigationService navigationService){
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
           // imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>()); placeholder
            AllReservations = new ObservableCollection<AccommodationReservationDTO>();
            PastReservations=new ObservableCollection<AccommodationReservationDTO>();
            FutureReservations=new ObservableCollection<AccommodationReservationDTO>();
            this.navigationService = navigationService;
            ChangeReservationCommand = new MyICommand<AccommodationReservationDTO>(OnChangeReservation); 
            CancelReservationCommand = new MyICommand<AccommodationReservationDTO>(OnCancelReservation);
            RateReservationCommand = new MyICommand<AccommodationReservationDTO>(CanRateAccommodation);
            Update();
        }
        
         public void Update(){
             AllReservations.Clear();
             foreach (AccommodationReservationDTO accommodationReservationDTO in accommodationReservationService.GetAll())
                 AllReservations.Add(accommodationReservationService.GetOneReservation(accommodationReservationDTO));
             FilterReservations(); 

         }
        //placeholder
       /* public void Update()
        {
            AllReservations.Clear();
            foreach (AccommodationReservationDTO accommodationReservationDTO in accommodationReservationService.GetAll())
            {
                var allImages = imageService.GetImagesForEntityType(EntityType.ACCOMMODATION);
                var matchingImages = new ObservableCollection<ImageDTO>(imageService.GetImagesByAccommodation(accommodationReservationDTO.AccommodationId, allImages));
                if (matchingImages.Count == 0)
                {
                    matchingImages.Add(new ImageDTO { Path = @"\Resources\Images\placeholder_accommodation.jpg" });
                }

                accommodationReservationDTO.Images = matchingImages.ToList();
                AllReservations.Add(accommodationReservationService.GetOneReservation(accommodationReservationDTO));
            }
               
            FilterReservations();
        }*/
            private void FilterReservations()
        {
            if (ShowPastReservations)
            {
                PastReservations.Clear();
                var past = AllReservations.Where(r => r.EndDate < DateTime.Now).ToList();
                past.ForEach(PastReservations.Add);
            }
            else
            {
                FutureReservations.Clear();
                var future = AllReservations.Where(r => r.InitialDate > DateTime.Now).ToList();
                future.ForEach(FutureReservations.Add);
            }
        }

        /*public void AccommodationDataGrid(AccommodationReservationDTO selectedReservation) {
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
        public void RateAccommodationClick() => AccommodationDataGrid(SelectedReservation);*/

        private void AreDatesValid(AccommodationReservationDTO accommodationReservationDTO)
        {
            if (accommodationReservationService.IsOverFiveDays(accommodationReservationDTO.ToAccommodationReservation()))
            {
                GradeAccommodation gradeAccommodation = new GradeAccommodation(navigationService,accommodationReservationDTO);
                navigationService.Navigate(gradeAccommodation);
            }
            else
                MessageBox.Show("Grading is not possible, it has been more than 5 days.");
        }
        public void AccommodationDataGrid(AccommodationReservationDTO selectedReservation)
        {
            int reservationId = accommodationGradeService.GetReservationId(selectedReservation.ToAccommodationReservation());
            if (!accommodationGradeService.IsReservationGraded(reservationId))
                AreDatesValid(selectedReservation);
            else
                MessageBox.Show("Accommodation is already graded.");
        }
        private void CanRateAccommodation(AccommodationReservationDTO SelectedReservation)
        {
            AccommodationDataGrid(SelectedReservation);
        }

        private void OnChangeReservation(AccommodationReservationDTO SelectedReservation)
        {
            var dates = reservationRequestService.GenerateNewDateRange(SelectedReservation.DaysToStay);
            SelectedReservation = accommodationReservationService.GetOneReservation(SelectedReservation);
            ChangeReservation changeReservation = new ChangeReservation(navigationService, dates, SelectedReservation);
            navigationService.Navigate(changeReservation);
           
        }
        private bool CanChangeReservation()
        {
            return SelectedReservation != null;
        }
        private void OnCancelReservation(AccommodationReservationDTO SelectedReservation)
        {
            var accommodationDTO = accommodationService.GetAccommodation(SelectedReservation.AccommodationId);
            if (cancelledReservationsService.IsCancellationPeriodValid(accommodationDTO.ToAccommodation(), SelectedReservation.ToAccommodationReservation()))
            {
                cancelledReservationsService.CancelReservation(accommodationService.GetById(accommodationDTO.Id), SelectedReservation.ToAccommodationReservation());
                MessageBox.Show("Your reservation has been successfully cancelled!");
                Update();
            }
            else
                MessageBox.Show("The deadline for canceling the reservation has passed.");
        }
    }
}
