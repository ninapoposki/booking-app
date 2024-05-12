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
using BookingApp.Domain.IRepositories;

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for AddAccommodation.xaml
    /// </summary>
    public partial class AddAccommodation : Window
    {
        
        public AddAccommodationVM AddAccommodationVM { get; set; }
        public AddAccommodation( int loggedInUserId)
        {
            InitializeComponent();
            AddAccommodationVM = new AddAccommodationVM(loggedInUserId);
            DataContext = AddAccommodationVM;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

      
       /* private void IntegerUpDown_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1)) e.Handled = true;
            if (MaxUpDown.Value == 0) { MaxUpDown.Value = 1; e.Handled = true; }
            if (MinUpDown.Value == 0) { MinUpDown.Value = 1; e.Handled = true; }
        }*/
    }
    
}
