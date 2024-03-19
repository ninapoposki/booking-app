using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Windows;
using System.Windows.Controls;

namespace BookingApp.View.Guest
{
    public partial class AvailableDatesWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Range> Dates { get; set; }
        public AccommodationReservationDTO selectedReservation=new AccommodationReservationDTO();
        public AccommodationReservationRepository accommodationReservationRepository = new AccommodationReservationRepository();

        private Range _selectedDate=new Range();

        public Range selectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }

        public AvailableDatesWindow(List<(DateTime, DateTime)> dates, AccommodationReservationDTO accommodationReservation)
        {
            InitializeComponent();
            this.DataContext = this;
            selectedReservation = new AccommodationReservationDTO();
            selectedReservation = accommodationReservation;
            Dates = new ObservableCollection<Range>(dates.Select(r => new Range { InitialDate = r.Item1, EndDate = r.Item2 }).ToList());
          
            CheckSixMonthsAvailability(Dates);

        }
        private void CheckSixMonthsAvailability(IEnumerable<Range> dates)
        {
            bool anyAvailable = dates.Any(date => date.InitialDate <= DateTime.Now.AddMonths(6));

            if (!anyAvailable)
            {
                MessageBox.Show("There are no available dates for the next 6 months.");
                this.Close();
            }
            else
            {
                selectedDate = new Range { InitialDate = dates.First().InitialDate, EndDate = dates.First().EndDate };
            }
        }

        public class Range
        {
            public DateTime InitialDate { get; set; }
            public DateTime EndDate { get; set; }
        }

        private void BookAccommodationClick(object sender, RoutedEventArgs e)
        {
            
             selectedReservation.InitialDate = selectedDate.InitialDate;
             selectedReservation.EndDate = selectedDate.EndDate; 
                    
             accommodationReservationRepository.Add(selectedReservation.ToAccommodationReservation());
             MessageBox.Show("Reservation added successfully");
             this.Close();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
