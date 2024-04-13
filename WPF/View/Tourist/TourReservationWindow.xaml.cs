using BookingApp.DTO;
using BookingApp.Domain.Model;
using BookingApp.Observer;
using BookingApp.Repository;
using BookingApp.WPF.View.Tourist;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using BookingApp.WPF.ViewModel.Tourist;
namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for TourReservation.xaml
    /// </summary>
    public partial class TourReservationWindow : Window
    {
        public TourReservationWindowVM tourReservationWindowVM { get; set; }
      /*  public TourReservationWindow(TourDTO selectedTour)
        {
            InitializeComponent();
            tourReservationWindowVM= new TourReservationWindowVM(selectedTour);
            DataContext = tourReservationWindowVM;
        }*/
        public TourReservationWindow(TourDTO selectedTour, string username)
        {
            InitializeComponent();
            tourReservationWindowVM = new TourReservationWindowVM(selectedTour, username);  
            DataContext = tourReservationWindowVM;
        }

    
        private void CancelTour(object sender, RoutedEventArgs e)
        {


            this.Close();
        }


        private void Vouchers(object sender, RoutedEventArgs e)
        {

            VouchersWindow vouchersWindow = new VouchersWindow();
            vouchersWindow.Show();

        }
        private void ConfirmTourReservation(object sender, RoutedEventArgs e)
        {
            tourReservationWindowVM.ConfirmTourReservation();
        }

    }
  

}
