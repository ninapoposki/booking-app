using BookingApp.Model;
using BookingApp.Repository;
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
            if (user != null)
            {
                if(user.Password == txtPassword.Password)
                {
                    if (user.UserType.ToString() =="OWNER") 
                    { 
                    //dajana
                    }
                    else if(user.UserType.ToString() =="GUEST")
                    {
                    //irina
                    }
                    else if (user.UserType.ToString() == "GUIDE")
                    {
                        //ovo je samo proba da vidim je l radi ovde cu biti ja(Anja)
                        MakeTour makeTour = new MakeTour();
                        makeTour.Show();
                         Close();
                    }
                    //arijana
                    else
                    {
                     TouristMainWindow touristMainWindow= new TouristMainWindow();
                        touristMainWindow.Show();
                        Close();
                    }
                  
                } 
                else
                {
                    MessageBox.Show("Wrong password!");
                }
            }
            else
            {
                MessageBox.Show("Wrong username!");
            }
            
        }
    }
}
