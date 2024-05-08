using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using BookingApp.WPF.View.Guest;
using BookingApp.WPF.View.Guide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class GuestMenuBarVM: ViewModelBase
    {
        
        public MyICommand HomePageCommand { get; set; }
        public MyICommand MyReservationsCommand {  get; set; }
        public MyICommand NotificationsCommand { get; set; }
        public NavigationService NavigationService { get; set; }
        private readonly UserService userService;
        private readonly GuestService guestService;
        public GuestDTO guestDTO { get; set; }
        private string loggedInUsername;
        public int loggedInUserId;
        public GuestMenuBarVM(NavigationService navigationService, string username)
        {
            NavigationService = navigationService;
            userService = new UserService(Injector.Injector.CreateInstance<IUserRepository>());
            guestService = new GuestService(Injector.Injector.CreateInstance<IGuestRepository>());
            HomePageCommand = new MyICommand(HomePageExecute);
            MyReservationsCommand = new MyICommand(MyReservationsExecute);
            NotificationsCommand=new MyICommand(NotificationsExecute);
            guestDTO = new GuestDTO();
            loggedInUsername = username;
            loggedInUserId = userService.GetByUsername(loggedInUsername).Id;
            guestDTO = guestService.UpdateGuest(loggedInUserId);
            NavigationService.Navigate(new GuestMainWindow(NavigationService, guestDTO));
        }
        private void HomePageExecute()
        {
            NavigationService.Navigate(new GuestMainWindow(NavigationService,guestDTO));
        }
        private void MyReservationsExecute()
        {
            NavigationService.Navigate(new MyReservationsWindow(NavigationService));
        }
        private void NotificationsExecute()
        {
            NavigationService.Navigate(new GuestNotifications(NavigationService));
        }



    }
}
