using BookingApp.DTO;
using BookingApp.Model;
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

namespace BookingApp.View.Owner
{
    /// summary>
    /// Interaction logic for OwnerMainWindow.xaml
    /// </summary>
    public partial class OwnerMainWindow : Window, IObserver
    {
        public static ObservableCollection<Comment> Comments { get; set; }

        public Comment SelectedComment { get; set; }

        public User LoggedInUser { get; set; }

        private readonly CommentRepository _repository;

        private readonly LocationRepository locationRepository;

        public readonly AccommodationRepository accommodationRepository;
        public ObservableCollection<AccommodationDTO> AllAccommodation { get; set; }

        public OwnerMainWindow()
        {
            InitializeComponent();
            DataContext = this;
            //LoggedInUser = user;
           // _repository = new CommentRepository();
           // Comments = new ObservableCollection<Comment>(_repository.GetByUser(user));

            locationRepository = new LocationRepository(); 
            accommodationRepository = new AccommodationRepository();
            AllAccommodation = new ObservableCollection<AccommodationDTO>();
            var Accoms = accommodationRepository.GetAll();
            AccommodationDataGrid.ItemsSource = Accoms;
            accommodationRepository.Subscribe(this);
            Update();

        }

        public void Update()
        {
            AllAccommodation.Clear();
           
              foreach (Accommodation acc in accommodationRepository.GetAll()) { 
                Location location = locationRepository.GetById(acc.IdLocation);
                AllAccommodation.Add(new AccommodationDTO(acc, location));

                      /*  AccommodationDTO accommodationDTO = new AccommodationDTO(acc, location);
                        //dodato
                        if (acc.Images.Any())
                    {
                        accommodationDTO.Path = acc.Images.First().Path;
                    }

                    AllAccommodation.Add(accommodationDTO);*/
              }
        }


        private void AddAccommodationClick(object sender, RoutedEventArgs e)
        {
            AddAccommodation addAccommodationWindow = new AddAccommodation(accommodationRepository);
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
