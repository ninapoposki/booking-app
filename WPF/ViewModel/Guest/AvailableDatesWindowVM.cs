using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using BookingApp.Domain.IRepositories;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;


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
                BookCommand.RaiseCanExecuteChanged();

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
        private readonly GuestService guestService;
        public NavigationService navigationService { get; set; }
        public event EventHandler RequestClose;
        public MyICommand <Range> BookCommand { get; set; }
        public AvailableDatesWindowVM(NavigationService navigationService, List<(DateTime, DateTime)> dates, AccommodationReservationDTO accommodationReservation)
        {
            accommodationReservationService = new AccommodationReservationService(Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
               Injector.Injector.CreateInstance<IGuestRepository>(),
               Injector.Injector.CreateInstance<IUserRepository>(),
               Injector.Injector.CreateInstance<IAccommodationRepository>(),
               Injector.Injector.CreateInstance<IImageRepository>(),
               Injector.Injector.CreateInstance<ILocationRepository>(),
               Injector.Injector.CreateInstance<IOwnerRepository>());
            guestService = new GuestService(Injector.Injector.CreateInstance<IGuestRepository>());
            Dates = new ObservableCollection<Range>(dates.Select(r => new Range { InitialDate = r.Item1, EndDate = r.Item2 }).ToList());
            SelectedReservation = accommodationReservation;
            this.navigationService = navigationService;
            BookCommand = new MyICommand <Range> (OnBookAccommodation);
        }
        public void OnBookAccommodation(Range selectedDate)
        {
            SelectedReservation.InitialDate = selectedDate.InitialDate;
            SelectedReservation.EndDate = selectedDate.EndDate;

            accommodationReservationService.Add(SelectedReservation.ToAccommodationReservation());
            if(SelectedReservation.Guest.Role=="SUPERGUEST" && SelectedReservation.Guest.Points > 0)
            {
                SelectedReservation.Guest.Points -= 1;
            }
            guestService.Update(SelectedReservation.Guest.ToGuest());
            MessageBox.Show("Reservation added successfully");
            navigationService.GoBack();
        }
        public class Range
        {
            public DateTime InitialDate { get; set; }
            public DateTime EndDate { get; set; }
        }
    }
}
