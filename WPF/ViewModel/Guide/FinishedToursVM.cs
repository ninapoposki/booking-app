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
    public class FinishedToursVM:ViewModelBase
    {
        public ObservableCollection<TourDTO> FinishedTours { get; set; }
        private TourStartDateService tourStartDateService;
        private TourReservationService tourReservationService;
        private ImageService imageService;
        private int userId;
        public FinishedToursVM(int userId)
        { 
            this.userId = userId;
            FinishedTours = new ObservableCollection<TourDTO>();
            tourStartDateService = new TourStartDateService();
            tourReservationService = new TourReservationService();
            imageService = new ImageService();  
            LoadFinishedTours();
        }
        private void LoadFinishedTours()
        {
            List<TourDTO> finishedTours = tourStartDateService.GetAllFinishedTours(userId);
            foreach (TourDTO tour in finishedTours) { FinishedTours.Add(tour); }
            SetNumberOfTourists();
        }
        private void SetNumberOfTourists()
        {
            foreach(TourDTO tour in FinishedTours)
            {
                tour.NumberOfTourists = tourReservationService.GetFinishedToursGuests(tour.SelectedDateTime.Id).Count();
                if(imageService.GetFirstPath(tour.Id, "TOUR") != null)
                {
                    tour.Path = imageService.GetFirstPath(tour.Id, "TOUR");
                }
            }
        }
    }
}
