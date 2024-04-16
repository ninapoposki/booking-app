using BookingApp.Domain.IRepositories;
using BookingApp.DTO;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class ChangeReservationVM:ViewModelBase
    {
        private ReservationRequestService reservationRequestService;
        public ReservationRequestDTO reservationRequestDTO { get; set; }

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
        private ObservableCollection<Range> dates;
        public ObservableCollection<Range> Dates
        {
            get { return dates; }
            set
            {
                dates = value;
                OnPropertyChanged();
            }
        }

        private Range _selectedDate;
        public Range selectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }

        public ChangeReservationVM(List<(DateTime, DateTime)> dates, AccommodationReservationDTO accommodationReservationDTO)
        {
            SelectedReservation = accommodationReservationDTO; 
            reservationRequestService = new ReservationRequestService(Injector.Injector.CreateInstance<IReservationRequestRepository>(),
                Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
                Injector.Injector.CreateInstance<IGuestRepository>(),
                Injector.Injector.CreateInstance<IUserRepository>(),
                Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>());
            reservationRequestDTO=new ReservationRequestDTO();
            Dates = new ObservableCollection<Range>(dates.Select(r => new Range { NewInitialDate = r.Item1, NewEndDate = r.Item2 }).ToList());

        }

        public void SendRequestClick()
        {
            if (selectedDate != null)
            {
                reservationRequestService.SetNewDates(selectedDate.NewInitialDate, selectedDate.NewEndDate, SelectedReservation.Id);

                MessageBox.Show("You successfully added new request!");

            }
            else
            {
                MessageBox.Show("Please select a date before sending request.");
            }
        }
        public class Range
        {
            public DateTime NewInitialDate { get; set; }
            public DateTime NewEndDate { get; set; }
        }

    }
}
