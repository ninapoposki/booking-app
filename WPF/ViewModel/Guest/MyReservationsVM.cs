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

        public ObservableCollection<AccommodationReservationDTO> AllReservations { get; }

        public AccommodationDTO accommodationDTO { get; set; }

        private AccommodationReservationDTO _selectedReservation;

        public AccommodationReservationDTO SelectedReservation
        {
            get => _selectedReservation;
            set
            {
                if (_selectedReservation != value)
                {
                    _selectedReservation = value;
                    OnPropertyChanged(nameof(SelectedReservation));
                }
            }
        }

        public MyReservationsVM()
        {
            accommodationReservationService = new AccommodationReservationService();
            accommodationGradeService = new AccommodationGradeService();
            cancelledReservationsService = new CancelledReservationsService();
            accommodationService = new AccommodationService();
            AllReservations = new ObservableCollection<AccommodationReservationDTO>();
            accommodationDTO=new AccommodationDTO();
            Update();
        }

        public void Update()
        {
           
            AllReservations.Clear();
            foreach (AccommodationReservationDTO accommodationReservationDTO in accommodationReservationService.GetAll())
            {
                var reservationDTO = accommodationReservationService.GetOneReservation(accommodationReservationDTO);
                AllReservations.Add(reservationDTO);
            }
        }
        public void AccommodationDataGrid(AccommodationReservationDTO selectedReservation)
        {
            int reservationId = accommodationGradeService.GetReservationId(selectedReservation.ToAccommodationReservation());
            if (accommodationGradeService.IsReservationGraded(reservationId))
            {
                MessageBox.Show("Accommodation is already graded.");
            }
            else
            {
                AreDatesValid(selectedReservation);
            }
        }
        private void AreDatesValid(AccommodationReservationDTO accommodationReservationDTO)
        {
            if (accommodationReservationDTO.EndDate > DateTime.Now)
            {
                MessageBox.Show("Your days to stay has not finished yet!");
            }
            else
            {
                if (accommodationReservationService.IsOverFiveDays(accommodationReservationDTO))
                {
                    var dialog = new GradeAccommodation(accommodationReservationDTO);
                    dialog.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Grading is not possible, it has been more than 5 days.");
                }
            }
        }
        public void RateAccommodationClick()
        {
            if (SelectedReservation != null)
            {
                AccommodationDataGrid(SelectedReservation);
            }
            else
            {
                MessageBox.Show("You didn't choose reservation!");
            }
        }

        public void CancelReservationClick()
        {
            //dodaj -kad se ne selektuje nikakva rezervacija
            accommodationDTO = accommodationService.GetAccommodation(SelectedReservation.AccommodationId);
            if(cancelledReservationsService.IsCancellationPeriodValid(accommodationDTO.ToAccommodation(), SelectedReservation.ToAccommodationReservation()))
            {
                cancelledReservationsService.CancelReservation(accommodationService.GetById(accommodationDTO.Id), SelectedReservation.ToAccommodationReservation());
                MessageBox.Show("Your reservation has been successfully cancelled!");
                Update();
            }
            else
            {
                if (cancelledReservationsService.IsReservationPassed(SelectedReservation.InitialDate))
                {
                    MessageBox.Show("You can't cancel reservation now!");
                }
                else
                {
                    MessageBox.Show("The deadline for canceling the reservation has passed.");

                }
            }
        }
    }
}
