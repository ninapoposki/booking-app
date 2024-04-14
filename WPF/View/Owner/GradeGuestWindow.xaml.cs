using BookingApp.DTO;
using BookingApp.Domain.Model;
using BookingApp.Repository;
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
using BookingApp.WPF.ViewModel.Owner;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for GradeGuestWindow.xaml
    /// </summary>

    
    public partial class GradeGuestWindow : Window
    {
       
        public GradeGuestVM GradeGuestVM {  get; set; }
        public GradeGuestWindow(AccommodationReservationDTO accommodationReservationDTO)
        {
            InitializeComponent();
            
           
            GradeGuestVM = new GradeGuestVM(accommodationReservationDTO);
            DataContext = GradeGuestVM;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

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
             int cleanness = GetSelectedRadioButtonValue(Cleanness);
             int followingRules = GetSelectedRadioButtonValue(FollowingTheRules);
             string comment = CommentsTextBox.Text;
            // GradeGuestVM.CleannessRadio = cleanness;
            // GradeGuestVM.FollowingTheRulesRadio = followingRules;
            GradeGuestVM.ConfirmButtonClick(cleanness, followingRules);
            Close();

        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
