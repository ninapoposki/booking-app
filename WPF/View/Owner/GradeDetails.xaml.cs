using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.WPF.ViewModel.Owner;
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

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for GradeDetails.xaml
    /// </summary>
    public partial class GradeDetails : Window
    {
        public GradeDetailsVM GradeDetailsVM;
        public GradeDetails(AccommodationGradeDTO selectedAccommodationGrade)
        {
            InitializeComponent();
            GradeDetailsVM = new GradeDetailsVM(selectedAccommodationGrade);
            DataContext = GradeDetailsVM;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
       

       
    }
}
