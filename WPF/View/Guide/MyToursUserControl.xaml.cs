using BookingApp.WPF.ViewModel.Guide;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookingApp.WPF.View.Guide
{
    /// <summary>
    /// Interaction logic for MyToursUserControl.xaml
    /// </summary>
    public partial class MyToursUserControl : UserControl
    {
        public MyToursUserControl(NavigationService navigationService, int userId,ObservableCollection<BreadcrumbItem> breadcrumbs)
        {
            InitializeComponent();
           DataContext=new MyToursUserControlVM(navigationService, userId,breadcrumbs);
        }
    }
}
