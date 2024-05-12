using BookingApp.Domain.IRepositories;
using BookingApp.DTO;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Navigation;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.ObjectModel;
using BookingApp.Utilities;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class MonthStatisticsVM
    {
        public NavigationService navigationService;
        public CancelledReservationsService cancelledReservationsService { get; set; }
        public AccommodationReservationService accommodationReservationService { get; set; }
        public AccommodationService accommodationService { get; set; }
        public ReservationRequestService reservationRequestService { get; set; }
        public string AccommodationName {  get; set; }
        public string BestMonth {  get; set; }
        public ObservableCollection<AccommodationStatisticsDTO> Months { get; set; }
        public AccommodationStatisticsDTO AccommodationStatisticsDTO {  get; set; }
        public MyICommand GoBackCommand { get; private set; }
        public MonthStatisticsVM(NavigationService navigation, AccommodationStatisticsDTO accommodationStatisticsDTO)
        {
            navigationService = navigation;
            AccommodationStatisticsDTO = accommodationStatisticsDTO;
            accommodationService = new AccommodationService(Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>());
            cancelledReservationsService = new CancelledReservationsService(Injector.Injector.CreateInstance<ICancelledReservationsRepository>(),
               Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
               Injector.Injector.CreateInstance<IGuestRepository>(),
               Injector.Injector.CreateInstance<IUserRepository>(),
               Injector.Injector.CreateInstance<IAccommodationRepository>(),
               Injector.Injector.CreateInstance<IImageRepository>(),
               Injector.Injector.CreateInstance<ILocationRepository>(),
               Injector.Injector.CreateInstance<IOwnerRepository>(),
               Injector.Injector.CreateInstance<IReservationRequestRepository>());
            accommodationReservationService = new AccommodationReservationService(Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
                          Injector.Injector.CreateInstance<IGuestRepository>(),
                          Injector.Injector.CreateInstance<IUserRepository>(),
                          Injector.Injector.CreateInstance<IAccommodationRepository>(),
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
            AccommodationName = accommodationService.GetById(accommodationStatisticsDTO.AccomId).Name;
            BestMonth = "January";
            Months = new ObservableCollection<AccommodationStatisticsDTO>();
            GoBackCommand = new MyICommand(GoBack);
            Update();
        }
        public void Update()
        {
            string[] monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            for (int month = 1; month <= 12; month++)
            {
                var statistics = MakeStatistics(month,  0, AccommodationStatisticsDTO.AccomId);
                statistics.MonthName = monthNames[month - 1];
                Months.Add(statistics);
            }
        }
        public AccommodationStatisticsDTO MakeStatistics(int month, int recommended, int accommodationId)
        {
            
            List<CancelledReservationsDTO> cancellationRes = cancelledReservationsService.GetAll();
            
            int cancelled = cancellationRes.Count(reservation => reservation.InitialDate.Month == month && reservation.AccommodationId == accommodationId);
            int made = accommodationReservationService.GetAll().Count(reservation => reservation.InitialDate.Month == month && reservation.AccommodationId == accommodationId);
            int postponed = GetPostponedNumber(accommodationId, month);


            return new AccommodationStatisticsDTO() { CancelledReservations = cancelled, MadeReservations = made, PostponedReservations = postponed, RecommendedReservations = recommended, AccomId = accommodationId };
        }
        public int GetPostponedNumber(int accommodationId, int month)
        {
            int postponed = 0;
            List<ReservationRequestDTO> requested = reservationRequestService.GetAll();
            foreach (var req in requested)
            {
                int id = accommodationReservationService.GetById(req.ReservationId).AccommodationId;
                AccommodationDTO accommodationDTO = accommodationService.GetByIdDTO(id);
                AccommodationReservationDTO accommodationReservationDTO = accommodationReservationService.GetByIdDTO(req.ReservationId);
                if (accommodationId == id && req.RequestStatus == Domain.Model.RequestStatus.ACCEPTED && req.NewInitialDate.Month == month)
                {
                    postponed++;
                }
            }

            return postponed;

        }
        private void GoBack()
        {
            if (navigationService.CanGoBack)
            {
                navigationService.GoBack();
            }
        }

    }
}