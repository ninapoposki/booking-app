using BookingApp.DTO;
using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.WPF.View.Guest;
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

namespace BookingApp.WPF.View.Owner
{
    /// <summary>
    /// Interaction logic for PictureWindow.xaml
    /// </summary>

    public partial class PictureWindow : Window
    {
        public static List<ImageDTO> Images { get; set; }
        private ImageRepository imageRepository { get; set; }

        public ImageDTO selectedImage { get; set; }
        public PictureWindow()
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
                bool isAvailable = image.EntityId == -1 && image.EntityType.ToString().Equals("NONE");
                if (isAvailable)
                {
                    Images.Add(new ImageDTO(image));
                }
            }
        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            if (selectedImage != null)
            {
                MessageBox.Show("You added a picture!");
                this.Close();
            }
        }
    }
}