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
        public MyICommand OwnersRatingsCommand { get; set; }
        public NavigationService NavigationService { get; set; }
        private readonly UserService userService;
        private readonly GuestService guestService;
        private readonly AccommodationReservationService accommodationReservationService;
        public GuestDTO guestDTO { get; set; }
        private string loggedInUsername;
        public int loggedInUserId;
        public GuestMenuBarVM(NavigationService navigationService, string username)
        {
            NavigationService = navigationService;
            userService = new UserService(Injector.Injector.CreateInstance<IUserRepository>());
            guestService = new GuestService(Injector.Injector.CreateInstance<IGuestRepository>());
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
            //accommodationReservationService.SetGuestRole(guestDTO);
            SetGuestRole(guestDTO);
        }
        /* public void SetGuestRole(GuestDTO guest)
         {
             var guestReservations = accommodationReservationService.GetOneYearReservations(guestDTO.Id);
             if (guestReservations.Count() >= 10)
             {
                 guestDTO.Role = "SUPERGUEST";
                 guestDTO.Points = 5; //ovde bi ptrebalo da se pozove get current points pa da se apdejtuje svaki put?
             }
             else
             {
                 guestDTO.Role = "GUEST";
                 guestDTO.Points = 0;
             }
             guestService.Update(guestDTO.ToGuest());
         }*/
        public void SetGuestRole(GuestDTO guest)
        {
            var guestReservations = accommodationReservationService.GetOneYearReservations(guestDTO.Id);
            var currentDate = DateTime.Now;
            
            //ako je supergost i ako nije proslo god dana od pocetka titule
            if(guest.Role=="SUPERGUEST")
            {
                //ako je proslo godinu dana od pocetka vazenja supergost titule
                if(guestDTO.SuperGuestTime.AddYears(1) <= currentDate)
                {
                    if (guestReservations.Count() >= 10) //ako e ostvario dovoljno rezervacija u prethodnih godinu dana
                    {
                        guest.Role = "SUPERGUEST";
                        guest.Points = 5;
                        guest.SuperGuestTime = currentDate;
                    }
                    else
                    {
                        guest.Role = "GUEST";
                        guest.Points = 0;
                        guest.SuperGuestTime = DateTime.MinValue;
                    }
                }
                else
                {  //ali ako nije rposlo god dana od pocetka vazenja supergost titule:

                    //uloga i tajming ostaje isti:
                    guest.Points = guestService.GetCurrentGuestPoints(guest.Id);
                }
            }
            else
            {
                //ako je role GUEST
                if (guestReservations.Count() >= 10) //ako e ostvario dovoljno rezervacija u prethodnih godinu dana
                {
                    guest.Role = "SUPERGUEST";
                    guest.Points = 5;
                    guest.SuperGuestTime = currentDate;
                }
                else
                {
                    guest.Role = "GUEST";
                    guest.Points = 0;
                    guest.SuperGuestTime = DateTime.MinValue;

                }

            }
            guestService.Update(guestDTO.ToGuest());

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
