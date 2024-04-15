using BookingApp.Observer;
using System;
using BookingApp.Repository;
using BookingApp.Domain.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using BookingApp.DTO;
using System.Runtime.CompilerServices;
using BookingApp.WPF.ViewModel.Tourist;
using BookingApp.Services;

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for TouristMainWindow.xaml
    /// </summary>
    public partial class TouristMainWindow : Window
    {
        public TouristMainWindowVM  touristMainWinodowVM { get; set; }
        
       

        public TouristMainWindow(string username)
        {
            InitializeComponent();
            touristMainWinodowVM= new TouristMainWindowVM(username);
            DataContext = touristMainWinodowVM;

           
        }

      
        private void CancelTour(object sender, RoutedEventArgs e)
        {

            this.Close();
        }


        private void SearchTour(object sender, RoutedEventArgs e)
        {
            touristMainWinodowVM.SearchTour();
        }

       
        private void BookTour(object sender, RoutedEventArgs e)
        {
           touristMainWinodowVM.BookTour();
        }

        private void RateTour(object sender, RoutedEventArgs e)
        {
           touristMainWinodowVM.RateTour();
        }

    }
}
