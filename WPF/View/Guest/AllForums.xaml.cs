using BookingApp.DTO;
using BookingApp.Services;
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
    /// Interaction logic for AllForums.xaml
    /// </summary>
    public partial class AllForums : Page
    {
        public AllForumsVM AllForumsVM { get; set; }    
        public AllForums(NavigationService navigationService,int loggedInUserId)
        {
            InitializeComponent();
            AllForumsVM = new AllForumsVM(navigationService,loggedInUserId);
            DataContext = AllForumsVM;
        }

      
    }
}
