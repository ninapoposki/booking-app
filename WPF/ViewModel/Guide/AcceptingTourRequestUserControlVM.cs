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
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class AcceptingTourRequestUserControlVM : ViewModelBase
    {
        public TourRequestDTO TourRequest { get; set; }
        public MyICommand<DatePicker> LoadSpecifiedDatesCommand { get; set; }
        public MyICommand AddDateCommand { get; set; }
        public MyICommand FinishCommand { get; set; }
        public MyICommand DeleteDateCommand { get; set; }
        public MyICommand<TextBox> ChoosenDateTextChangedCommand{ get; set; }
        public NavigationService NavigationService { get; set; }
        public List<TourGuestDTO> Tourists { get; set; }
        public BreadCrumbsVM BreadCrumbsVM { get; set; }
        private TourStartDateService tourStartDateService;
        private TourRequestService tourRequestService;
        private TourGuestService tourGuestService;
        public DateTime SelectedDate { get; set; }
        public AcceptingTourRequestUserControlVM(TourRequestDTO tourRequest,NavigationService navigationService,ObservableCollection<BreadcrumbItem> breadcrumbs) 
        {
            NavigationService = navigationService;
            TourRequest = tourRequest;
            TourRequest.ChoosenDate = "";
            AddDateCommand = new MyICommand(OnAddDate);
            DeleteDateCommand=new MyICommand(OnDeleteDate);
            ChoosenDateTextChangedCommand = new MyICommand<TextBox>(TextChanged);
            FinishCommand = new MyICommand(OnFinishCommand, CanFinishCommand);
            BreadCrumbsVM = new BreadCrumbsVM(breadcrumbs);
            LoadSpecifiedDatesCommand = new MyICommand<DatePicker>(LoadSpecifiedDates);
            tourRequestService = new TourRequestService(Injector.Injector.CreateInstance<ITourRequestRepository>(), Injector.Injector.CreateInstance<ILocationRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>());
            tourStartDateService = new TourStartDateService(Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
            tourRequestService = new TourRequestService(Injector.Injector.CreateInstance<ITourRequestRepository>(), Injector.Injector.CreateInstance<ILocationRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>());
            tourGuestService = new TourGuestService(Injector.Injector.CreateInstance<ITourGuestRepository>());
            TourRequest.Username = tourGuestService.GetCreatorName(TourRequest.Id);
            SelectedDate = FindFirstAvailableDate();
            Tourists = tourGuestService.GetRequestGuest(TourRequest.Id);
        }
        private bool CanFinishCommand()
        {
            return TourRequest.ChoosenDate != "";
        }
        private void TextChanged(TextBox text)
        {
           if( text.Text != "") FinishCommand.RaiseCanExecuteChanged();
        }
        private void OnFinishCommand()
        {
            tourRequestService.UpdateState(TourRequest.ToTourRequest());
            NavigationService.Navigate(new TourRequestUserControl(NavigationService, BreadCrumbsVM.Breadcrumbs));
        }
        private DateTime FindFirstAvailableDate()
        {
            var startDate = DateTime.ParseExact(TourRequest.StartDate, "dd/MM/yyyy",CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(TourRequest.EndDate, "dd/MM/yyyy",CultureInfo.InvariantCulture);
            var unavailableDates = tourStartDateService.GetUnavailableDates(DateOnly.FromDateTime(startDate), DateOnly.FromDateTime(endDate));
            DateTime firstAvailableDate = startDate;
            while(startDate <= endDate)
            {
                if (!unavailableDates.Contains(startDate)){ firstAvailableDate = startDate; break; }
                else { startDate=startDate.AddDays(1); }
            }return firstAvailableDate;
        }
        private void OnDeleteDate()
        {
            TourRequest.ChoosenDate = "";
            Time = "";
        }
        private void OnAddDate()
        {
            var time = TryTimeParse(Time);
            DateTime date = SelectedDate.Date;
            date = date.Add(time.Value);
            TourRequest.ChoosenDate = date.ToString("dd/MM/yyyy HH:mm",CultureInfo.InvariantCulture);
        }
        private TimeSpan? TryTimeParse(string input)
        {
            if (TimeSpan.TryParse(input, out var time)) return time;
            return null;
        }
        private void LoadSpecifiedDates(DatePicker picker)
        {
            var startDate = DateTime.ParseExact(TourRequest.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(TourRequest.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            BlackoutDatesBeforeStartDate(picker, startDate);
            BlackoutDatesAfterEndDate(picker, endDate);
            BlackoutUnavailableDates(picker, startDate, endDate);
        }
       private void BlackoutDatesBeforeStartDate(DatePicker picker, DateTime startDate)
        {
            var today = DateTime.Now.Date;  
            if (startDate > today)
            {
                picker.BlackoutDates.Add(new CalendarDateRange(today, startDate.AddDays(-1)));
            }
        }
        private void BlackoutDatesAfterEndDate(DatePicker picker, DateTime endDate)
        {
            var maxDate = new DateTime(2025, 12, 31);                                                     
            if (endDate < maxDate)
            {
                picker.BlackoutDates.Add(new CalendarDateRange(endDate.AddDays(1), maxDate));
            }
        }
        private void BlackoutUnavailableDates(DatePicker picker, DateTime startDate, DateTime endDate)
        {
            var unavailableDates = tourStartDateService.GetUnavailableDates(DateOnly.FromDateTime(startDate), DateOnly.FromDateTime(endDate));
            foreach (var date in unavailableDates)
            {
                picker.BlackoutDates.Add(new CalendarDateRange(date));
            }
        }
        private string time;
        public string Time
        {
            get { return time; }
            set
            {if (time != value){
                 time = value;
                 OnPropertyChanged("Time");}}
        }
     
    }
}
