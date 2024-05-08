using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using BookingApp.WPF.View.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class GuideHomeUserControlVM:ViewModelBase
    {
        private ToursTodayDTO selectedTour;
        public ToursTodayDTO SelectedTour
        {
            get { return selectedTour; }
            set
            {
                if (selectedTour != value)
                {
                    selectedTour = value;
                    StartTourCommand.RaiseCanExecuteChanged();
                }
            }
        }
        private int userId;
        public BreadCrumbsVM BreadCrumbsVM { get; set; }
        public NavigationService NavigationService { get; set; }
        public ObservableCollection<ToursTodayDTO> Tours { get; set; }
        private TourService tourService;
        private TourStartDateService tourStartDateService;
        private ImageService imageService;
        private TourReservationService tourReservationService;
        public MyICommand StartTourCommand { get; set; }
        public MyICommand CreateTourCommand { get; set; }
        public GuideHomeUserControlVM(NavigationService navigationService, int userId, ObservableCollection<BreadcrumbItem> breadcrumbs)
        {
            BreadCrumbsVM=new BreadCrumbsVM(breadcrumbs);
            this.userId = userId;
            NavigationService = navigationService;
            Tours = new ObservableCollection<ToursTodayDTO>();
            tourService = new TourService(Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            tourStartDateService = new TourStartDateService(Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            tourReservationService = new TourReservationService(Injector.Injector.CreateInstance<ITourReservationRepository>(), Injector.Injector.CreateInstance<ITourGuestRepository>(),Injector.Injector.CreateInstance<IUserRepository>(), Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(),Injector.Injector.CreateInstance<ILanguageRepository>(),Injector.Injector.CreateInstance<ILocationRepository>());
            StartTourCommand = new MyICommand(OnStartTour, CanStartTour);
            CreateTourCommand = new MyICommand(OnCreateTour);
            LoadTodaysTours();
        }
        private void OnCreateTour()
        {
            NavigationService.Navigate(new CreateTourUserControl(NavigationService, userId, BreadCrumbsVM.Breadcrumbs));
            BreadCrumbsVM.AddBreadcrumb("Create tour", new MyICommand(() => OnCreateTour()));
        }
        private void OnStartTour()
        {
            if (!tourReservationService.DoReservationExists(SelectedTour.TourDateTime.Id)) { MessageBox.Show("Tour does not have reservations"); return; }
            NavigationService.Navigate(new StartTourUserControl(NavigationService, SelectedTour.TourDateTime, userId,BreadCrumbsVM.Breadcrumbs));
            BreadCrumbsVM.AddBreadcrumb("Start tour", new MyICommand(() => OnStartTour()));
        }
        private bool CanStartTour()
        {
            return SelectedTour != null;
        }
        private void LoadTodaysTours()
        {
            if (IsAnyTourActive()) return;
            foreach (TourDTO tour in tourService.GetAllForUser(userId))
            {
                List<TourStartDateDTO> tourDates = GetFilteredTourDates(tour.Id);
                foreach (TourStartDateDTO tourStartDate in tourDates)
                {
                    TourDTO tourDTO = tourService.GetTour(tourStartDate.TourId);
                    AddTour(tourDTO, tourStartDate);
                }
            }
        }
        private void AddTour(TourDTO tourDTO, TourStartDateDTO tourStartDate)
        {
            ToursTodayDTO toursTodayDTO = new ToursTodayDTO(tourDTO.ToTour(), tourDTO.Language);
            toursTodayDTO.TourDateTime = tourStartDate;
            SetTime(toursTodayDTO);
            SetImage(toursTodayDTO);
            Tours.Add(toursTodayDTO);
        }
        private void SetTime(ToursTodayDTO toursDTO)
        {
            TimeSpan time = DateTime.ParseExact(toursDTO.TourDateTime.StartTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) - DateTime.Now;
            if (time < TimeSpan.Zero) { toursDTO.TimeUntilStart = TimeSpan.Zero; }
            else { toursDTO.TimeUntilStart = time; }
        }
        private void SetImage(ToursTodayDTO toursTodayDTO)
        {
            var path = imageService.GetFirstPath(toursTodayDTO.Id, "TOUR");
            toursTodayDTO.Path = path ?? "..\\..\\..\\Resources\\Images\\placeholderGuide.png";
        }
        private List<TourStartDateDTO> GetFilteredTourDates(int tourId)
        {
            IEnumerable<TourStartDateDTO> allTourStartDates = tourStartDateService.GetTourDates(tourId);
            List<TourStartDateDTO> filteredTourDates = allTourStartDates.Where(tourStart => AreToursToday(tourStart)).ToList();
            return filteredTourDates;
        }
        private bool AreToursToday(TourStartDateDTO tourStart)
        {
            return tourStart.StartDateTime.Date == DateTime.Today && tourStart.TourStatus.ToString().Equals("INACTIVE");
        }
        private bool IsAnyTourActive()
        {
            if (tourStartDateService.GetActiveTour() != null)
            {
                TourStartDateDTO tourStart=tourStartDateService.GetActiveTour();
                TourDTO tourDTO = tourService.GetTour(tourStart.TourId);
                tourDTO.SelectedDateTime = tourStart;
                AddTour(tourDTO, tourStart);
                AnyTourActive = true;
                return true;
            } AnyTourActive = false; return false;
        }
        private DateTime todayDate = DateTime.Now.Date;
        public DateTime TodayDate
        {
            get { return todayDate; }
            set
            {
                if (todayDate != value)
                {
                    todayDate = value;
                    OnPropertyChanged("TodayDate");
                }
            }
        }
        private bool anyTourActive;
        public bool AnyTourActive
        {
            get { return anyTourActive; }
            set
            {
                if (anyTourActive != value)
                {
                    anyTourActive = value;
                    OnPropertyChanged(nameof(anyTourActive));
                    StartTourCommand.RaiseCanExecuteChanged();
                }
            }
        }
    }
}
