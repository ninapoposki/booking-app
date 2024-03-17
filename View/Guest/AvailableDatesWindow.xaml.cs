using BookingApp.DTO;
using BookingApp.Model;
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

namespace BookingApp.View.Guest
{
    /// <summary>
    /// Interaction logic for AvailableDatesWindow.xaml
    /// </summary>
    public partial class AvailableDatesWindow : Window, INotifyPropertyChanged
    {

        public ObservableCollection<Range> Dates { get; set; }
       // public AccommodationReservation accommodationReservation;
       // public AccommodationReservationDTO accommodationReservationDTO;
        public AvailableDatesWindow(List<(DateTime,DateTime)> dates)
        {
            InitializeComponent();
            this.DataContext = this;
            //accommodationReservationDTO=new AccommodationReservationDTO();
            //accommodationReservation = accommodationReservationDTO.ToAccommodationReservation();

            

            Dates = new ObservableCollection<Range>(dates.Select(r => new Range {InitialDate = r.Item1, EndDate= r.Item2 }).ToList());
        }
        

        public class Range
        {
            public DateTime InitialDate { get; set; }
            public DateTime EndDate { get; set; }
        }
        private void BookAccommodationButton(object sender, RoutedEventArgs e)
        {
            

        }
        private void CancelButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
