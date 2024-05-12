using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
namespace BookingApp.WPF.ViewModel.Tourist
{   public class TourRequestViewWindowVM : ViewModelBase
    {   public NavigationService NavigationService { get; set; }
        public ObservableCollection<TourRequestDTO> TourRequests { get; set; }
        public ObservableCollection<TourRequestDTO> AllTourRequests { get; set; }
        public ObservableCollection<TourRequestDTO> AcceptedTourRequests { get; set; }
        public ObservableCollection<TourDTO> FilteredTours { get; set; }
        private TourRequestService tourRequestService;
        private LocationService locationService;
        private LanguageService languageService;
        private TourService tourService;
        private  ImageService imageService;
        private TourStartDateService tourStartDateService;
        private DispatcherTimer timer;
        public double AcceptedPercentage { get; set; }
        public double AverageNumberOfTourists { get; set; }
        public double NotAcceptedPercentage { get; set; }
        public Dictionary<string, int> RequestsByLanguage { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> RequestsByLocation { get; set; } = new Dictionary<string, int>();
        public SeriesCollection LanguageSeries { get; set; }
        public SeriesCollection LocationSeries { get; set; }
        public MyICommand<TourRequestDTO> MarkAsReadCommand { get; set; }
        public TourRequestViewWindowVM(NavigationService navigationService)
        {    NavigationService = navigationService;
            locationService = new LocationService(Injector.Injector.CreateInstance<ILocationRepository>());
            languageService = new LanguageService(Injector.Injector.CreateInstance<ILanguageRepository>());
            tourService = new TourService(Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            tourStartDateService = new TourStartDateService(Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            AllTourRequests = new ObservableCollection<TourRequestDTO>();
            tourRequestService = new TourRequestService(Injector.Injector.CreateInstance<ITourRequestRepository>(), Injector.Injector.CreateInstance<ILocationRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>());
            TourRequests = new ObservableCollection<TourRequestDTO>();
            AcceptedTourRequests = new ObservableCollection<TourRequestDTO>();
            FilteredTours = new ObservableCollection<TourDTO>();
            LoadTourRequests(); LoadAcceptedTourRequests();ExpirePendingRequests();CalculateStatistics();LoadAllTourRequests(); CalculateRequestsByLocation();CalculateRequestsByLanguage(); CalculateAverageNumberOfTourists();LoadFilteredTours();
            MarkAsReadCommand = new MyICommand<TourRequestDTO>(MarkAsRead);
        }
        private void LoadTourRequests()
        {   TourRequests.Clear();
            var tourRequestDTOs = tourRequestService.GetAllNonAcceptedTourRequests();
            foreach (TourRequestDTO dto in tourRequestDTOs){
                TourRequests.Add(dto); }}
        public void MarkAsRead(TourRequestDTO tourRequest)
        { if (tourRequest == null) {
                MessageBox.Show("Please select a request.");
                return;}
            tourRequest.IsNotified = true;
            tourRequestService.UpdateRequest(tourRequest);
            OnPropertyChanged(nameof(TourRequests));
            SortTourRequests(); }
        private void SortTourRequests()
        {List<TourRequestDTO> sortedRequests = AcceptedTourRequests.OrderBy(t => t.IsNotified).ToList();
            AcceptedTourRequests.Clear();
            foreach (TourRequestDTO request in sortedRequests)
            {AcceptedTourRequests.Add(request);}}
        private void LoadAcceptedTourRequests()

        {   AcceptedTourRequests.Clear();
            var acceptedTourRequestDTOs = tourRequestService.GetAllAcceptedTourRequests();
            foreach (TourRequestDTO dto in acceptedTourRequestDTOs)
            {AcceptedTourRequests.Add(dto);} }

        private void ExpirePendingRequests()
        {   DateTime currentDate = DateTime.Now;
            foreach (TourRequestDTO dto in TourRequests) {
                if (dto.State == State.PENDING && DateTime.ParseExact(dto.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= currentDate.AddDays(2))
                {   dto.State = State.EXPIRED;
                    TourRequest tourRequest = dto.ToTourRequest();
                    tourRequest.State = State.EXPIRED;
                    tourRequestService.Update(tourRequest);}} }
        public void CalculateStatistics(int? year = null)
        {   var requests = tourRequestService.GetAll().ToList();
            if (year.HasValue)
            { requests = requests.Where(r => r.StartDate.Year == year.Value).ToList(); }
            int totalRequests = requests.Count();
            int acceptedRequests = requests.Count(r => r.State == State.ACCEPTED);
            int notAcceptedRequests = totalRequests - acceptedRequests;
            AcceptedPercentage = totalRequests > 0 ? (double)acceptedRequests / totalRequests * 100 : 0;
            NotAcceptedPercentage = totalRequests > 0 ? (double)notAcceptedRequests / totalRequests * 100 : 0;
            OnPropertyChanged(nameof(AcceptedPercentage));
            OnPropertyChanged(nameof(NotAcceptedPercentage)); }
        private void CalculateRequestsByLanguage()
        {var languageCounts = AllTourRequests.GroupBy(req => req.LanguageId).Select(group => new { LanguageId = group.Key, Count = group.Count() });
            RequestsByLanguage.Clear();
            foreach (var item in languageCounts)
            {   string languageName = languageService.GetById(item.LanguageId).Name;
                RequestsByLanguage.Add(languageName, item.Count);}
            LanguageSeries = new SeriesCollection();
            foreach (var item in RequestsByLanguage)
            {   LanguageSeries.Add(new PieSeries
                { Title = item.Key, Values = new ChartValues<int> { item.Value }, DataLabels = true });
            }
            OnPropertyChanged(nameof(RequestsByLanguage)); }
        private void CalculateRequestsByLocation()
        { var locationCounts = AllTourRequests.GroupBy(req => req.LocationId).Select(group => new { LocationId = group.Key, Count = group.Count() });
            RequestsByLocation.Clear();
            foreach (var item in locationCounts)
            { string location = locationService.GetById(item.LocationId).City;
                RequestsByLocation.Add(location, item.Count); }
             LocationSeries = new SeriesCollection();
            foreach (var item in RequestsByLocation)
            {LocationSeries.Add(new ColumnSeries {
                    Title = item.Key,Values = new ChartValues<int> { item.Value }});}
            OnPropertyChanged(nameof(RequestsByLocation));}
        public void CalculateAverageNumberOfTourists(int? year = null)
        { List<TourRequestDTO> acceptedRequests = AcceptedTourRequests.ToList();
            if (year.HasValue)
            { acceptedRequests = acceptedRequests.Where(r => DateTime.ParseExact(r.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).Year == year.Value).ToList(); }
            if (acceptedRequests.Any()){
                AverageNumberOfTourists = acceptedRequests.Average(r => r.NumberOfTourists); }
            else{ AverageNumberOfTourists = 0; }
            OnPropertyChanged(nameof(AverageNumberOfTourists)); } 
        private void LoadAllTourRequests()
        {var allTourRequests = tourRequestService.GetAll();
            foreach (var request in allTourRequests) {
                Location location = locationService.GetById(request.LocationId);
                Language language = languageService.GetById(request.LanguageId);
                TourRequestDTO dto = new TourRequestDTO(request, location, language);
                AllTourRequests.Add(dto);}}
        private void LoadFilteredTours()
        {   List<TourRequestDTO> expiredAndPendingRequests = TourRequests.Where(t => t.State == State.EXPIRED || t.State == State.PENDING).ToList();
            List<TourDTO> allTours = tourService.GetAll(); 
            foreach (TourDTO tour in allTours){
                if (expiredAndPendingRequests.Any(req => req.LocationId == tour.Location.Id || req.LanguageId == tour.Language.Id))
                {   SetTourImages(tour);
                    SetTourDates(tour);
                    FilteredTours.Add(tour);}}}
        private void SetTourImages(TourDTO tour)
        {   List<ImageDTO> images = imageService.GetImagesForTour(tour.Id); 
            foreach (ImageDTO image in images)
            {tour.Images.Add(new ImageDTO { Path = image.Path });}}
         private void SetTourDates(TourDTO tour)
            {ObservableCollection<TourStartDateDTO> dateTimes = new ObservableCollection<TourStartDateDTO>(tourStartDateService.GetTourDates(tour.Id));
            tour.DateTimes = dateTimes;  }}
}
