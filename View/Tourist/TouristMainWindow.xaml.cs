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

namespace BookingApp.View.Tourist
{
    /// <summary>
    /// Interaction logic for TouristMainWindow.xaml
    /// </summary>
    public partial class TouristMainWindow : Window,IObserver
    {

        private TourRepository tourRepository; 
        public List<Tour> AllTours { get; set; }
        public TouristMainWindow()
        {
            InitializeComponent();
            DataContext = this;
            AllTours = tourRepository.GetAll();
            Update();
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

            


        }
    }
}
