using BookingApp.DTO;
using BookingApp.WPF.View.Tourist;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class AvailableTourWindowVM:ViewModelBase
    {
        public TourDTO SelectedTour { get; set; }

        public ObservableCollection<TourDTO> AvailableTours { get; set; }
        private string username;
        public AvailableTourWindowVM(List<TourDTO> availableTours, string username)
        {

            
            AvailableTours = new ObservableCollection<TourDTO>(availableTours);
            SelectedTour = new TourDTO();
            this.username = username;
            this.username = username;
        }

        public void BookTour()
        {

            if (SelectedTour == null)
            {


                MessageBox.Show("Molimo Vas da odaberete turu koju želite da rezervižšete.");

                return;
            }
            else
            {

                TourReservationWindow tourReservationWindow = new TourReservationWindow(SelectedTour, username);
                tourReservationWindow.Show();

            }


        }
    }
}
