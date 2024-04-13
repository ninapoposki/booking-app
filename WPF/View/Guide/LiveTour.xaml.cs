using BookingApp.DTO;
using BookingApp.Domain.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using BookingApp.WPF.ViewModel.Guide;

namespace BookingApp.WPF.View.Guide
{
    /// <summary>
    /// Interaction logic for LiveTour.xaml
    /// </summary>
    public partial class LiveTour : Window
    {
        
       public LiveTourVM LiveTourVM { get; set; }
        public LiveTour()
        {
            InitializeComponent();
            LiveTourVM = new LiveTourVM();
            DataContext = LiveTourVM;
        }
 
        private void StartTourClick(object sender, RoutedEventArgs e)
        {
            LiveTourVM.StartTourClick();
           
        }
    }
}

