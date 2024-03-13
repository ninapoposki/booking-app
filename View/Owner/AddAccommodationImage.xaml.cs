using BookingApp.Model;
using BookingApp.Observer;
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
    /// Interaction logic for AddAccommodationImage.xaml
    /// </summary>
    public partial class AddAccommodationImage : Window, IObserver
    {
        public AddAccommodationImage()
        {
            InitializeComponent();
            DataContext = this;
            //  Image = new ImageDTO();

        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBox.Show("Dodavanje URL-a slike");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBox.Show("Otkazivanje dodavanja URL-a slike");
            this.DialogResult = false;
            this.Close();
        }
    }
}