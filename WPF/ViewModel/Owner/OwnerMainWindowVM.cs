using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.DTO;
using BookingApp.Repository;
using BookingApp.WPF.View.Owner;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BookingApp.Domain.IRepositories;
using BookingApp.Utilities;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Navigation;
using System.Threading.Channels;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class OwnerMainWindowVM :  ViewModelBase
    {
        private string loggedInUserUsername;
        public int loggedInUserId;
        private double gradeOwnerLimit = 4.5;
        int gradeNum;
        public OwnerDTO ownerDTO { get; set; }
        public UserService userService;
        public OwnerService ownerService;
        public AccommodationGradeService accommodationGradeService;
        
        public NavigationService NavigationService { get; set; }

        public OwnersAccommodationVM OwnersAccommodation;//= new OwnersAccommodationVM();
                                                         //public MainWindowVM MainWindow = new MainWindowVM(navigation,loggedInUserId);
        
       
        public string title;
        public string Title
        {
            get { return title; }
            set { if (value != title) { title = value; OnPropertyChanged("Title"); } }
        }
     

        public MyICommand<string> NavCommand { get; private set; }

        
       
        public OwnerMainWindowVM(NavigationService navigationService, string username) {

            
            NavigationService = navigationService;
            ownerDTO = new OwnerDTO();
            userService = new UserService(Injector.Injector.CreateInstance<IUserRepository>());
            ownerService = new OwnerService(Injector.Injector.CreateInstance<IOwnerRepository>());
            accommodationGradeService = new AccommodationGradeService(Injector.Injector.CreateInstance<IAccommodationGradeRepository>(),
                Injector.Injector.CreateInstance<IUserRepository>(),
                Injector.Injector.CreateInstance<IOwnerRepository>(),
                Injector.Injector.CreateInstance<IAccommodationRepository>(),
                Injector.Injector.CreateInstance<IImageRepository>(),
                Injector.Injector.CreateInstance<ILocationRepository>());
            loggedInUserUsername = username;
            loggedInUserId = userService.GetByUsername(loggedInUserUsername).Id;
            
            double average = GetAverageGrade();
            AverageGrade = Math.Round(average, 2);
            Update();
            SharedData.Instance.CurrentUserId = loggedInUserId;


            
            NavCommand = new MyICommand<string>(OnNav);
            MainWindow mainWindow = new MainWindow(NavigationService, loggedInUserId);
            NavigationService.Navigate(mainWindow);
            Title = "Home page";


        }
        public void Update(){
            ownerDTO = ownerService.UpdateOwner(loggedInUserId);
            if (gradeNum > 49){ ownerDTO.Role = (AverageGrade > gradeOwnerLimit) ? "SUPEROWNER" : "OWNER";
            } else {  ownerDTO.Role = "OWNER"; }
            string Role =ownerDTO.Role;
            ownerService.UpdateOwnerRole(ownerDTO, Role);
        }
        public double GetAverageGrade() {
            double gradeSum = 0;
            gradeNum = 0;
            foreach( double grade in accommodationGradeService.GetAverageGrades(loggedInUserUsername)){
                gradeNum++;
                gradeSum += grade;
            }
            return gradeSum / (double)gradeNum;
        }
       
      
        private double averageGrade;
        public double AverageGrade {
            get { return averageGrade; }
            set {
                averageGrade = value;
                OnPropertyChanged("AverageGrade");
            }
        }

        private void OnNav(string destination)
            {
           
            switch (destination)
                {
                    case "home":
                         Title = "Home page";
                    CloseNotificationsWindow();
                    MainWindow mainWindow = new MainWindow(NavigationService, loggedInUserId);
                    NavigationService.Navigate(mainWindow);
                    

                         break;
                    case "accomm":
                    CloseNotificationsWindow();
                    Title = "My accommodations";
                    OwnersAccommodation ownerAccommodation = new OwnersAccommodation(NavigationService, loggedInUserId);
                    NavigationService.Navigate(ownerAccommodation);
                     
                         break;
                    case "grade":
                        Title = "My grades";
               
                    CloseNotificationsWindow();

                    OwnerGrades ownerGrades = new OwnerGrades(NavigationService, loggedInUserId);
                    NavigationService.Navigate(ownerGrades);
                    
                       
                        break;
                case "reservations":
                    Title = "My reservations";
                    
                    CloseNotificationsWindow();

                    GuestReservations guestReservations = new GuestReservations(NavigationService, loggedInUserId);
                    NavigationService.Navigate(guestReservations);


                    break;
                case "requests":
                    Title = "Date change requests";
                    

                    CloseNotificationsWindow();
                    DateChangeRequests dateChangeRequests = new DateChangeRequests(NavigationService, loggedInUserId);
                    NavigationService.Navigate(dateChangeRequests);


                    break;
                case "logout":
                    CloseNotificationsWindow();
                    CloseWindow();
                    break;
                case "notifications":
                    Notifications notifications = new Notifications(loggedInUserId);
                    notifications.Show();
                    break;
            }
            }
        private void CloseWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
            }
        }
        private void CloseNotificationsWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Notifications))
                {
                    window.Close();
                    return; 
                }
            }
        }
        
    }
    
        
}