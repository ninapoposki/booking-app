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
using BookingApp.WPF.ViewModel.Guide;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for TouristMainWindow.xaml
    /// </summary>
    public partial class TouristMainWindow : Window
    {
        public TouristMainWindowVM  TouristMainWinodowVM { get; set; }
        
       

        public TouristMainWindow(string username)
        {
            InitializeComponent();
           TouristMainWinodowVM= new TouristMainWindowVM(username);
            DataContext = TouristMainWinodowVM;

           
        }

      
        private void CancelTour(object sender, RoutedEventArgs e)
        {

            this.Close();
        }
        

        private void SearchTour(object sender, RoutedEventArgs e)
        {
            TouristMainWinodowVM.SearchTour();
        }

       
        private void BookTour(object sender, RoutedEventArgs e)
        {
           TouristMainWinodowVM.BookTour();
        }


        private void FinishedTourClick(object sender, RoutedEventArgs e)
        {
            TouristMainWinodowVM.FinishedTourClick();
        }

        private void ActiveTourClick(object sender, RoutedEventArgs e)
        {
            TouristMainWinodowVM.ActiveTourClick();
        }

        private void NotificationsClick(object sender, RoutedEventArgs e)
        {
            TouristMainWinodowVM.NotificationsClick();
        }
        private void CityChanged(object sender, SelectionChangedEventArgs e)
        {
            TouristMainWinodowVM.CityChanged();
        }

        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
