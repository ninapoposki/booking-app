using BookingApp.WPF.ViewModel.Tourist;
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

namespace BookingApp.WPF.View.Tourist
{
    /// <summary>
    /// Interaction logic for ActiveToursWindow.xaml
    /// </summary>
    public partial class ActiveToursWindow : Window
    {

        public ActiveToursWindowVM ActiveToursWindowVM {  get; set; }
        public ActiveToursWindow(int userId)
        {
            ActiveToursWindowVM= new ActiveToursWindowVM(userId);

            InitializeComponent();
            DataContext = ActiveToursWindowVM;
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
