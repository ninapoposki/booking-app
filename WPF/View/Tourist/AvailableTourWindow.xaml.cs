using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.WPF.ViewModel.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for AvailableTourWindow.xaml
    /// </summary>
    public partial class AvailableTourWindow : Window
    {
      public AvailableTourWindowVM availableTourWindowVM { get; set; }
        public AvailableTourWindow(List<TourDTO> availableTours, string username)
        {

            InitializeComponent();
            availableTourWindowVM=new AvailableTourWindowVM(availableTours,username);
            DataContext = availableTourWindowVM;
           
        }

        private void BookTour(object sender, RoutedEventArgs e)
        {

            availableTourWindowVM.BookTour();
        }

        private void CancelTour(object sender, RoutedEventArgs e)
        {

            Close();
        }
    }
}
