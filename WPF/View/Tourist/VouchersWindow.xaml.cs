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
    /// Interaction logic for VouchersWindow.xaml
    /// </summary>
    public partial class VouchersWindow : Window
    {
        public VouchersWindow()
        {

            InitializeComponent();

            DataContext = this;

        }

        private void Apply(object sender, RoutedEventArgs e) { 
        
       
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
    }
}
