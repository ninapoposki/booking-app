using BookingApp.WPF.ViewModel.Tourist;
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

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for TourRequestViewWindow.xaml
    /// </summary>
    public partial class TourRequestViewWindow : Page
    {
       
        public TourRequestViewWindow(NavigationService navigationService)
        {
            InitializeComponent();
            DataContext = new TourRequestViewWindowVM(navigationService);
        }

        public void OnYearSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as TourRequestViewWindowVM;
            if (viewModel == null) return;
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedItem is ComboBoxItem item && item.Content.ToString() != "All Time")
            {
                int year = int.Parse(item.Content.ToString());
                viewModel.CalculateStatistics(year);
                viewModel.CalculateAverageNumberOfTourists(year);}
            else{
                viewModel.CalculateStatistics();
                viewModel.CalculateAverageNumberOfTourists();
            }
        }
    }
}
