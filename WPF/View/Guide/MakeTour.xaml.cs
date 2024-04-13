using BookingApp.DTO;
using BookingApp.Domain.Model;
using BookingApp.Repository;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xaml.Schema;
using static System.Net.Mime.MediaTypeNames;
using BookingApp.WPF.ViewModel.Guide;

namespace BookingApp.WPF.View.Guide
{
    /// <summary>
    /// Interaction logic for MakeTour.xaml
    /// </summary>
    public partial class MakeTour : Window
    {
        public MakeTourVM MakeTourVM { get; set; }
        public MakeTour()
        {
            InitializeComponent();
            MakeTourVM = new MakeTourVM();
            DataContext = MakeTourVM;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
 
        private void CityChanged(object sender, SelectionChangedEventArgs e)
        {
            MakeTourVM.CityChanged();
        }
        private void AddClick(object sender, RoutedEventArgs e)
        {
            MakeTourVM.AddClick();
        }

        //kljucne tacke
        private void AddCheckPointClick(object sender, RoutedEventArgs e)
        {
            MakeTourVM.AddCheckPointClick();
        }
        private void RemoveCheckPointClick(object sender, RoutedEventArgs e)
        {
           MakeTourVM.RemoveCheckPointClick();
        }

    
        //Datumi
        private void AddDateClick(object sender, RoutedEventArgs e)
        {
            MakeTourVM.AddDate();
        }
        private void RemoveDate(object sender, RoutedEventArgs e)
        {
            MakeTourVM.RemoveDate(); 
        }

        private void BrowseImageClick(object sender, RoutedEventArgs e)
        {
           MakeTourVM.BrowseImageClick();
        }
        private void RemoveImageClick(object sender, RoutedEventArgs e)
        {
            MakeTourVM.RemoveImageClick();
        }

        private void LiveTourClick(object sender, RoutedEventArgs e)
        {
            LiveTour liveTour = new LiveTour();
            liveTour.Owner = this;
            liveTour.WindowStartupLocation= WindowStartupLocation.CenterOwner;
            liveTour.ShowDialog();
        }
        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void UpcomingToursClick(object sender, RoutedEventArgs e)
        {
            UpcomigTours upcomingTours = new UpcomigTours();
            upcomingTours.Owner = this;
            upcomingTours.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            upcomingTours.ShowDialog();
        }
    }
}
