using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using BookingApp.WPF.View.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class MyToursUserControlVM:ViewModelBase
    {
        private int selectedTabIndex;
        public int SelectedTabIndex
        {
            get { return selectedTabIndex; }
            set
            {
                if (selectedTabIndex != value)
                {
                    selectedTabIndex = value;
                    OnPropertyChanged("SelectedTabIndex");
                }
            }
        }
        public MyICommand TourStatisticsCommand { get; }
        public MyICommand<TourDTO> CancelTourCommand { get; }
        public MyICommand<TourDTO> StartTourCommand { get; }
        public MyICommand<TourDTO> SeeReviewsCommand { get; }
        public NavigationService  NavigationService { get; set; }
        private int userId;
        public ObservableCollection<TourDTO> TodaysTours { get; set; }
        public ObservableCollection<TourDTO> FinishedTours { get; set; }
        public ObservableCollection<TourDTO> UpcomingTours { get; set; }
        private TourStartDateService tourStartDateService;
        private TourReservationService tourReservationService;
        private ImageService imageService;
        private TourService tourService;
        private VoucherService voucherService;
        public MyToursUserControlVM(NavigationService navigationService,int userId) 
        {
            TourStatisticsCommand = new MyICommand(OnTourStatistics);
            CancelTourCommand = new MyICommand<TourDTO>(OnCancelTour, CanCancelTour);
            StartTourCommand = new MyICommand<TourDTO>(OnStartTour,CanTourStart);
            SeeReviewsCommand = new MyICommand<TourDTO>(OnSeeReviews);
            this.userId = userId;
            this.NavigationService = navigationService;
            FinishedTours = new ObservableCollection<TourDTO>();
            TodaysTours = new ObservableCollection<TourDTO>();
            UpcomingTours= new ObservableCollection<TourDTO>();
            tourStartDateService = new TourStartDateService(Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            tourReservationService = new TourReservationService(Injector.Injector.CreateInstance<ITourReservationRepository>(), Injector.Injector.CreateInstance<ITourGuestRepository>(),Injector.Injector.CreateInstance<IUserRepository>(), Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(),Injector.Injector.CreateInstance<ILanguageRepository>(),Injector.Injector.CreateInstance<ILocationRepository>());
            tourService = new TourService(Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            voucherService = new VoucherService(Injector.Injector.CreateInstance<IVoucherRepository>(), Injector.Injector.CreateInstance<ITourReservationRepository>(), Injector.Injector.CreateInstance<ITourGuestRepository>(),Injector.Injector.CreateInstance<IUserRepository>(), Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(),Injector.Injector.CreateInstance<ILanguageRepository>(),Injector.Injector.CreateInstance<ILocationRepository>());
            LoadFinishedTours();
            LoadTodaysTours();
            LoadUpcomingTours();          
        }

        private bool CanCancelTour(TourDTO tour)
        {
            if (tour != null)
            {
                TimeSpan timeDifference = tour.SelectedDateTime.StartDateTime - DateTime.Now;
                return timeDifference.TotalHours > 48;
            }
            return true;
        }
        private void OnCancelTour(TourDTO tour)
        {
            if (!IsVaucherGranted(tour.SelectedDateTime)) { MessageBox.Show("No reservation for this tour, no vauchers granted"); }

            tourStartDateService.UpdateTourStatus(tour.SelectedDateTime.Id);
            tour.DateTimes.Remove(tour.SelectedDateTime);
            if (tour.DateTimes == null) { UpcomingTours.Remove(tour); }
        }
        private bool IsVaucherGranted(TourStartDateDTO tourStart)
        {
            return voucherService.GrantVoucher(tourStart, "Tour has been cancelled, you have granted a voucher!");
        }
        private void OnTourStatistics()
        {
            NavigationService.Navigate(new TourStatisticsUserControl(userId));
        }
        private void OnSeeReviews(TourDTO tourDTO)
        {
            NavigationService.Navigate(new TourReviewsUserControl(tourDTO));
        }
        private bool CanTourStart(TourDTO tourDTO)
        {
            if (tourStartDateService.GetActiveTour() == null) { return true; }
            else { return false;}
        }
        private void OnStartTour(TourDTO tourDTO)
        {
            NavigationService.Navigate(new StartTourUserControl(NavigationService, tourDTO.SelectedDateTime, userId));
        }
        private void LoadTours(Func<TourStartDateDTO, bool> filter, ObservableCollection<TourDTO> collection, Action<TourDTO, TourStartDateDTO> addAction)
        {
            foreach (TourDTO tour in tourService.GetAllForUser(userId))
            {
                List<TourStartDateDTO> tourDates = GetFilteredTourDates(tour.Id, filter);
                foreach (TourStartDateDTO tourStartDate in tourDates) { TourDTO tourDTO = tourService.GetTour(tourStartDate.TourId); addAction(tourDTO, tourStartDate); }
            }
        }
        private void AddTourToToday(TourDTO tourDTO, TourStartDateDTO tourStartDate)
        {
            tourDTO.SelectedDateTime = tourStartDate;
            SetImage(tourDTO);
            tourDTO.NumberOfTourists = tourReservationService.GetByStartDate(tourStartDate.Id).Count();
            TodaysTours.Add(tourDTO);
        }
        private void AddTourToFinished(TourDTO tourDTO, TourStartDateDTO tourStartDate)
        {
            tourDTO.SelectedDateTime = tourStartDate;
            SetImage(tourDTO);
            tourDTO.NumberOfTourists = tourReservationService.GetFinishedToursGuests(tourStartDate.Id).Count();
            FinishedTours.Add(tourDTO);
        }
        private List<TourStartDateDTO> GetFilteredTourDates(int tourId, Func<TourStartDateDTO, bool> filter)
        {
            return tourStartDateService.GetTourDates(tourId).Where(filter).ToList();
        }
        private void LoadTodaysTours()
        {
            LoadTours(ts => IsTourToday(ts), TodaysTours,AddTourToToday);
        }
        private bool IsTourToday(TourStartDateDTO tourStart)
        {
            return tourStart.StartDateTime.Date == DateTime.Today && tourStart.TourStatus.ToString().Equals("INACTIVE");
        }
        private void LoadFinishedTours()
        {
            LoadTours(ts => IsTourFinished(ts), FinishedTours, AddTourToFinished);
        }
        private bool IsTourFinished(TourStartDateDTO tourStartDate)
        {
            return tourStartDate.TourStatus == TourStatus.FINISHED;
        }
        private void LoadUpcomingTours()
         {
             foreach (TourDTO tour in tourService.GetAllForUser(userId))
             {
                 List<TourStartDateDTO> filteredDates = GetUpcomingTourDates(tour.Id);
                 if (filteredDates.Count > 0)
                 {
                     tour.DateTimes = new ObservableCollection<TourStartDateDTO>(filteredDates);
                     SetImage(tour);
                     UpcomingTours.Add(tour);
                 }
             }
        }    
        private bool AreToursUpcoming(TourStartDateDTO tourStartDate)
        {
            return tourStartDate.StartDateTime >= DateTime.Today && tourStartDate.TourStatus.ToString().Equals("INACTIVE");
        }
        private void SetImage(TourDTO tourDTO)
        {
            if (imageService.GetFirstPath(tourDTO.Id, "TOUR") != null) { tourDTO.Path = imageService.GetFirstPath(tourDTO.Id, "TOUR"); }
            else { tourDTO.Path = "..\\..\\..\\Resources\\Images\\placeholderGuide.png"; }
        }
        private List<TourStartDateDTO> GetUpcomingTourDates(int tourId)
        {
            IEnumerable<TourStartDateDTO> allTourStartDates = tourStartDateService.GetTourDates(tourId);
            List<TourStartDateDTO> filteredTourDates = allTourStartDates.Where(tourStart => AreToursUpcoming(tourStart)).ToList();
            return filteredTourDates;
        }
    }
}
