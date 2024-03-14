using BookingApp.DTO;
using BookingApp.Model;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookingApp.View.Owner
{
    /// <summary>
    /// Interaction logic for PictureBrowseWindow.xaml
    /// </summary>

    public partial class PictureBrowseWindow : Window
    {
        public static List<ImageDTO> Images { get; set; }
        private ImageRepository imageRepository { get; set; }

        public ImageDTO selectedImage { get; set; }
        public PictureBrowseWindow()
        {
            InitializeComponent();
            DataContext = this;
            Images = new List<ImageDTO>();
            imageRepository = new ImageRepository();
            LoadImages();
        }

        public void LoadImages()
        {
            Images.Clear();
            foreach (Image image in imageRepository.GetAll())
            {
                Images.Add(new ImageDTO(image));
            }
        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You added a picture!");
            this.Close();
        }
    }
}