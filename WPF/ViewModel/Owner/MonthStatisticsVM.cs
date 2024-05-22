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
using System.Globalization;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class MonthStatisticsVM {
        public NavigationService navigationService;
        public CancelledReservationsService cancelledReservationsService { get; set; }
        public AccommodationReservationService accommodationReservationService { get; set; }
        public AccommodationService accommodationService { get; set; }
        public ReservationRequestService reservationRequestService { get; set; }
        public RenovationRecommendationService recommendationService { get; set; }
        public string AccommodationName {  get; set; }
        public string BestMonth {  get; set; }
        public ObservableCollection<AccommodationStatisticsDTO> Months { get; set; }
        public AccommodationStatisticsDTO AccommodationStatisticsDTO {  get; set; }
        public MyICommand GoBackCommand { get; private set; }
        public MonthStatisticsVM(NavigationService navigation, AccommodationStatisticsDTO accommodationStatisticsDTO) {
            navigationService = navigation;
            AccommodationStatisticsDTO = accommodationStatisticsDTO;
            accommodationService = new AccommodationService(Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>());
            cancelledReservationsService = new CancelledReservationsService(Injector.Injector.CreateInstance<ICancelledReservationsRepository>(),
               Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),Injector.Injector.CreateInstance<IGuestRepository>(),
               Injector.Injector.CreateInstance<IUserRepository>(), Injector.Injector.CreateInstance<IAccommodationRepository>(),
               Injector.Injector.CreateInstance<IImageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>(),
               Injector.Injector.CreateInstance<IOwnerRepository>(), Injector.Injector.CreateInstance<IReservationRequestRepository>());
            accommodationReservationService = new AccommodationReservationService(Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
                          Injector.Injector.CreateInstance<IGuestRepository>(), Injector.Injector.CreateInstance<IUserRepository>(),
                          Injector.Injector.CreateInstance<IAccommodationRepository>(),Injector.Injector.CreateInstance<IImageRepository>(),
                          Injector.Injector.CreateInstance<ILocationRepository>(),Injector.Injector.CreateInstance<IOwnerRepository>());
            reservationRequestService = new ReservationRequestService(Injector.Injector.CreateInstance<IReservationRequestRepository>(),
               Injector.Injector.CreateInstance<IAccommodationReservationRepository>(), Injector.Injector.CreateInstance<IGuestRepository>(),
               Injector.Injector.CreateInstance<IUserRepository>(),  Injector.Injector.CreateInstance<IAccommodationRepository>(),
               Injector.Injector.CreateInstance<IImageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>(),
               Injector.Injector.CreateInstance<IOwnerRepository>());
            recommendationService = new RenovationRecommendationService(Injector.Injector.CreateInstance<IRenovationRecommendationRepository>(),
                Injector.Injector.CreateInstance<IAccommodationRepository>(), Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>(), Injector.Injector.CreateInstance<IOwnerRepository>());
            AccommodationName = accommodationService.GetById(accommodationStatisticsDTO.AccomId).Name;
           Months = new ObservableCollection<AccommodationStatisticsDTO>();
            GoBackCommand = new MyICommand(GoBack);
            Update();
            BestMonth = GetBestMonth(accommodationStatisticsDTO.AccomId, Months);
        }
        public string GetBestMonth(int accommodationId, ObservableCollection<AccommodationStatisticsDTO> Months){
            string BestMonth = "January";
            int max_value = 0;
            foreach (var item in Months)  {
                item.Year = AccommodationStatisticsDTO.Year;
                DateTimeFormatInfo dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
                string[] monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                int monthNumber = Array.IndexOf(monthNames, item.MonthName) + 1;
               AccommodationDTO accommodationDTO = accommodationService.GetByIdDTO(AccommodationStatisticsDTO.AccomId);
                int made = CalculateOccupancy(accommodationDTO, monthNumber, item.Year);
                if (made > max_value) {
                    max_value = made;
                    BestMonth = item.MonthName;  
                }
                if(max_value == 0) {BestMonth = " "; }
            }
            return BestMonth;
        }
        public void Update(){
            string[] monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            for (int month = 1; month <= 12; month++){
                var statistics = MakeStatistics(month,AccommodationStatisticsDTO.AccomId, AccommodationStatisticsDTO.Year);
                statistics.MonthName = monthNames[month - 1];
                Months.Add(statistics);
            }
        }
        public AccommodationStatisticsDTO MakeStatistics(int month, int accommodationId, int year) {
            List<CancelledReservationsDTO> cancellationRes = cancelledReservationsService.GetAll();
            int cancelled = cancellationRes.Count(reservation => reservation.InitialDate.Month == month && reservation.AccommodationId == accommodationId && reservation.InitialDate.Year == year);
            int made = accommodationReservationService.GetAll().Count(reservation => reservation.InitialDate.Month == month && reservation.AccommodationId == accommodationId && reservation.InitialDate.Year == year);
            int postponed = GetPostponedNumber(accommodationId, month, year);
            int recommended = GetRecommendedNumber(accommodationId, month, year);
            return new AccommodationStatisticsDTO() { CancelledReservations = cancelled, MadeReservations = made, PostponedReservations = postponed, RecommendedReservations = recommended, AccomId = accommodationId };
        }
        public int GetRecommendedNumber(int accommodationId, int month, int year) {
            int recommended = 0;
            List<RenovationRecommendationDTO> Renrecommended = recommendationService.GetAll();
            foreach (var recommendation in Renrecommended){
                int id = accommodationReservationService.GetById(recommendation.ReservationId).AccommodationId;
                AccommodationDTO accommodationDTO = accommodationService.GetByIdDTO(id);
                AccommodationReservationDTO accommodationReservationDTO = accommodationReservationService.GetByIdDTO(recommendation.ReservationId);
                if (accommodationId == id && accommodationReservationDTO.InitialDate.Month == month && accommodationReservationDTO.InitialDate.Year == year) {
                    recommended++;}
            }
            return recommended;
        }
        public int GetPostponedNumber(int accommodationId, int month, int year ) {
            int postponed = 0;
            List<ReservationRequestDTO> requested = reservationRequestService.GetAll();
            foreach (var req in requested) {
                int id = accommodationReservationService.GetById(req.ReservationId).AccommodationId;
                AccommodationDTO accommodationDTO = accommodationService.GetByIdDTO(id);
                AccommodationReservationDTO accommodationReservationDTO = accommodationReservationService.GetByIdDTO(req.ReservationId);
                if (accommodationId == id && req.RequestStatus == Domain.Model.RequestStatus.ACCEPTED && req.NewInitialDate.Month == month && req.NewInitialDate.Year == year) {
                    postponed++; }
            }return postponed;
        }
        private void GoBack(){
            if (navigationService.CanGoBack){  navigationService.GoBack(); } }
        public int CalculateOccupancy(AccommodationDTO accommodationDTO, int month, int year)  {
            int countedDays = 0;
            int totalDays = DateTime.DaysInMonth(2024, month);
            int overDays = 0;
            var Reservations = accommodationReservationService.GetAll();
            foreach (var reservation in Reservations){
                if (reservation.AccommodationId == accommodationDTO.Id &&  reservation.InitialDate.Month == month && reservation.InitialDate.Year == year)  {
                    if (reservation.InitialDate.Month == reservation.EndDate.Month ) {
                        for (DateTime date = reservation.InitialDate; date <= reservation.EndDate; date = date.AddDays(1)){
                            countedDays++;
                        }  
                    } else {
                        DateTime endDateToConsider = new DateTime(2024, month, totalDays);
                        DateTime endDate = reservation.EndDate;
                            overDays += (reservation.EndDate-endDateToConsider  ).Days;
                            for (DateTime date = reservation.InitialDate; date.Month == month; date = date.AddDays(1)) {
                                countedDays++;
                            }
                    } } }
            countedDays += OverDays;
            int Occupancy = (int)(100 * (double)countedDays / (double)totalDays);
            OverDays = overDays;
            return Occupancy;
        }
        public int OverDays = 0;
    }
}