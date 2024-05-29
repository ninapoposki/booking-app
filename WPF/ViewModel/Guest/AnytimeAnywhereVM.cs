using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using BookingApp.WPF.View.Guest;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class AnytimeAnywhereVM : ViewModelBase
    {
        private readonly AccommodationReservationService accommodationReservationService;
        private readonly AccommodationService accommodationService;
        private readonly ImageService imageService;
        private readonly LocationService locationService;
        public GuestDTO guestDTO { get; set; }
        public NavigationService NavigationService { get; set; }

        private DateTime? _initialDate;
        public DateTime? InitialDate
        {
            get => _initialDate;
            set
            {
                _initialDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }

        private int _daysToStay;
        public int DaysToStay
        {
            get => _daysToStay;
            set
            {
                _daysToStay = value;
                OnPropertyChanged();
            }
        }

        private int _numberOfGuests;
        public int NumberOfGuests
        {
            get => _numberOfGuests;
            set
            {
                _numberOfGuests = value;
                OnPropertyChanged();
            }
        }

        private AccommodationDTO _selectedAccommodation;
        public AccommodationDTO SelectedAccommodation
        {
            get => _selectedAccommodation;
            set
            {
                _selectedAccommodation = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<AccommodationDTO> AvailableAccommodations { get; set; }

        public MyICommand SearchCommand { get; }
        public MyICommand<AccommodationDTO> ReserveCommand { get; }
        public MyICommand<AccommodationDTO> MoreDatesCommand { get; }

        public AnytimeAnywhereVM(NavigationService navigationService, GuestDTO guestDTO)
        {
            accommodationReservationService = new AccommodationReservationService(Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
                                       Injector.Injector.CreateInstance<IGuestRepository>(),
                                       Injector.Injector.CreateInstance<IUserRepository>(),
                                       Injector.Injector.CreateInstance<IAccommodationRepository>(),
                                       Injector.Injector.CreateInstance<IImageRepository>(),
                                       Injector.Injector.CreateInstance<ILocationRepository>(),
                                       Injector.Injector.CreateInstance<IOwnerRepository>());
            accommodationService = new AccommodationService(Injector.Injector.CreateInstance<IAccommodationRepository>(),
               Injector.Injector.CreateInstance<IImageRepository>(),
               Injector.Injector.CreateInstance<ILocationRepository>(),
               Injector.Injector.CreateInstance<IOwnerRepository>());
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            locationService = new LocationService(Injector.Injector.CreateInstance<ILocationRepository>());
            NavigationService = navigationService;
            this.guestDTO = guestDTO;
            SelectedAccommodation = new AccommodationDTO();
            AvailableAccommodations = new ObservableCollection<AccommodationDTO>();
            SearchCommand = new MyICommand(OnSearch);
            ReserveCommand = new MyICommand<AccommodationDTO>(OnReserve);
            MoreDatesCommand = new MyICommand<AccommodationDTO>(OnMoreDates);
        }

        private void OnSearch()
        {
            var accommodations = accommodationService.GetAll();
            AvailableAccommodations.Clear();

            foreach (var accommodation in accommodations)
            {
                var allImages = imageService.GetImagesForEntityType(EntityType.ACCOMMODATION);
                var matchingImages = new ObservableCollection<ImageDTO>(imageService.GetImagesByAccommodation(accommodation.Id, allImages));
                if (matchingImages.Count == 0)
                {
                    matchingImages.Add(new ImageDTO { Path = @"\Resources\Icons\Owner\accommodation_placeholder.jpg" });
                }

                if (accommodation.Capacity >= NumberOfGuests && accommodation.MinStayDays <= DaysToStay)
                {
                    if (InitialDate.HasValue && EndDate.HasValue)
                    {
                        if (accommodationReservationService.AreDatesAvailable(accommodation.Id, InitialDate.Value, EndDate.Value))
                        {
                            var dates = accommodationReservationService.FindDateRange(
                                new AccommodationReservation
                                {
                                    InitialDate = this.InitialDate.Value,
                                    EndDate = this.EndDate.Value,
                                    DaysToStay = DaysToStay,
                                    NumberOfGuests = NumberOfGuests,
                                    AccommodationId = accommodation.Id
                                },
                                accommodation.Id);

                            if (dates.Any())
                            {
                                var firstDate = dates.First();
                                //accommodation.FirstAvailableDates = new Tuple<DateTime, DateTime>(firstDate.Item1, firstDate.Item2);

                                LocationDTO location = locationService.GetByIdDTO(accommodation.IdLocation);
                                AvailableAccommodations.Add(new AccommodationDTO(accommodation.ToAccommodation(), location.ToLocation())
                                {
                                    Images = matchingImages,
                                    FirstAvailableDates = new Tuple<DateTime, DateTime>(firstDate.Item1, firstDate.Item2),
                                    AllAvailableDates = dates.Select(d => (d.Item1, d.Item2)).ToList() // Koristite ValueTuple ovde
                                });
                            }
                        }
                    }
                    else
                    {
                        var alternativeDates = accommodationReservationService.FindAlternativeDates(
                            new AccommodationReservation
                            {
                                InitialDate = DateTime.Now,
                                EndDate = DateTime.Now.AddDays(DaysToStay),
                                DaysToStay = DaysToStay,
                                NumberOfGuests = NumberOfGuests,
                                AccommodationId = accommodation.Id
                            },
                            accommodation.Id);

                        if (alternativeDates.Any())
                        {
                            var firstDate = alternativeDates.First();
                            accommodation.FirstAvailableDates = new Tuple<DateTime, DateTime>(firstDate.Item1, firstDate.Item2);
                            LocationDTO location = locationService.GetByIdDTO(accommodation.IdLocation);
                            AvailableAccommodations.Add(new AccommodationDTO(accommodation.ToAccommodation(), location.ToLocation())
                            {
                                Images = matchingImages,
                                FirstAvailableDates = new Tuple<DateTime, DateTime>(firstDate.Item1, firstDate.Item2),
                                AllAvailableDates = alternativeDates.Select(d => (d.Item1, d.Item2)).ToList() // Koristite ValueTuple ovde
                            });
                        }
                    }
                }
            }
        }

        private void OnReserve(AccommodationDTO accommodation)
        {
            if (accommodation.FirstAvailableDates == null)
            {
                MessageBox.Show("No available dates.");
                return;
            }

            var reservationDTO = new AccommodationReservationDTO
            {
                AccommodationId = accommodation.Id,
                InitialDate = accommodation.FirstAvailableDates.Item1,
                EndDate = accommodation.FirstAvailableDates.Item2,
                NumberOfGuests = NumberOfGuests,
                DaysToStay = DaysToStay
            };

            reservationDTO = new AccommodationReservationDTO(accommodationReservationService.ProcessDateRange(reservationDTO.ToAccommodationReservation(), accommodation.Id, guestDTO.ToGuest()));
            accommodationReservationService.GetOneReservation(reservationDTO);
            accommodationReservationService.Add(reservationDTO.ToAccommodationReservation());
            MessageBox.Show("Reservation added successfully.");
        }

        private void OnMoreDates(AccommodationDTO accommodation)
        {
            var reservationDTO = new AccommodationReservationDTO
            {
                AccommodationId = accommodation.Id,
                NumberOfGuests = NumberOfGuests,
                DaysToStay = DaysToStay,
                GuestId = guestDTO.Id,
                Accommodation = accommodation,
                Guest = guestDTO
            };
            var availableDatesWindow = new ReccommendedDates(NavigationService, reservationDTO);
            NavigationService.Navigate(availableDatesWindow);
        }
    }
}
