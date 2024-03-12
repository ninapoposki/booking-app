using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
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

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for AddAccommodation.xaml
    /// </summary>
    public partial class AddAccommodation : Window, INotifyPropertyChanged
    {
        public AccommodationDTO Accommodation { get; set; }
        public AccommodationRepository accommodationRepository { get; set; }
        public AddAccommodation(AccommodationRepository accommodationRepository)
        {
            InitializeComponent();
            DataContext = this;
            Accommodation = new AccommodationDTO();
            this.accommodationRepository = accommodationRepository;

        }


        // Metoda koja se poziva kada se pritisne dugme "Add"
        private void AddAccommodationButton_Click(object sender, RoutedEventArgs e)
        {
            // Implementacija koda za dodavanje smještaja



            if (Accommodation.IsValid)
            {
                MessageBox.Show("Dodavanje smeštaja");
                this.DialogResult = true;
                accommodationRepository.Add(Accommodation.ToAccommodation());
                Close();
            }
            else
            {
                MessageBox.Show("Accommodation cannot be created. All fields are not valid.");
            }
        }

        // Metoda koja se poziva kada se pritisne dugme "Cancel"
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Implementacija koda za otkazivanje dodavanja smještaja
            MessageBox.Show("Otkazivanje dodavanja smeštaja");
            this.DialogResult = false;
            this.Close();
        }

        private void AddImageButton_Click(object sender, RoutedEventArgs e)
        {
            AddAccommodationImage addAccommodationImageWindow = new AddAccommodationImage();
            addAccommodationImageWindow.ShowDialog();
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
