﻿using BookingApp.Domain.IRepositories;
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
             checkPointService = new CheckPointService(Injector.Injector.CreateInstance<ICheckPointRepository>());
             tourGuestService = new TourGuestService(Injector.Injector.CreateInstance<ITourGuestRepository>());
             tourReservationService = new TourReservationService(Injector.Injector.CreateInstance<ITourReservationRepository>(), Injector.Injector.CreateInstance<ITourGuestRepository>(),
              Injector.Injector.CreateInstance<IUserRepository>(), Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(),
              Injector.Injector.CreateInstance<ILanguageRepository>(),
              Injector.Injector.CreateInstance<ILocationRepository>());
             tourStartDateService = new TourStartDateService(Injector.Injector.CreateInstance<ITourStartDateRepository>(), Injector.Injector.CreateInstance<ITourRepository>(), Injector.Injector.CreateInstance<ILanguageRepository>(), Injector.Injector.CreateInstance<ILocationRepository>());
             Guests = new ObservableCollection<TourGuestDTO>();
             tourId = this.selectedStartDate.TourId;
             PresentTourists = new List<TourGuestDTO>();
             ToursCheckPoints = new List<CheckPointDTO>();
             SelectedTourists= new List<TourGuestDTO>();
             tourStartDateService.UpdateStartTime(selectedStartDate.Id);
             InitializeData();
         }
        private void InitializeData()
        {
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
             }//1
         }
         public void FinishingTour()
         {
            tourStartDateService.UpdateEndTime(selectedStartDate.Id);
         }
        private void UpdateUI()
        {
            UpdateCurrentCheckPoint();
            CheckAndFinishTourIfNeeded();
        }
        private void UpdateCurrentCheckPoint()
        {
            if (ToursCheckPoints != null && ToursCheckPoints.Count > currentCheckPointIndex)
            {
                currentCheckPoint = ToursCheckPoints[currentCheckPointIndex];
                CheckPointName = currentCheckPoint.Name;
                CheckPointType = currentCheckPoint.Type;
                tourStartDateService.UpdateCurrentCheckPoint(currentCheckPoint.Id, selectedStartDate.Id);
            }
        }
        private void CheckAndFinishTourIfNeeded()
        {
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
            SelectedTourists.AddRange(e.AddedItems.Cast<TourGuestDTO>().Where(guest => !SelectedTourists.Contains(guest)));
            foreach (TourGuestDTO guest in e.RemovedItems.Cast<TourGuestDTO>())
            {
                SelectedTourists.Remove(guest);
            }
        }
    }
}


