using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.WPF.ViewModel.Guest;
using BookingApp.WPF.ViewModel.Guide;
using BookingApp.WPF.ViewModel.Owner;
using Microsoft.Win32;
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
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Guest
{
    /// <summary>
    /// Interaction logic for GradeAccommodation.xaml
    /// </summary>
    public partial class GradeAccommodation : Page
    {
        public GradeAccommodationVM GradeAccommodationVM { get; set; }
        public GradeAccommodation(NavigationService navigationService,AccommodationReservationDTO accommodationReservationDTO)
        {
            InitializeComponent();
            GradeAccommodationVM = new GradeAccommodationVM(navigationService,accommodationReservationDTO);
            DataContext = GradeAccommodationVM;
        }
    }
}
