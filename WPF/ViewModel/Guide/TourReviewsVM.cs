using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class TourReviewsVM:ViewModelBase
    {
        public TourDTO SelectedTour { get; set; }
        public ObservableCollection<TourGradeDTO> TourReviews {  get; set; }
        private TourGradeService tourGradeService;
        private ImageService imageService;
        public TourReviewsVM(TourDTO tour) 
        {
            SelectedTour = tour;
            tourGradeService= new TourGradeService();
            imageService = new ImageService();
            TourReviews= new ObservableCollection<TourGradeDTO>();
            LoadReviews();
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
        public void ReportReviewClick()
        {
            SelectedReview.Validity = Domain.Model.Validity.NO;
            tourGradeService.UpdateValidity(SelectedReview.Id);
        }
        private TourGradeDTO selectedReview;
        public TourGradeDTO SelectedReview
        {
            get { return selectedReview; }
            set
            {
                if (selectedReview != value)
                {
                    selectedReview = value;
                    OnPropertyChanged("SelectedReview");
                }
            }
        }
    }
}
