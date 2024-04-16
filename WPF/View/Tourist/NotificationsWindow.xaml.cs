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
    /// Interaction logic for NotificationsWindow.xaml
    /// </summary>
    public partial class NotificationsWindow : Window
    {
        public NotificationsWindowVM NotificationsWindowVM { get; set; }
        public NotificationsWindow()
        {
            InitializeComponent();
            NotificationsWindowVM = new NotificationsWindowVM();
            DataContext = NotificationsWindowVM;
        
        }

        private void MarkAsRead(object sender, RoutedEventArgs e)
        {
            NotificationsWindowVM.MarkAsRead();
        }
    }
}
