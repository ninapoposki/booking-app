using BookingApp.Observer;
using System;
using BookingApp.Repository;
using BookingApp.Domain.Model;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Collections.ObjectModel;
using BookingApp.DTO;
using System.Runtime.CompilerServices;
using BookingApp.WPF.ViewModel.Tourist;
using BookingApp.Services;
using BookingApp.WPF.ViewModel.Guide;
using System.Windows.Navigation;

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for TouristMainWindow.xaml
    /// </summary>
    public partial class TouristMainWindow : Page
    {
        public TouristMainWindowVM  TouristMainWinodowVM { get; set; }
        
        public TouristMainWindow(NavigationService navigationService, string username)
        {
            InitializeComponent();
            TouristMainWindowVM viewModel = new TouristMainWindowVM(navigationService, username);
            DataContext = viewModel;
            TouristMainWinodowVM = viewModel;
          
        }
       private void CityChanged(object sender, SelectionChangedEventArgs e)
        {
            TouristMainWindowVM viewModel = DataContext as TouristMainWindowVM; 
            if (viewModel != null)
            {
                viewModel.CityChanged();
            }
            else
            {
                MessageBox.Show("ViewModel nije inicijalizovan!");
            }
        }
        private void LanguageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
