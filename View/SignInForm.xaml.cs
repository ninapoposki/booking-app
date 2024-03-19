using BookingApp.Model;
using BookingApp.Repository;
using BookingApp.View.Guest;
using BookingApp.View.Owner;
using BookingApp.View.Guide;
using BookingApp.View.Tourist;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace BookingApp.View
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {

        private readonly UserRepository _repository;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SignInForm()
        {
            InitializeComponent();
            DataContext = this;
            _repository = new UserRepository();
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            User user = _repository.GetByUsername(Username);
            if (user == null)
            {
                MessageBox.Show("Wrong username!");
                return;
            }
            if (user.Password != txtPassword.Password)
            {
                MessageBox.Show("Wrong password!");
                return;
            }
            HandleUserSignIn(user.UserType);   
        }
        private void HandleUserSignIn(UserType type) { 
        
            switch(type)
            {
                case UserType.OWNER:
                    OpenOwnerWindow();
                    break;
                case UserType.GUEST:
                    OpenGuestWindow();
                    break;
                case UserType.GUIDE:
                    OpenGuideWindow();
                    break;
                default:
                    OpenTouristWindow();
                    break;
            }
        }
        private void OpenOwnerWindow()
        {
            //OwnerMainWindow ownerMainWindow = new OwnerMainWindow(user);
            //ownerMainWindow.Show();
            //Close();
        }
        private void OpenGuestWindow()
        {
            GuestMainWindow guestMainWindow = new GuestMainWindow();
            guestMainWindow.Show();
            Close();
        }
        private void OpenGuideWindow()
        {
           
            MakeTour makeTour = new MakeTour();
            makeTour.Show();
            Close();
        }
        private void OpenTouristWindow()
        {
            TouristMainWindow touristMainWindow = new TouristMainWindow();
            touristMainWindow.Show();
            Close();
        }

    }
}
