using BookingApp.DTO;
using BookingApp.Domain.Model;
using BookingApp.Observer;
using BookingApp.Repository;
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
using System.Windows.Shapes;

namespace BookingApp.WPF.View.Owner
{
    /// summary>
    /// Interaction logic for OwnerMainWindow.xaml
    /// </summary>
    public partial class OwnerMainWindow : Window, IObserver
    {
        public static ObservableCollection<Comment> Comments { get; set; }

        public Comment SelectedComment { get; set; }

        private User LoggedInUser;
        private string loggedInUserUsername;
        public OwnerDTO OwnerDTO { get; set; }

        private readonly CommentRepository _repository;

        private readonly LocationRepository locationRepository;
        private readonly UserRepository userRepository;
        private readonly OwnerRepository ownerRepository;

        public readonly AccommodationRepository accommodationRepository;
        public ObservableCollection<AccommodationDTO> AllAccommodation { get; set; }

        public OwnerMainWindow(string username)
        {
            InitializeComponent();
            DataContext = this;

            locationRepository = new LocationRepository(); 
            accommodationRepository = new AccommodationRepository();
            userRepository = new UserRepository();
            ownerRepository = new OwnerRepository();
            AllAccommodation = new ObservableCollection<AccommodationDTO>();
            var Accoms = accommodationRepository.GetAll();
            AccommodationDataGrid.ItemsSource = Accoms;
            accommodationRepository.Subscribe(this);

            LoggedInUser  = userRepository.GetByUsername(username);
            loggedInUserUsername = username;
            

            Update();

        }

        public void Update()
        {
            AllAccommodation.Clear();
           
              foreach (Accommodation acc in accommodationRepository.GetAll()) { 
                Location location = locationRepository.GetById(acc.IdLocation);
                AllAccommodation.Add(new AccommodationDTO(acc, location));

                      
              }
               OwnerDTO = new OwnerDTO();

              var owner = ownerRepository.GetByUserId(LoggedInUser.Id);

              OwnerDTO.User = LoggedInUser;

                OwnerDTO = new OwnerDTO(owner);
          
        }


        private void AddAccommodationClick(object sender, RoutedEventArgs e)
        {
            
            
            AddAccommodation addAccommodationWindow = new AddAccommodation(accommodationRepository, loggedInUserUsername);
            addAccommodationWindow.ShowDialog();

        }
      
        private void GradeGuestClick(object sender, RoutedEventArgs e)
        {
            GuestReservations guestReservations = new GuestReservations();
            guestReservations.ShowDialog();
        }
        private void NotificationsClick(object sender, RoutedEventArgs e)
        {
            Notifications notifications = new Notifications();
            notifications.ShowDialog();
        }



    }
}
