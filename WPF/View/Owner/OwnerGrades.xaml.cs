using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Observer;
using BookingApp.Repository;
using BookingApp.WPF.ViewModel.Owner;
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

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for OwnerGrades.xaml
    /// </summary>
    public partial class OwnerGrades : Window
    {
        public OwnerGradesVM OwnerGradesVM;
       
        public OwnerGrades(string username)
        {
            InitializeComponent();
            OwnerGradesVM = new OwnerGradesVM(username);
            DataContext = OwnerGradesVM;
            
        }

        private void GradeDetailsClick(object sender, RoutedEventArgs e)
        {
            OwnerGradesVM.GradeDetailsClick();
        }

       
    }
}
