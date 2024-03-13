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

        public OwnerMainWindow(User user)
        {
            InitializeComponent();
            DataContext = this;
            LoggedInUser = user;
            _repository = new CommentRepository();
            Comments = new ObservableCollection<Comment>(_repository.GetByUser(user));

            locationRepository = new LocationRepository(); //add
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
            foreach (Accommodation all in accommodationRepository.GetAll()) {
                AllAccommodation.Add(new AccommodationDTO(all)); 
            }
           
        }


        private void AddAccommodationButton(object sender, RoutedEventArgs e)
        {
            AddAccommodation addAccommodationWindow = new AddAccommodation(accommodationRepository);

            addAccommodationWindow.AccommodationAdded += (sender, args) =>
            {
                UpdateAccommodationDataGrid();
            };
            addAccommodationWindow.ShowDialog();

        }
        private void UpdateAccommodationDataGrid()
        {
            AllAccommodation.Clear();
            foreach (Accommodation all in accommodationRepository.GetAll())
            {
                AllAccommodation.Add(new AccommodationDTO(all));
            }
        }

        private void GradeGuestButton(object sender, RoutedEventArgs e)
        {
            GradeGuestWindow gradeGuestWindow = new GradeGuestWindow();
            gradeGuestWindow.ShowDialog();
        }

        private void AccommodationDataGrid_SelectionChanged(object sender, RoutedEventArgs e)
        {
            // Implementacija koda koji se izvršava kada se selektuje stavka u DataGrid-u
        }
    }
}
