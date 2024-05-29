using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using BookingApp.Domain.IRepositories;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class ReccommendedDatesVM : ViewModelBase
    {
        private readonly AccommodationReservationService accommodationReservationService;
        private readonly GuestService guestService;
        public NavigationService NavigationService { get; set; }

        public ObservableCollection<Range> Dates { get; set; }

        private Range _selectedDate;
        public Range SelectedDate
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
        public int currentIndex = 0;
        public MyICommand PreviousPicture { get; private set; }
        public MyICommand NextPicture { get; private set; }
        public AccommodationDTO SelectedAccommodationDTO { get; set; }
        public MyICommand <Range>BookCommand { get; set; }

        public ReccommendedDatesVM(NavigationService navigationService, AccommodationReservationDTO resrvationDTO)
        {
            accommodationReservationService = new AccommodationReservationService(
                Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
                Injector.Injector.CreateInstance<IGuestRepository>(),
                Injector.Injector.CreateInstance<IUserRepository>(),
                Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>());

            guestService = new GuestService(Injector.Injector.CreateInstance<IGuestRepository>());

            Dates = new ObservableCollection<Range>(resrvationDTO.Accommodation.AllAvailableDates.Select(r => new Range { InitialDate = r.Item1, EndDate = r.Item2 }).ToList());
          /* SelectedReservation = new AccommodationReservationDTO
            {
                AccommodationId = accommodation.Id,
                Accommodation = accommodation,
                NumberOfGuests = numberOfGuests,
                DaysToStay = daysToStay,
                Guest = new GuestDTO { Id = accommodation.Id } // Update as per your GuestDTO initialization
            };*/
            //SelectedAccommodationDTO = accommodation;
            SelectedReservation=resrvationDTO;

            NavigationService = navigationService;
            BookCommand = new MyICommand<Range>(OnBookAccommodation);
        }

        private void OnBookAccommodation(Range selectedDate)
        {
            SelectedReservation.InitialDate = selectedDate.InitialDate;
            SelectedReservation.EndDate = selectedDate.EndDate;

            accommodationReservationService.Add(SelectedReservation.ToAccommodationReservation());
            if (SelectedReservation.Guest.Role == "SUPERGUEST" && SelectedReservation.Guest.Points > 0)
            {
                SelectedReservation.Guest.Points -= 1;
            }
            guestService.Update(SelectedReservation.Guest.ToGuest());
            MessageBox.Show("Reservation added successfully");
            NavigationService.GoBack();
        }

        public class Range
        {
            public DateTime InitialDate { get; set; }
            public DateTime EndDate { get; set; }
        }
    }
}
