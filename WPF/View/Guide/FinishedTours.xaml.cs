using BookingApp.DTO;
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
    /// Interaction logic for FinishedTours.xaml
    /// </summary>
    public partial class FinishedTours : Window
    {
        public FinishedToursVM FinishedToursVM { get; set; }
        public FinishedTours(int userId)
        {
            FinishedToursVM=new FinishedToursVM(userId);
            InitializeComponent();
            DataContext = FinishedToursVM;
        }

        private void SeeReviewsClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var tour = button.DataContext as TourDTO;
            TourReviews tourReviews = new TourReviews(tour);
            tourReviews.Owner = this;
            tourReviews.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            tourReviews.ShowDialog();
        }
    }
}
