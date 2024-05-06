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
    /// Interaction logic for DateChangeRequests.xaml
    /// </summary>
    public partial class DateChangeRequests : Page
    {
        public DateChangeRequestsVM DateChangeRequestsVM;
        public DateChangeRequests(NavigationService navigation, int loggedInUserId)
        {
            InitializeComponent();
            DateChangeRequestsVM = new DateChangeRequestsVM(navigation,commentTextBox, loggedInUserId);
            DataContext = DateChangeRequestsVM;
        }

        private void DeclineButtonClick(object sender, RoutedEventArgs e)
        {
            DateChangeRequestsVM.DeclineButtonClick();
        }
        private void AcceptButtonClick(object sender, RoutedEventArgs e)
        {

          DateChangeRequestsVM.AcceptButtonClick();
        }
    }
}
