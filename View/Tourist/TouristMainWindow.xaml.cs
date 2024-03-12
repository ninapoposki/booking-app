﻿using BookingApp.Observer;
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
using BookingApp.DTO;

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for TouristMainWindow.xaml
    /// </summary>
    public partial class TouristMainWindow : Window,IObserver
    {


        private TourRepository tourRepository; 
        public List<Tour> AllTours { get; set; }

        private readonly TourRepository tourRepository;
        private readonly LocationRepository locationRepository;
        private readonly LanguageRepository languageRepository;
        public ObservableCollection<TourDTO> AllTours { get; set; }
        public ObservableCollection<LanguageDTO> Languages { get; set; }

        public TourDTO SelectedTour { get; set; } 

        public TouristMainWindow()
        {
            InitializeComponent();
            DataContext = this;
            tourRepository= new TourRepository();
            locationRepository = new LocationRepository(); 
            languageRepository = new LanguageRepository();
            AllTours = new ObservableCollection<TourDTO>();
            Languages = new ObservableCollection<LanguageDTO>();
            
          
           
            Update();

        }

        public void Update()
        {
            AllTours.Clear();
            foreach(Tour tour in tourRepository.GetAll())
            {
                AllTours.Add(new TourDTO(tour, locationRepository, languageRepository));
                
            }
            Languages.Clear();


            foreach (Language language in languageRepository.GetAll())
            {
                Languages.Add(new LanguageDTO(language));
            }



        }

        private void SearchClick(object sender, RoutedEventArgs e)
        {

        }

        private void BookTourClick(object sender, RoutedEventArgs e)
        {


        }

        private void CancelClick(object sender, RoutedEventArgs e)
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
           if (SelectedTour == null)
            {


                MessageBox.Show("Molimo Vas da odaberete turu koju želite da rezervižšete.");

                return;
            }
            TourReservation tourReservation=new TourReservation(SelectedTour);
            tourReservation.Show();
        }
    }
}
