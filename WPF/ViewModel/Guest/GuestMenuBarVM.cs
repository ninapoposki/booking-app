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
using System.Windows.Threading;

namespace BookingApp.WPF.ViewModel.Guest
{
    public class GuestMenuBarVM: ViewModelBase
    {
        public MyICommand HomePageCommand { get; set; }
        public MyICommand MyReservationsCommand {  get; set; }
        public MyICommand NotificationsCommand { get; set; }
        public MyICommand OwnersRatingsCommand { get; set; }
        public NavigationService NavigationService { get; set; }
        private readonly UserService userService;
        private readonly GuestService guestService;
        private readonly AccommodationReservationService accommodationReservationService;
        public GuestDTO guestDTO { get; set; }
        private string loggedInUsername;
        public int loggedInUserId;
        private DispatcherTimer updateTimer;

        public GuestMenuBarVM(NavigationService navigationService, string username)
        {
            NavigationService = navigationService;
            userService = new UserService(Injector.Injector.CreateInstance<IUserRepository>());
            guestService = new GuestService(Injector.Injector.CreateInstance<IGuestRepository>() );
            accommodationReservationService = new AccommodationReservationService(Injector.Injector.CreateInstance<IAccommodationReservationRepository>(),
                          Injector.Injector.CreateInstance<IGuestRepository>(),
                          Injector.Injector.CreateInstance<IUserRepository>(),
                          Injector.Injector.CreateInstance<IAccommodationRepository>(),
                          Injector.Injector.CreateInstance<IImageRepository>(),
                          Injector.Injector.CreateInstance<ILocationRepository>(),
                          Injector.Injector.CreateInstance<IOwnerRepository>());
            HomePageCommand = new MyICommand(HomePageExecute);
            MyReservationsCommand = new MyICommand(MyReservationsExecute);
            NotificationsCommand=new MyICommand(NotificationsExecute);
            OwnersRatingsCommand = new MyICommand(OwnersRatingsExecute);
            guestDTO = new GuestDTO();
            loggedInUsername = username;
            loggedInUserId = userService.GetByUsername(loggedInUsername).Id;
            guestDTO = guestService.UpdateGuest(loggedInUserId);
            NavigationService.Navigate(new GuestMainWindow(NavigationService, guestDTO));
            SetNewGuestRole(guestDTO);
            updateTimer = new DispatcherTimer();
            updateTimer.Interval = TimeSpan.FromSeconds(1); 
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();

        }
        private void SetNewGuestRole(GuestDTO guestDTO)
        {
            accommodationReservationService.SetGuestRole(guestDTO);
        }
        private void UpdateTimer_Tick(object sender, EventArgs e){  RefreshGuestData(); }
        private void RefreshGuestData() { 
            guestDTO = guestService.UpdateGuest(loggedInUserId);
            OnPropertyChanged(nameof(GuestDTO)); }
        public void Cleanup()
        {
            updateTimer.Stop();
            updateTimer.Tick -= UpdateTimer_Tick;
        }
     
        private void HomePageExecute()
        {
            NavigationService.Navigate(new GuestMainWindow(NavigationService,guestDTO));
        }
        private void MyReservationsExecute()
        {
            NavigationService.Navigate(new MyReservationsWindow(NavigationService,guestDTO));
        }
        private void NotificationsExecute()
        {
            NavigationService.Navigate(new GuestNotifications(NavigationService));
        }
        private void OwnersRatingsExecute()
        {
            NavigationService.Navigate(new OwnersRatings(NavigationService,loggedInUserId));

        }
     
    }
}
