﻿using BookingApp.Observer;
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

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for TourReservation.xaml
    /// </summary>
    public partial class TourReservation : Window, IObserver
    {
        public TourReservation()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        private void ButtonCancel(object sender, RoutedEventArgs e)
        {


            this.Close();
        }

        private void ConfirmTourReservation(object sender, RoutedEventArgs e)
        {


        }
    }
}
