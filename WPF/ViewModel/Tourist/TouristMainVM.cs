using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.Services;
using BookingApp.Utilities;
using BookingApp.WPF.View.Guide;
using BookingApp.WPF.View.Tourist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class TouristMainVM:ViewModelBase
    {
        public MyICommand HomePageCommand { get; set; }
        public MyICommand NotificationPageCommand { get; set; }
        public MyICommand ActiveTourCommand { get; set; }
        public MyICommand ToursToRateCommand { get; set; }
        public MyICommand LogOutCommand { get; set; }
        public MyICommand TourRequestCommand { get; set; }
        public MyICommand TourRequestViewCommand { get; set; }
        private UserService userService;
        public NavigationService NavigationService { get; set; }
        private string username;
        private int userId;


        public TouristMainVM(NavigationService navigationService, string username)
        {
            NavigationService = navigationService;
           
            this.username = username;
            userService = new UserService(Injector.Injector.CreateInstance<IUserRepository>());
            var user = userService.GetByUsername(username);
            if (user != null)
            {
                this.userId = user.Id;
            }

            NavigationService.Navigate(new TouristMainWindow(NavigationService,username));
           HomePageCommand = new MyICommand(HomePageExecute);
            ActiveTourCommand = new MyICommand(ActivePageExecute);
            NotificationPageCommand = new MyICommand(NotificationPageExecute);
            ToursToRateCommand = new MyICommand(ToursToRatePageExecute);
            TourRequestCommand = new MyICommand(TourRequestPageExecute);
            TourRequestViewCommand = new MyICommand(TourRequestViewPageExecute);
         
        }

        private void HomePageExecute()
        {
            NavigationService.Navigate(new TouristMainWindow(NavigationService, username));
        }
        private void ActivePageExecute()
        {
            NavigationService.Navigate(new ActiveToursWindow(NavigationService, userId));
        }
        private void NotificationPageExecute()
        {
            NavigationService.Navigate(new NotificationsWindow(NavigationService));
        }
        private void ToursToRatePageExecute()
        {
            NavigationService.Navigate(new ToursToRateWindow(NavigationService,userId));
        }

       private void TourRequestPageExecute()
        {


            NavigationService.Navigate(new TourRequestWindow(NavigationService));
        }

        private void TourRequestViewPageExecute()
        {

            NavigationService.Navigate(new TourRequestViewWindow(NavigationService));
        }



    }
}
