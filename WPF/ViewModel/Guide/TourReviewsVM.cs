using BookingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class TourReviewsVM:ViewModelBase
    {
        public TourDTO SelectedTour { get; set; }
        public TourReviewsVM(TourDTO tour) 
        {
            SelectedTour = tour;
        }
    }
}
