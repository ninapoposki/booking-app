using BookingApp.Domain.IRepositories;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class TourRequestStatisticsUserControlVM:ViewModelBase
    {
        public NavigationService NavigationService { get; set; }
        public List<LanguageDTO> LanguageComboBox { get; set; }
        public List<LocationDTO> LocationComboBox { get; set; }
        public SeriesCollection ChartSeries { get; set; }

        private LanguageDTO selectedLanguage;
        public LanguageDTO SelectedLanguage
        {
            get => selectedLanguage;
            set
            {
                if (selectedLanguage != value)
                {
                    selectedLanguage = value;
                    OnPropertyChanged(nameof(SelectedLanguage));
                    OnPropertyChanged(nameof(IsLocationComboBoxEnabled));
                    if (selectedLanguage != null) { SelectedLocation = null; }
                    //if (SelectedYear != 0) { LoadMonthlyStatistics(SelectedYear); }
                }
            }
        }
        private LocationDTO selectedLocation;
        public LocationDTO SelectedLocation
        {
            get => selectedLocation;
            set
            {
                if (selectedLocation != value)
                {
                    selectedLocation = value;
                    OnPropertyChanged(nameof(SelectedLocation));
                    OnPropertyChanged(nameof(IsLanguageComboBoxEnabled));
                    if (selectedLocation != null){SelectedLanguage = null; }
                    //if (SelectedYear != 0) { LoadMonthlyStatistics(SelectedYear); }
                }
            }
        }
        private int selectedYear;
        public int SelectedYear
        {
            get => selectedYear;
            set
            {
                if (selectedYear != value)
                {
                    selectedYear = value;
                    OnPropertyChanged(nameof(SelectedYear));
                    if (SelectedLocation != null || SelectedLanguage!= null) { LoadMonthlyStatistics(value); }               
                }
            }
        }
        public ObservableCollection<KeyValuePair<int,int>> StatisticsPerYear {  get; set; }
        public List<int> Years { get; set; }
        public List<string> Months { get; set; }
        public bool IsLanguageComboBoxEnabled => selectedLocation == null;
        public bool IsLocationComboBoxEnabled => selectedLanguage == null;
        public MyICommand FilterCommand {  get; set; }

        private LocationService locationService;
        private LanguageService languageService;
        private TourRequestService tourRequestService;
        public TourRequestStatisticsUserControlVM(NavigationService navigationService) 
        {
            NavigationService = navigationService;
            languageService = new LanguageService(Injector.Injector.CreateInstance<ILanguageRepository>());
            locationService = new LocationService(Injector.Injector.CreateInstance<ILocationRepository>());
            tourRequestService = new TourRequestService(Injector.Injector.CreateInstance<ITourRequestRepository>(), Injector.Injector.CreateInstance<ILocationRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>());
            LanguageComboBox= new List<LanguageDTO>();
            LocationComboBox= new List<LocationDTO>();
            StatisticsPerYear= new ObservableCollection<KeyValuePair<int,int>>();
            FilterCommand = new MyICommand(OnFilterTour);
            ChartSeries = new SeriesCollection();
            Years = new List<int>();
            Months = GetMonths();
            LoadYears();
            LoadLanguagesAndLocation();
            InitializeChartWithEmptyData();
        }
        public List<string> GetMonths()
        {
            var months = new List<string>();
            for (int i = 1; i <= 12; i++) { months.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i)); }
            return months;
        }
        private void LoadLanguagesAndLocation()
        {
            languageService.GetAll(LanguageComboBox);
            locationService.GetAll(LocationComboBox);
            OnPropertyChanged(nameof(LanguageComboBox));
            OnPropertyChanged(nameof(LocationComboBox));
        }
        private void LoadYears()
        {
            for (int year = 2015; year <= DateTime.Now.Year; year++) { Years.Add(year); }
        }
        private void OnFilterTour()
        {
            if (SelectedLanguage != null || SelectedLocation!= null) {
                GetSatisticsPerYear();
                if (SelectedYear != 0) { LoadMonthlyStatistics(SelectedYear); }
            }     
        }
        private void GetSatisticsPerYear()
        {
            StatisticsPerYear.Clear();
            int typeId = (SelectedLanguage != null && SelectedLanguage.Id != 0) ? SelectedLanguage.Id : SelectedLocation.Id;
            string type = SelectedLanguage != null ? "language" : "location";
            foreach (var year in Years)
            {
                int value=tourRequestService.GetStatisticsPerYear(typeId,type,year);
                StatisticsPerYear.Add(new KeyValuePair<int, int> ( year, value ));
            }
        }
        private void InitializeChartWithEmptyData()
        {
            ChartSeries = new SeriesCollection
            {
                new ColumnSeries{ Values = new ChartValues<int> { 0 } }
            };
        }
        private void LoadMonthlyStatistics(int year)
        {
            var columnSeries = ChartSeries.FirstOrDefault() as ColumnSeries;
            if (columnSeries == null)
            {
                columnSeries = new ColumnSeries { Values = new ChartValues<int>() };
                ChartSeries.Add(columnSeries);
            }columnSeries.Values.Clear();
            int typeId = (SelectedLanguage != null && SelectedLanguage.Id != 0) ? SelectedLanguage.Id : SelectedLocation.Id;
            string type = SelectedLanguage != null ? "language" : "location";
            foreach (var month in Months)
            {
                int monthIndex = Months.IndexOf(month) + 1;  
                int requests = tourRequestService.GetStatisticsPerMonth(typeId, type, year, monthIndex);
                columnSeries.Values.Add(requests);
            }ChartSeries.Add(columnSeries);
        }
    }
}
