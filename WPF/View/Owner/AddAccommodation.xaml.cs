using BookingApp.DTO;
using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.WPF.ViewModel.Owner;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
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
using static System.Net.Mime.MediaTypeNames;
using BookingApp.WPF.ViewModel.Guide;
using BookingApp.Domain.IRepositories;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for AddAccommodation.xaml
    /// </summary>
    public partial class AddAccommodation : Window
    {
        private ImageRepository imageRepository { get; set; }
        public AccommodationRepository accommodationRepository { get; set; }
        public AccommodationDTO Accommodation { get; set; }
        private ImageDTO SelectedImage;
        public ObservableCollection<ImageDTO> Images { get; set; }
        private LocationRepository locationRepository { get; set; }
        private User currentUser;
        private UserRepository userRepository { get; set; }
        

        public AddAccommodationVM AddAccommodationVM { get; set; }
        public AddAccommodation( string currentUserUsername)
        {
            InitializeComponent();
            AddAccommodationVM = new AddAccommodationVM(currentUserUsername);
            DataContext = AddAccommodationVM;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            
           
        }

        
        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Otkazivanje dodavanja smeštaja");
             this.DialogResult = false;
             this.Close();
        }
        private void BrowseImageClick(object sender, RoutedEventArgs e)
        {


            AddAccommodationVM.BrowseImageClick();
        }

        private void AddAccommodationButtonClick(object sender, RoutedEventArgs e)
        {

            AddAccommodationVM.AddAccommodationButtonClick();
            Close();


        }
        private void CountryChanged(object sender, SelectionChangedEventArgs e)
        {
           AddAccommodationVM.CountryChanged();
        }

        private void CityChanged(object sender, SelectionChangedEventArgs e)
        {
            AddAccommodationVM.SelectedCity = (string)cityComboBox.SelectedItem;
        }
    }
    
}
