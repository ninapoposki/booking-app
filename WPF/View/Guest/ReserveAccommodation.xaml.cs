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
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Guest
{
    /// <summary>
    /// Interaction logic for ReserveAccommodation.xaml
    /// </summary>
    public partial class ReserveAccommodation : Page
    {

        public ReserveAccommodationVM ReserveAccommodationVM { get; set; }

        public ReserveAccommodation(NavigationService navigationService, AccommodationDTO selectedAccommodationDTO,GuestDTO guestDTO)
        {
            InitializeComponent();
            ReserveAccommodationVM = new ReserveAccommodationVM(AvailableDatesFrame.NavigationService, selectedAccommodationDTO,guestDTO);
            DataContext = ReserveAccommodationVM;
        }

    }
}
