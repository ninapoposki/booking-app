﻿using BookingApp.DTO;
using BookingApp.WPF.ViewModel.Guide;
using System;
using System.Collections.Generic;
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

namespace BookingApp.WPF.View.Guide
{
    /// <summary>
    /// Interaction logic for TourReviews.xaml
    /// </summary>
    public partial class TourReviews : Window
    {

        public TourReviews(TourDTO tour)
        {
            InitializeComponent();
            DataContext = new TourReviewsUserControlVM(tour);
        }

        private void ReportReviewClick(object sender, RoutedEventArgs e)
        {
         var button = sender as Button;
         var tourGrade = button.DataContext as TourGradeDTO;
         //TourReviewsVM.SelectedReview = tourGrade;
         //TourReviewsVM.ReportReviewClick();
        }
        
    }
}
