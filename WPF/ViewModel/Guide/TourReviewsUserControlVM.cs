using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class TourReviewsUserControlVM:ViewModelBase
    {
        public TourDTO SelectedTour { get; set; }
        public MyICommand<TourGradeDTO> ReportCommand { get; }
        public ObservableCollection<TourGradeDTO> TourReviews {  get; set; }
        private TourGradeService tourGradeService;
        private ImageService imageService;
        public TourReviewsUserControlVM(TourDTO tour) 
        {
            ReportCommand = new MyICommand<TourGradeDTO>(OnReportCommand);
            SelectedTour = tour;
            tourGradeService= new TourGradeService(Injector.Injector.CreateInstance<ICheckPointRepository>(),Injector.Injector.CreateInstance<ITourGradeRepository>(),
                Injector.Injector.CreateInstance<ITourReservationRepository>(), Injector.Injector.CreateInstance<ITourGuestRepository>(),
                Injector.Injector.CreateInstance<IUserRepository>(), Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(),
                Injector.Injector.CreateInstance<ILanguageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>());
            imageService = new ImageService(Injector.Injector.CreateInstance<IImageRepository>());
            TourReviews= new ObservableCollection<TourGradeDTO>();
            LoadReviews();
        }
        private void OnReportCommand(TourGradeDTO tourGrade)
        {
            tourGrade.Validity = Validity.NO;
            tourGradeService.UpdateValidity(tourGrade.Id);
        }
        private void SetImage(TourGradeDTO tour)
        {
            var path = imageService.GetFirstPath(tour.Id, "TOURGRADE");
            tour.Path = path ?? "..\\..\\..\\Resources\\Images\\placeholderGuide.png";
        }
        private void LoadReviews()
        {
            foreach(TourGradeDTO tourGrade in tourGradeService.GetById(SelectedTour.SelectedDateTime.Id))
            {
                SetImage(tourGrade);
                TourReviews.Add(tourGrade);
            }
            CalculateAverageGrade();
        }
        private void CalculateAverageGrade()
        {
            double sumGrade = 0;
            foreach(TourGradeDTO tourGrade in TourReviews)
            {
               sumGrade += (tourGrade.GuideKnowledge + tourGrade.LanguageKnowledge + tourGrade.TourAttractions);
            }  
            AverageGrade=sumGrade/(TourReviews.Count()*3);
        }
        private double averageGrade;
        public double AverageGrade
        {
            get { return averageGrade; }
            set
            {
                averageGrade = value;
                OnPropertyChanged("AverageGrade");
            }
        }
    }
}
