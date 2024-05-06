using BookingApp.WPF.ViewModel.Guest;
using BookingApp.WPF.ViewModel.Owner;
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

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for OwnersAccommodation.xaml
    /// </summary>
    public partial class OwnersAccommodation : Page
    {
        public OwnersAccommodationVM OwnersAccommodationVM { get; set; }
        public OwnersAccommodation(NavigationService navigation, int loggedInUserId)
        {
            InitializeComponent();
            OwnersAccommodationVM = new OwnersAccommodationVM(navigation, loggedInUserId);
            DataContext = OwnersAccommodationVM;
        }
       /* private void SearchClick(object sender, RoutedEventArgs e)
        {
            OwnersAccommodationVM.SearchClick();
        }*/
        /* private void AddAccommodationClick(object sender, RoutedEventArgs e)
         {
             OwnersAccommodationVM.AddAccommodationClick();
         }*/
    }
}
