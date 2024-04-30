using BookingApp.WPF.ViewModel.Guide;
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

namespace BookingApp.WPF.View.Guide
{
    /// <summary>
    /// Interaction logic for GuideMainWindow.xaml
    /// </summary>
    public partial class GuideMainWindow : Window
    {
        public GuideMainWindowVM GuideMainWindowVM { get; set; }
        public GuideMainWindow(int userId)
        {
            InitializeComponent();
            DataContext = new GuideMainWindowVM(MainWindowFrame.NavigationService,userId);
        }

        private void MyToursPageClick(object sender, RoutedEventArgs e)
        {

        }

        private void TourRequestsPageClick(object sender, RoutedEventArgs e)
        {

        }

        private void ProfilePageClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
