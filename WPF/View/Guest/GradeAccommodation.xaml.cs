using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.WPF.ViewModel.Guest;
using BookingApp.WPF.ViewModel.Guide;
using BookingApp.WPF.ViewModel.Owner;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace BookingApp.WPF.View.Guest
{
    /// <summary>
    /// Interaction logic for GradeAccommodation.xaml
    /// </summary>
    public partial class GradeAccommodation : Window
    {
        public GradeAccommodationVM GradeAccommodationVM { get; set; }
        public GradeAccommodation(AccommodationReservationDTO accommodationReservationDTO)
        {
            InitializeComponent();
            GradeAccommodationVM = new GradeAccommodationVM(accommodationReservationDTO);
            DataContext = GradeAccommodationVM;
        }

        private int GetSelectedRadioButtonValue(StackPanel panel)
        {

            foreach (var radioButton in panel.Children)
            {
                if (radioButton is RadioButton && ((RadioButton)radioButton).IsChecked == true)
                {
                    return int.Parse(((RadioButton)radioButton).Content.ToString());
                }
            }
            return 0;
        }


        private void ConfirmButtonClick(object sender, RoutedEventArgs e)
        {
            int cleanness = GetSelectedRadioButtonValue(Cleanliness);
            int followingRules = GetSelectedRadioButtonValue(Correctness);
            string comment = CommentsTextBox.Text;
            GradeAccommodationVM.ConfirmButtonClick(cleanness, followingRules);
            MessageBox.Show("You graded reservation successfully!");
            Close();
        }
        private void BrowseImageClick(object sender, RoutedEventArgs e)
        {
            GradeAccommodationVM.BrowseImageClick();
        }
        private void RemoveImageClick(object sender, RoutedEventArgs e)
        {
            GradeAccommodationVM.RemoveImageClick();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
