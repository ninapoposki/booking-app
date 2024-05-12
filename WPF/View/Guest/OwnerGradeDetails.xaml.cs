using BookingApp.DTO;
using BookingApp.WPF.ViewModel.Guest;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.View.Guest
{
    /// <summary>
    /// Interaction logic for GradeDetails.xaml
    /// </summary>
    public partial class OwnerGradeDetails : Page
    {
        public OwnerGradeDetailsVM  OwnerGradeDetailsVM { get; set; }

        public OwnerGradeDetails(NavigationService navigationService,GuestGradeDTO selectedGrade)
        {
            InitializeComponent();
            OwnerGradeDetailsVM = new OwnerGradeDetailsVM(navigationService,selectedGrade);
            DataContext = OwnerGradeDetailsVM;
        }
    }
}
