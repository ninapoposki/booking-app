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
using System.Windows.Navigation;


namespace BookingApp.WPF.View.Guest
{
    public partial class AvailableDatesWindow : Page
    {
        public AvailableDatesWindowVM AvailableDatesWindowVM { get; set; }

        public AvailableDatesWindow(NavigationService navigationService,List<(DateTime, DateTime)> dates, AccommodationReservationDTO accommodationReservation)
        {
            InitializeComponent();
            AvailableDatesWindowVM = new AvailableDatesWindowVM(navigationService,dates,accommodationReservation);
            DataContext = AvailableDatesWindowVM;
        }

    }
}
