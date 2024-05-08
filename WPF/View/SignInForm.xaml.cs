using BookingApp.Domain.Model;
using BookingApp.Repository;
using BookingApp.WPF.View.Guest;
using BookingApp.WPF.View.Owner;
using BookingApp.WPF.View.Guide;
using BookingApp.WPF.View.Tourist;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Security.Cryptography.Pkcs;

namespace BookingApp.WPF.View
{
    /// <summary>
    /// Interaction logic for SignInForm.xaml
    /// </summary>
    public partial class SignInForm : Window
    {

        private readonly UserRepository _repository;
        public int UserId { get; set; }
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
            UserRepository.Instance.SetCurrentUserId(user.Id);
            UserId = user.Id;
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

            OwnerMainWindow ownerMainWindow = new OwnerMainWindow(Username);
            ownerMainWindow.Show();
            Close();
        }
        private void OpenGuestWindow()
        {
            GuestMainWindow guestMainWindow = new GuestMainWindow();
           guestMainWindow.Show();
            Close();
        }
        private void OpenGuideWindow()
        {
            GuideMainWindow guideMainWindow = new GuideMainWindow(UserId);
            guideMainWindow.Show();
            Close();
            //MakeTour makeTour = new MakeTour(UserId);
            //makeTour.Show();
            //Close();
        }
        private void OpenTouristWindow()
        {
            TouristMain touristMain = new TouristMain(Username);
            touristMain.Show();
            Close();
        }

    }
}
