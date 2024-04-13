using BookingApp.DTO;
using BookingApp.Domain.Model;
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
using BookingApp.WPF.ViewModel.Guest;

namespace BookingApp.WPF.View.Guest
{
    public partial class AvailableDatesWindow : Window
    {
        public AvailableDatesWindowVM AvailableDatesWindowVM { get; set; }

        public AvailableDatesWindow(List<(DateTime, DateTime)> dates, AccommodationReservationDTO accommodationReservation)
        {
            InitializeComponent();
            AvailableDatesWindowVM = new AvailableDatesWindowVM(dates,accommodationReservation);
            DataContext = AvailableDatesWindowVM;
            AvailableDatesWindowVM.RequestClose += (sender, args) =>
            {
                Close(); 
            };
        }

        private void BookAccommodationClick(object sender, RoutedEventArgs e)
        {
            AvailableDatesWindowVM.BookAccommodationClick();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
