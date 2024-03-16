using BookingApp.DTO;
using BookingApp.Model;
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

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for GradeGuestWindow.xaml
    /// </summary>

    
    public partial class GradeGuestWindow : Window
    {
        public GuestGradeRepository guestGradeRepository { get; set; }
        public AccommodationReservationDTO selectedAccommodationReservation;
        public GuestGradeDTO guestGradeDTO { get; set; }
        public GradeGuestWindow(GuestGradeRepository guestGradeRepository,AccommodationReservationDTO accommodationReservationDTO)
        {
            InitializeComponent();
            selectedAccommodationReservation = accommodationReservationDTO;
            DataContext = selectedAccommodationReservation;

            this.guestGradeRepository = new GuestGradeRepository();
            guestGradeDTO = new GuestGradeDTO();
        }

        private int GetSelectedRadioButtonValue(StackPanel panel)
        {
            // Pronalazak izabranog radio dugmeta i preuzimanje vrednosti
            foreach (var radioButton in panel.Children)
            {
                if (radioButton is RadioButton && ((RadioButton)radioButton).IsChecked == true)
                {
                    return int.Parse(((RadioButton)radioButton).Content.ToString());
                }
            }
            return 0; // Ukoliko nijedno dugme nije izabrano
        }


        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            int cleanness = GetSelectedRadioButtonValue(Cleanness);
            int followingRules = GetSelectedRadioButtonValue(FollowingTheRules);

            // Preuzimanje vrednosti komentara
            string comment = CommentsTextBox.Text;

            guestGradeDTO.Cleanless = cleanness;
            guestGradeDTO.RulesFollowing = followingRules;
            guestGradeDTO.Comment = comment;

            guestGradeDTO.GuestId = selectedAccommodationReservation.GuestId;
            guestGradeDTO.ReservationId = selectedAccommodationReservation.Id;
            guestGradeRepository.Add(guestGradeDTO.ToGuestGrade());
            this.DialogResult = true;
            Close();

        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
