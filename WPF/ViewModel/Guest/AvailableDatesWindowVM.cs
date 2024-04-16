using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using BookingApp.Domain.IRepositories;
using BookingApp.DTO;
using BookingApp.Services;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class AvailableDatesWindowVM : ViewModelBase
    {
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

        private AccommodationReservationDTO _selectedReservation;
        public AccommodationReservationDTO SelectedReservation
        {
            get { return _selectedReservation; }
            set
            {
                _selectedReservation = value;
                OnPropertyChanged();
            }
        }

        private readonly AccommodationReservationService accommodationReservationService;
        public event EventHandler RequestClose;


        public AvailableDatesWindowVM(List<(DateTime, DateTime)> dates, AccommodationReservationDTO accommodationReservation)
        {
            accommodationReservationService = new AccommodationReservationService(Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
               Injector.Injector.CreateInstance<IGuestRepository>(),
               Injector.Injector.CreateInstance<IUserRepository>(),
               Injector.Injector.CreateInstance<IAccommodationRepository>(),
               Injector.Injector.CreateInstance<IImageRepository>(),
               Injector.Injector.CreateInstance<ILocationRepository>(),
               Injector.Injector.CreateInstance<IOwnerRepository>());
            Dates = new ObservableCollection<Range>(dates.Select(r => new Range { InitialDate = r.Item1, EndDate = r.Item2 }).ToList());
            SelectedReservation = accommodationReservation;
        }

        public void BookAccommodationClick()
        {
            if (selectedDate != null)
            {
                SelectedReservation.InitialDate = selectedDate.InitialDate; 
                SelectedReservation.EndDate = selectedDate.EndDate;

                accommodationReservationService.Add(SelectedReservation.ToAccommodationReservation()); 
                
                MessageBox.Show("Reservation added successfully");
                RequestClose?.Invoke(this, EventArgs.Empty);

            }
            else
            {
                MessageBox.Show("Please select a date before booking.");
            }
        }


        public class Range
        {
            public DateTime InitialDate { get; set; }
            public DateTime EndDate { get; set; }
        }
    }
}
