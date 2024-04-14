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
    /// Interaction logic for TourStatistics.xaml
    /// </summary>
    public partial class TourStatistics : Window
    {
        public TourStatisticsVM TourStatisticsVM { get; set; }
        public TourStatistics(int userId)
        {
            InitializeComponent();
            TourStatisticsVM=new TourStatisticsVM(userId);
            DataContext = TourStatisticsVM;
        }

        private void ItemClicked(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            var tour = border.DataContext as TourDTO;
            TourStatisticsVM.SelectedTour = tour;
            TourStatisticsVM.LoadStatistics(tour);
        }

        private void YearChanged(object sender, SelectionChangedEventArgs e)
        {
            TourStatisticsVM.YearChanged();
        }
    }
}
