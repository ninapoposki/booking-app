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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.View.Guide
{
    /// <summary>
    /// Interaction logic for TourReviewsUserControl.xaml
    /// </summary>
    public partial class TourReviewsUserControl : UserControl
    {
        public TourReviewsUserControl(TourDTO tourDTO)
        {
            InitializeComponent();
            DataContext = new TourReviewsUserControlVM(tourDTO);
        }
    }
}
