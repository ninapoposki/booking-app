using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using BookingApp.WPF.View.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class TourRequestUserControlVM : ViewModelBase
    {
        public List<LocationDTO> LocationComboBox { get; set; }
        public List<LanguageDTO> LanguageComboBox { get; set; }
        public TourRequestSearchDTO SearchParametars { get; set; }
        public MyICommand SearchCommand { get; set; }
        public MyICommand ClearSearchCommand { get; set; }
        public MyICommand<TourRequestDTO> AcceptTourRequestCommand {  get; set; }
        public MyICommand SeeRequestStatistics { get; set; }
        public ObservableCollection<TourRequestDTO> TourRequests { get; set; }
        public NavigationService NavigationService { get; set; }
        public BreadCrumbsVM BreadCrumbVM { get; set; }
        private LocationService locationService;
        private LanguageService languageService;
        private TourRequestService tourRequestService;
        private TourGuestService tourGuestService;
        public TourRequestUserControlVM(NavigationService navigationService, ObservableCollection<BreadcrumbItem> breadcrumbs)
        {
            NavigationService = navigationService;
            SearchCommand = new MyICommand(OnSearchTourRequest);
            ClearSearchCommand = new MyICommand(OnClearSearch);
            SeeRequestStatistics = new MyICommand(OnRequestStatistics);
            AcceptTourRequestCommand=new MyICommand<TourRequestDTO>(OnAcceptTourRequest);
            BreadCrumbVM = new BreadCrumbsVM(breadcrumbs);
            LanguageComboBox = new List<LanguageDTO>();
            LocationComboBox = new List<LocationDTO>();
            SearchParametars = new TourRequestSearchDTO();
            TourRequests = new ObservableCollection<TourRequestDTO>();
            locationService = new LocationService(Injector.Injector.CreateInstance<ILocationRepository>());
            languageService = new LanguageService(Injector.Injector.CreateInstance<ILanguageRepository>());
            tourGuestService=new TourGuestService(Injector.Injector.CreateInstance<ITourGuestRepository>());
            tourRequestService = new TourRequestService(Injector.Injector.CreateInstance<ITourRequestRepository>(), Injector.Injector.CreateInstance<ILocationRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>());
            LoadLanguagesAndCities();
            SearchParametars.PropertyChanged += SearchParametarsPropertyChanged;
            LoadTourRequests();
        }
        private void OnRequestStatistics(){
            NavigationService.Navigate(new TourRequestStatisticsUserControl(NavigationService));
        }
        private void GetUser(TourRequestDTO tourRequest)
        {
            tourRequest.Username=tourGuestService.GetCreatorName(tourRequest.Id);
        }
        private void OnAcceptTourRequest(TourRequestDTO request)
        {
            NavigationService.Navigate(new AcceptingTourRequestUserControl(request, NavigationService, BreadCrumbVM.Breadcrumbs));
        }
        private void SearchParametarsPropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == "SelectedCity") { LoadCountry(); }
        }
        private void LoadLanguagesAndCities() {
            languageService.GetAll(LanguageComboBox);
            locationService.GetAll(LocationComboBox);
        }
        private void LoadCountry() {
            SearchParametars.Country = locationService.GetCountry(SearchParametars.SelectedCity);
        }
        private void LoadTourRequests() {
            TourRequests.Clear();
            foreach (TourRequestDTO tourRequest in tourRequestService.GetAllUnaccepted()) { TourRequests.Add(tourRequest);  GetUser(tourRequest); }
        }
        private void OnClearSearch() {
            SearchParametars.SelectedLanguage = null;
            SearchParametars.SelectedCity = null;
            SearchParametars.StartDate = DateTime.Now;
            SearchParametars.EndDate = DateTime.Now;
            SearchParametars.NumberOfTourists = 0;
            LoadTourRequests();
        }
        private void OnSearchTourRequest() {
            List<TourRequestDTO> filteredTours = FilterTours();
            TourRequests.Clear();
            foreach (TourRequestDTO request in filteredTours) { TourRequests.Add(request); GetUser(request); }
        }
        private List<TourRequestDTO> FilterTours() {
            return TourRequests.Where(tr => IsMatchLocation(tr) && IsMatchLanguage(tr) && IsMatchNumberOfTourists(tr) && IsMatchDates(tr)).ToList(); }
        private bool IsMatchDates(TourRequestDTO tourRequest){
            var requestStartDate = DateOnly.ParseExact(tourRequest.StartDate, "dd/MM/yyyy");
            var requestEndDate = DateOnly.ParseExact(tourRequest.EndDate, "dd/MM/yyyy");
            var searchStartDate = DateOnly.FromDateTime(SearchParametars.StartDate);
            var searchEndDate = DateOnly.FromDateTime(SearchParametars.EndDate);
            return IsDateWithinRange(requestStartDate, searchStartDate, searchEndDate) || IsDateWithinRange(requestEndDate, searchStartDate, searchEndDate) ||
                   AreDatesOverlapping(requestStartDate, requestEndDate, searchStartDate, searchEndDate);
        }
        private bool IsDateWithinRange(DateOnly dateToCheck, DateOnly rangeStart, DateOnly rangeEnd){
            return dateToCheck >= rangeStart && dateToCheck <= rangeEnd;
        }
        private bool AreDatesOverlapping(DateOnly requestStart, DateOnly requestEnd, DateOnly searchStart, DateOnly searchEnd){
            return requestStart <= searchEnd && requestEnd >= searchStart;
        }
        private bool IsMatchLocation(TourRequestDTO tourRequest){
            return tourRequest.Location.City.Contains(GetSelectedCity(), StringComparison.OrdinalIgnoreCase); }
        private bool IsMatchLanguage(TourRequestDTO tourRequest){
            return tourRequest.Language.Name.Contains(GetSelectedLanguage(), StringComparison.OrdinalIgnoreCase); }
        private bool IsMatchNumberOfTourists(TourRequestDTO tourRequest){
            return tourRequest.NumberOfTourists == SearchParametars.NumberOfTourists || SearchParametars.NumberOfTourists == 0; }
        private string GetSelectedCity(){
            if (SearchParametars.SelectedCity == null) return "";
            return SearchParametars.SelectedCity.City;
        }
        private string GetSelectedLanguage(){
            if (SearchParametars.SelectedLanguage == null) return "";
            return SearchParametars.SelectedLanguage.Name;
        } 
    }
}
