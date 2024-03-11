using BookingApp.Observer;
using System;
using BookingApp.Repository;
using BookingApp.Model;
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

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for TouristMainWindow.xaml
    /// </summary>
    public partial class TouristMainWindow : Window,IObserver
    {

        private readonly TourRepository tourRepository;
        public ObservableCollection<Tour> AllTours { get; set; }

        //public TourDTO SelectedTour {get;set} 
        public TouristMainWindow()
        {
            InitializeComponent();
            DataContext = this;
            tourRepository= new TourRepository();
            AllTours = new ObservableCollection<Tour>(tourRepository.GetAll()); //tourDTO
            ToursDataGrid.ItemsSource = AllTours; //Ovo povezuje vašu kolekciju tura (AllTours) sa DataGrid-om na vašem prozoru, omogućavajući prikaz tura.

        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        private void ButtonCancel(object sender, RoutedEventArgs e)
        {


            this.Close();
        }


        private void SearchClick(object sender, RoutedEventArgs e)
        {

            bool parsePeopleSuccess = int.TryParse(PeopleTextBox.Text, out int peopleCountParsed);
            bool parseDurationSuccess = double.TryParse(DaysTextBox.Text, out double durationParsed);

            string selectedLanguage = (LanguageComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            var filteredTours = AllTours.Where(tour =>
                (string.IsNullOrWhiteSpace(CityTextBox.Text) || tour.Location.City.Equals(CityTextBox.Text, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(CountryTextBox.Text) || tour.Location.Country.Equals(CountryTextBox.Text, StringComparison.OrdinalIgnoreCase)) &&
                (!parseDurationSuccess || Math.Abs(tour.Duration - durationParsed) < 0.01) && // Usporedba double vrijednosti sa dozvoljenom razlikom
                (selectedLanguage == null || tour.Language.Name.Equals(selectedLanguage, StringComparison.OrdinalIgnoreCase)) &&
                // Provjera da li je uneseni broj ljudi manji ili jednak kapacitetu ture, ako je uspješno parsiran
                (!parsePeopleSuccess || (tour.Capacity >= peopleCountParsed && peopleCountParsed > 0))
            ).ToList();

            ToursDataGrid.ItemsSource = filteredTours;


        }

        private void BookTourButton(object sender, RoutedEventArgs e)
        {
           /* if (SelectedTour == null)
            {


                MessageBox.Show("Molimo Vas da odaberete turu koju želite da rezervižšete.");

                return;
            }*/
            TourReservation tourReservation=new TourReservation();
            tourReservation.Show();
        }
    }
}
