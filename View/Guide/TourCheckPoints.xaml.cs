using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApp.View.Guide
{
    /// <summary>
    /// Interaction logic for TourCheckPoints.xaml
    /// </summary>
    public partial class TourCheckPoints : Window
    {
        private int tourId;
        private int currentCheckPointIndex=0;
        private TourRepository tourRepository;
        private TourStartDateRepository tourStartDateRepository;
        private TourStartDateDTO selectedStartDate;
        private TourGuestRepository tourGuestRepository;
        private TourReservationRepository tourReservationRepository;
        public List<TourGuestDTO> presentTourists {  get; set; }
        public ObservableCollection<TourGuestDTO> guests {  get; set; }
        public List<CheckPointDTO> toursCheckPoints {  get; set; }
        private CheckPointRepository checkPointRepository;
        private CheckPointDTO currentCheckPoint;
        //private TourStartDate tourStart;
        public TourCheckPoints(TourStartDateDTO selectedStartDate)
        {
            InitializeComponent();
            DataContext = this;
            this.selectedStartDate = selectedStartDate;
          
           
            tourRepository = new TourRepository();
            checkPointRepository = new CheckPointRepository();
            tourGuestRepository = new TourGuestRepository();
            tourReservationRepository = new TourReservationRepository();
            tourStartDateRepository = new TourStartDateRepository();
            guests =new ObservableCollection<TourGuestDTO>();


            TourStartDate tourStart = this.selectedStartDate.ToTourStartDate();
            tourStart.HasStarted= true;
            tourStartDateRepository.Update(tourStart);
            tourId = this.selectedStartDate.TourId;

            presentTourists= new List<TourGuestDTO>();
            toursCheckPoints= new List<CheckPointDTO>();
            LoadCheckPoints();
            LoadTourists();
            UpdateUI();
          
        }
        private void LoadCheckPoints()
        {
            List<CheckPoint> checkPoints= checkPointRepository.GetByTourId(tourId);
            foreach(CheckPoint checkPoint in checkPoints)
            {
                toursCheckPoints.Add(new CheckPointDTO(checkPoint));
            }
            
            UpdateUI();
           
        }
        private void LoadTourists()
        {
            guests.Clear();
            List<TourReservation> tourReservations = tourReservationRepository.GetByTourDateId(selectedStartDate.Id);
            foreach(TourReservation reservation in tourReservations)
            {
                foreach(TourGuest guest in tourGuestRepository.GetAll())
                {
                    if (guest.TourReservationId == reservation.Id && guest.CheckPointId==-1)
                    {
                        guests.Add(new TourGuestDTO(guest));
                    }
                }
            }

        }
        private void MarkAsPresentClick(object sender, RoutedEventArgs e)
        {
            foreach(TourGuestDTO tourGuest in TouristList.SelectedItems)
            {
                TourGuest guest = tourGuest.ToTourGuest();
                guest.CheckPointId = currentCheckPoint.Id; 
                tourGuestRepository.Update(guest);
            }
            MessageBox.Show("Tourist marked as present!");
        }

        private void NextCheckPointClick(object sender, RoutedEventArgs e)
        {
            if(currentCheckPointIndex+1<toursCheckPoints.Count) 
            {
                currentCheckPointIndex++;
                UpdateUI();
                LoadTourists();
            }
            else if(currentCheckPointIndex + 1 == toursCheckPoints.Count && String.Equals(currentCheckPoint.Type,"END"))
            { 
                FinishingTour();
            }

        }
         private void FinishingTour()
         {
                TourStartDate tourStart = selectedStartDate.ToTourStartDate();
                tourStart.HasFinished = true;
                tourStartDateRepository.Update(tourStart);
                MessageBox.Show("Tour has ended");
                Close();
            
         }
         private void UpdateUI()
         {
            if(toursCheckPoints!=null && toursCheckPoints.Count > currentCheckPointIndex)
            {
                currentCheckPoint = toursCheckPoints[currentCheckPointIndex];
                CheckPointName.Text = currentCheckPoint.Name;
            }
         }

        private void EndTourClick(object sender, RoutedEventArgs e)
        {
            FinishingTour();
        }
    }
}
