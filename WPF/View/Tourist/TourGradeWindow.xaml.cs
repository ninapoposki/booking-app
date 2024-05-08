using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.WPF.ViewModel.Guide;
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
    /// Interaction logic for TourGradeWindow.xaml
    /// </summary>
    public partial class TourGradeWindow : Window
    {
        public TourGradeWindowVM tourGradeWindowVM { get; set; }
        
        
        public TourGradeWindow(int tourStartDateId)//treba selected date
        {
            InitializeComponent();
            tourGradeWindowVM = new TourGradeWindowVM(tourStartDateId);
            DataContext = tourGradeWindowVM;
        }
        private int GetSelectedRadioButtonValue(StackPanel panel)
        {

            foreach (var radioButton in panel.Children)
            {
                if (radioButton is RadioButton && ((RadioButton)radioButton).IsChecked == true)
                {
                    return int.Parse(((RadioButton)radioButton).Content.ToString());
                }
            }
            return 0;
        }
        public void Confirm(object sender, RoutedEventArgs e) {

            int knowledge = GetSelectedRadioButtonValue(Knowledge);
            int language = GetSelectedRadioButtonValue(Language);
            int attractions = GetSelectedRadioButtonValue(Attractions);
            string comment = CommentsTextBox.Text;
            tourGradeWindowVM.Confirm(knowledge,language,attractions);
           
            Close();

        }
     
    }
}
