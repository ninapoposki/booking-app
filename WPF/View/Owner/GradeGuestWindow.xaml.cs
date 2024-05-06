using BookingApp.DTO;
using BookingApp.Domain.Model;
using BookingApp.Repository;
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
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for GradeGuestWindow.xaml
    /// </summary>

    
    public partial class GradeGuestWindow : Window
    {
       
        public GradeGuestVM GradeGuestVM {  get; set; }
        public GradeGuestWindow(AccommodationReservationDTO accommodationReservationDTO)
        {
            InitializeComponent();
            
           
            GradeGuestVM = new GradeGuestVM(accommodationReservationDTO);
            DataContext = GradeGuestVM;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }

    }
}
