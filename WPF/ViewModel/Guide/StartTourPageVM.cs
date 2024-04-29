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
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class StartTourPageVM : ViewModelBase
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
                    OnPropertyChanged(nameof(SelectedTabIndex));
                }
            }
        }
        private TourGuestDTO selectedTourist;
        public TourGuestDTO SelectedTourist
        {
            get { return selectedTourist; }
            set
            {
                if (selectedTourist != value)
                {
                    selectedTourist = value;
                    OnPropertyChanged(nameof(SelectedTourist));
                    MarkAsPresentCommand.RaiseCanExecuteChanged();
                }
            }
        }
        private int tourId;
        private int userId;
        private int currentCheckPointIndex = 0;
        public MyICommand NextStopCommand { get; }
        public MyICommand MarkAsPresentCommand { get; }
        public MyICommand EndTourCommand { get; }
        private CheckPointService checkPointService;
        private TourReservationService tourReservationService;
        private TourGuestService tourGuestService;
        private TourStartDateService tourStartDateService;
        private CheckPointDTO currentCheckPoint;
        private TourService tourService;
        public TourStartDateDTO TourStartDate { get; set; }
        public List<CheckPointDTO> ToursCheckPoints { get; set; }    
        public ObservableCollection<TourGuestDTO> TourGuests { get; set; }
        public NavigationService NavigationService { get; set; }
        public ActiveTourDTO ActiveTour { get; set; }    
        public StartTourPageVM(NavigationService navigationService, TourStartDateDTO selectedStartDate,int userId) 
        {
            NextStopCommand = new MyICommand(OnNextStop);
            MarkAsPresentCommand=new MyICommand(OnMarkAsPresent,CanMarkAsPresent);
            EndTourCommand = new MyICommand(OnEndTour);
            NavigationService = navigationService;
            SelectedTabIndex = 2;
            TourStartDate = selectedStartDate;
            tourId = selectedStartDate.TourId;
            this.userId = userId;
            tourService = new TourService(Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(),Injector.Injector.CreateInstance<ILocationRepository>());
            tourGuestService = new TourGuestService(Injector.Injector.CreateInstance<ITourGuestRepository>());
            checkPointService = new CheckPointService(Injector.Injector.CreateInstance<ICheckPointRepository>());
            tourReservationService = new TourReservationService(Injector.Injector.CreateInstance<ITourReservationRepository>(), Injector.Injector.CreateInstance<ITourGuestRepository>(),
             Injector.Injector.CreateInstance<IUserRepository>(), Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(),
             Injector.Injector.CreateInstance<ILanguageRepository>(),
             Injector.Injector.CreateInstance<ILocationRepository>());
            tourStartDateService = new TourStartDateService(Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            currentCheckPoint =new CheckPointDTO();
            ToursCheckPoints =new List<CheckPointDTO>();
            TourGuests=new ObservableCollection<TourGuestDTO>();
            tourStartDateService.UpdateStartTime(selectedStartDate.Id);
            ActiveTour = new ActiveTourDTO(tourService.GetTour(TourStartDate.TourId));
            ActiveTour.NumberOfTourists = tourReservationService.GetByStartDate(TourStartDate.Id).Count();
            InitializeData();
        }
        private void InitializeData()
        {
            LoadCheckPoints();
            LoadTourists();
        }
        private void LoadTourists()
        {
            TourGuests.Clear();
            foreach (TourGuestDTO guests in tourReservationService.GetByStartDate(TourStartDate.Id)) { TourGuests.Add(guests); }
        }
        public void OnNextStop()
        {
            if (currentCheckPointIndex + 1 < ToursCheckPoints.Count)
            {
                currentCheckPointIndex++;
                UpdateUI();
                LoadTourists();
            }
        }
        public bool CanMarkAsPresent()
        {
            return SelectedTourist!=null;
        }
        public void OnMarkAsPresent()
        {
            tourGuestService.UpdatePresentGuest(SelectedTourist, currentCheckPoint);
            MessageBox.Show("Tourist marked as present!");
            LoadTourists();
        }
        public void OnEndTour()
        {
            MessageBox.Show("Tour has ended");
            FinishingTour();
        }
        private void LoadCheckPoints()
        {
            ToursCheckPoints = checkPointService.GetByTourId(tourId, TourStartDate.CurrentCheckPointId);
            UpdateUI();
        }
        private void UpdateUI()
        {
            UpdateCurrentCheckPoint();
            CheckAndFinishTourIfNeeded();
        }
        private void UpdateCurrentCheckPoint()
        {
            if (ToursCheckPoints != null && ToursCheckPoints.Count > currentCheckPointIndex)
            {
                currentCheckPoint = ToursCheckPoints[currentCheckPointIndex];
                ActiveTour.CheckPointName = currentCheckPoint.Name;
                ActiveTour.CheckPointType = currentCheckPoint.Type;
                tourStartDateService.UpdateCurrentCheckPoint(currentCheckPoint.Id, TourStartDate.Id);
            }
        }
        private void CheckAndFinishTourIfNeeded()
        {
            if (currentCheckPointIndex + 1 == ToursCheckPoints.Count) { CheckAndFinishTour(); }       
        }
        private void CheckAndFinishTour()
        {
            if (currentCheckPoint.Type == "END") { MessageBox.Show("You reached last check point, tour ended!"); FinishingTour(); }
        }
        public void FinishingTour()
        {
            tourStartDateService.UpdateEndTime(TourStartDate.Id);
            NavigationService.Navigate(new GuideHomePage(NavigationService, userId));
        }
    }
}
