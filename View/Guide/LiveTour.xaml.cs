using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApp.View.Guide
{
    /// <summary>
    /// Interaction logic for LiveTour.xaml
    /// </summary>
    public partial class LiveTour : Window,INotifyPropertyChanged
    {
        public ObservableCollection<TourDTO> Tours { get; set; }
        private TourRepository tourRepository;
        private TourStartDateRepository tourStartDateRepository;
        private LocationRepository locationRepository;
        private LanguageRepository languageRepository;
        private TourReservationRepository tourReservationRepository;
        public TourDTO SelectedTour { get; set; }
       
        public LiveTour()
        {
            InitializeComponent();
            DataContext = this;
            Tours = new ObservableCollection<TourDTO>();
            SelectedTour=new TourDTO();
            //SelectedDateTime = new TourStartDateDTO();
            tourRepository = new TourRepository();
            tourStartDateRepository = new TourStartDateRepository();
            locationRepository = new LocationRepository();
            languageRepository = new LanguageRepository();
            tourReservationRepository = new TourReservationRepository();

            LoadTodaysTours();
        }

        private void LoadTodaysTours()
        {
            foreach(TourStartDate tourStartDate in tourStartDateRepository.GetAll())
            {
                if (AreToursToday(tourStartDate))
                {
                    TourDTO newTodayTour = GetTour(tourStartDate);
                    if (!AlreadyExist(newTodayTour.Id))
                    {
                        newTodayTour.DateTimes = new ObservableCollection<TourStartDateDTO>(UpdateDate(tourStartDate.TourId));
                        Tours.Add(newTodayTour);
                    }
                }
            }
        
        }
        

        bool AlreadyExist(int newTourId)
        {
            foreach(var tour in Tours)
            {
                if(tour.Id == newTourId)
                {
                    return true;
                }
            }
            return false;
        }
        private TourDTO GetTour(TourStartDate tourDate) 
        {
            Tour? todayTour = tourRepository.GetById(tourDate.TourId);
            Location location = locationRepository.GetById(todayTour.LocationId);
            Language language = languageRepository.GetById(todayTour.LanguageId);
            return new TourDTO(todayTour, location, language);
        }
        private bool AreToursToday(TourStartDate tourStart)
        {
            return tourStart.StartTime.Date == todayDate && tourStart.HasStarted == false && tourStart.HasFinished == false;
        }
        private IEnumerable<TourStartDateDTO> UpdateDate(int tourId)
        {
            var dateTimesForTour = new List<TourStartDateDTO>();
            foreach (var startTime in tourStartDateRepository.GetByTourId(tourId))
            {
                dateTimesForTour.Add(new TourStartDateDTO(startTime));
            }
            return dateTimesForTour;
        }

        private DateTime todayDate=DateTime.Now.Date;

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
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void StartTourClick(object sender, RoutedEventArgs e)
        {
            if (!DoReservationExist())
            {
                MessageBox.Show("There are no reservations for selected tour and date");
                return;
            }
            TourCheckPoints tourCheckPoints = new TourCheckPoints(SelectedTour.SelectedDateTime);
            tourCheckPoints.Owner = this;
            tourCheckPoints.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            tourCheckPoints.Show();
           
        }
        private bool DoReservationExist()
        {
            return tourReservationRepository.GetByTourDateId(SelectedTour.SelectedDateTime.Id).Count>0;
        }
    }
}

