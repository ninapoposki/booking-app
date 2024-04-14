using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.Services;
using BookingApp.WPF.View.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class TourCheckPointsVM:ViewModelBase
    {
         private int tourId;
         private int currentCheckPointIndex = 0;
         private TourService tourService;
         private TourStartDateService tourStartDateService;
         private TourGuestService tourGuestService;
         private TourReservationService tourReservationService;
         private CheckPointService checkPointService;
         private CheckPointDTO currentCheckPoint;
         private TourStartDateDTO selectedStartDate;
         public List<TourGuestDTO> PresentTourists { get; set; }
         public ObservableCollection<TourGuestDTO> Guests { get; set; }
         public List<CheckPointDTO> ToursCheckPoints { get; set; }
         public List<TourGuestDTO> SelectedTourists { get; set; }
        public TourCheckPointsVM(TourStartDateDTO selectedStartDate)
         {
             this.selectedStartDate = selectedStartDate;
             tourService = new TourService();
             checkPointService = new CheckPointService();
             tourGuestService = new TourGuestService();
             tourReservationService = new TourReservationService();
             tourStartDateService = new TourStartDateService();
             Guests = new ObservableCollection<TourGuestDTO>();
             tourId = this.selectedStartDate.TourId;
             PresentTourists = new List<TourGuestDTO>();
             ToursCheckPoints = new List<CheckPointDTO>();
             SelectedTourists= new List<TourGuestDTO>();
             tourStartDateService.UpdateStartTime(selectedStartDate.Id);
             LoadCheckPoints();
             LoadTourists();
         }
         private void LoadCheckPoints()
         {  
             ToursCheckPoints=checkPointService.GetByTourId(tourId,selectedStartDate.CurrentCheckPointId);
             UpdateUI();
         }
         private void LoadTourists()
         {
             Guests.Clear();
             foreach(TourGuestDTO guests in tourReservationService.GetByStartDate(selectedStartDate.Id))
             {
                Guests.Add(guests);
             }
         }
         public void MarkAsPresentClick()
         {
             foreach (TourGuestDTO tourGuest in SelectedTourists)
             {
                tourGuestService.UpdatePresentGuest(tourGuest, currentCheckPoint);
             }
             MessageBox.Show("Tourist marked as present!");
             LoadTourists();
         }
         public void NextCheckPointClick()
         {
             if (currentCheckPointIndex + 1 < ToursCheckPoints.Count)
             {
                 currentCheckPointIndex++;
                 UpdateUI();
                 LoadTourists();
             }
         }
         private void CheckAndFinishTour()
         {
             if (currentCheckPoint.Type == "END")
             {
                 MessageBox.Show("You reached last check point, tour ended!");
                 FinishingTour();
             }
         }
         public void FinishingTour()
         {
            tourStartDateService.UpdateEndTime(selectedStartDate.Id);
         }
         private void UpdateUI()
         {
             if (ToursCheckPoints != null && ToursCheckPoints.Count > currentCheckPointIndex)
             {
                 currentCheckPoint = ToursCheckPoints[currentCheckPointIndex];
                 CheckPointName = currentCheckPoint.Name;
                 CheckPointType = currentCheckPoint.Type;
                 tourStartDateService.UpdateCurrentCheckPoint(currentCheckPoint.Id,selectedStartDate.Id);
            }
             if (currentCheckPointIndex + 1 == ToursCheckPoints.Count)
             {
                 CheckAndFinishTour();
             }
         }
         public void EndTourClick()
         {
             MessageBox.Show("Tour has ended");
             FinishingTour();
         }
         private string checkPointName;
         public string CheckPointName
         {
             get { return checkPointName; }
             set
             {
                 if (checkPointName != value)
                 {
                     checkPointName = value;
                     OnPropertyChanged("CheckPointName");
                 }
             }
         }
         private string checkPointType;
         public string CheckPointType
         {
             get { return checkPointType; }
             set
             {
                 if (checkPointType != value)
                 {
                     checkPointType = value;
                     OnPropertyChanged("CheckPointType");
                 }
             }
         }
        public void TouristListSelectionChaged(object sender, SelectionChangedEventArgs e)
        {
            foreach (TourGuestDTO tourGuest in e.AddedItems)
            {
                if (!SelectedTourists.Contains(tourGuest))
                {
                    SelectedTourists.Add(tourGuest);
                }
            }
            foreach (TourGuestDTO tourGuest in e.RemovedItems)
            {
                SelectedTourists.Remove(tourGuest);
            }
        }
    }
}


