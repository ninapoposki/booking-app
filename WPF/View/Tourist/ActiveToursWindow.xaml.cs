﻿using BookingApp.WPF.ViewModel.Tourist;
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

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for ActiveToursWindow.xaml
    /// </summary>
    public partial class ActiveToursWindow : Page
    {

        public ActiveToursWindowVM ActiveToursWindowVM {  get; set; }
        public ActiveToursWindow(NavigationService navigationService,int userId)
        {
         
            InitializeComponent();
            DataContext = new ActiveToursWindowVM(navigationService,userId);
        }

     
    }
}
