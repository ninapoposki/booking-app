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

        private void LoadReviews()
        {
            foreach(TourGradeDTO tourGrade in tourGradeService.GetById(SelectedTour.SelectedDateTime.Id))
            {
                if (imageService.GetFirstPath(tourGrade.Id,"TOURGRADE") != null)
                {
                    tourGrade.Path = imageService.GetFirstPath(tourGrade.Id,"TOURGRADE");
                }
                TourReviews.Add(tourGrade);
            }
        }
    }
}
