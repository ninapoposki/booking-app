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
    /// Interaction logic for FinishedTour.xaml
    /// </summary>
    public partial class FinishedTour : Window
    {
        private int userId;
        public FinishedTour(int userId)
        {
            InitializeComponent();
            DataContext = this;
            this.userId = userId;
        }

        private void SeeTourStatisticsClick(object sender, RoutedEventArgs e)
        {
            TourStatistics tourStatistics=new TourStatistics(userId);
            tourStatistics.Owner = this;
            tourStatistics.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            tourStatistics.ShowDialog();
        }
        private void SeeTourReviewsClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
