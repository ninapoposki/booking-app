using BookingApp.DTO;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Guide
{
    internal class UpcomingToursVM
    {
        private TourService tourService;
        private TourStartDateService tourStartDateService;
        private VoucherService voucherService;
        private int userId;
        public ObservableCollection<TourDTO> UpcomingTours { get; set; }
        public TourDTO SelectedTour { get; set; }
        public UpcomingToursVM(int userId)
        {
            this.userId = userId;
            tourService = new TourService();
            tourStartDateService = new TourStartDateService();
            voucherService = new VoucherService();
            UpcomingTours = new ObservableCollection<TourDTO>();
            SelectedTour = new TourDTO();
            LoadUpcomingTours();
        }
        private void LoadUpcomingTours()
        {
            foreach (TourDTO tour in tourService.GetAllForUser(userId))
            {
                List<TourStartDateDTO> filteredDates = GetUpcomingTourDates(tour.Id);
                if (filteredDates.Count > 0)
                {
                    tour.DateTimes = new ObservableCollection<TourStartDateDTO>(filteredDates);
                    UpcomingTours.Add(tour);
                }
            }
        }
        private List<TourStartDateDTO> GetUpcomingTourDates(int tourId)
        {
            IEnumerable<TourStartDateDTO> allTourStartDates = tourStartDateService.GetTourDates(tourId);
            List<TourStartDateDTO> filteredTourDates = allTourStartDates.Where(tourStart => AreToursUpcoming(tourStart)).ToList();
            return filteredTourDates;
        }
        private bool AreToursUpcoming(TourStartDateDTO tourStartDate)
        {
            return tourStartDate.StartDateTime >= DateTime.Today && tourStartDate.TourStatus.ToString().Equals("INACTIVE");
        }
        private bool CanTourBeCancelled(TourStartDateDTO tourStart)
        {
            TimeSpan timeDifference = tourStart.StartDateTime - DateTime.Now;
            return timeDifference.TotalHours > 48;
        }
        public void CancelTourClick()
        {
            if (CanTourBeCancelled(SelectedTour.SelectedDateTime))
            {
                if (!IsVaucherGranted(SelectedTour.SelectedDateTime)) { MessageBox.Show("No reservation for this tour, no vauchers granted"); }
                   
                    tourStartDateService.UpdateTourStatus(SelectedTour.SelectedDateTime.Id);
                    SelectedTour.DateTimes.Remove(SelectedTour.SelectedDateTime);
                    if(SelectedTour.SelectedDateTime == null) { UpcomingTours.Remove(SelectedTour); }
            }
            else { MessageBox.Show("Chosen tour can not be canceled!"); }
        }
        private bool IsVaucherGranted(TourStartDateDTO tourStart)
        {
            return voucherService.GrantVoucher(tourStart, "Tour has been cancelled, you have granted a voucher!");
        }
    }
}

