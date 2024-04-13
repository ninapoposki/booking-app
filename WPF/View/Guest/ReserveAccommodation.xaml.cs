using BookingApp.DTO;
using BookingApp.Repository;
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
using BookingApp.Domain.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using BookingApp.WPF.ViewModel.Guest;

namespace BookingApp.WPF.View.Guest
{
    /// <summary>
    /// Interaction logic for ReserveAccommodation.xaml
    /// </summary>
    public partial class ReserveAccommodation : Window
    {

        public ReserveAccommodationVM ReserveAccommodationVM { get; set; }

        public ReserveAccommodation(AccommodationDTO selectedAccommodationDTO)
        {
            InitializeComponent();
            ReserveAccommodationVM = new ReserveAccommodationVM(selectedAccommodationDTO);
            DataContext = ReserveAccommodationVM;
            //PROVERI DA LI OVO SME!!!
            ReserveAccommodationVM.RequestClose += (sender, args) =>
            {
                Close(); 
            };
        }

        private void TryToBookClick(object sender, RoutedEventArgs e)
        {
            ReserveAccommodationVM.TryToBookAccommodation();
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close(); 
        }

 

    }
}
