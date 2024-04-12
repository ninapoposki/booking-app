using BookingApp.DTO;
using BookingApp.Domain.Model;
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
using System.ComponentModel;
using BookingApp.WPF.ViewModel;
using System.Runtime.CompilerServices;
using BookingApp.WPF.ViewModel.Guide;

namespace BookingApp.WPF.View.Guide
{
    /// <summary>
    /// Interaction logic for TourCheckPoints.xaml
    /// </summary>
    public partial class TourCheckPoints : Window
    {
        private TourCheckPointsVM tourCheckPointsVM;
        
        public TourCheckPoints(TourStartDateDTO selectedStartDate)
        {
            InitializeComponent();
            tourCheckPointsVM=new TourCheckPointsVM(selectedStartDate);
            DataContext = tourCheckPointsVM;
        }
        private void MarkAsPresentClick(object sender, RoutedEventArgs e)
        {
            tourCheckPointsVM.MarkAsPresentClick();
        }

        private void NextCheckPointClick(object sender, RoutedEventArgs e)
        {
            tourCheckPointsVM.NextCheckPointClick();
        }
         private void FinishingTour()
         {
              tourCheckPointsVM.FinishingTour();
              Close();
            
         }

        private void EndTourClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tour has ended");
            FinishingTour();
        }

        private void TouristListSelectionChaged(object sender, SelectionChangedEventArgs e)
        {
            tourCheckPointsVM.TouristListSelectionChaged(sender,e);
        }
    }
}
