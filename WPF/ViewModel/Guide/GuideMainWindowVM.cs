using BookingApp.Utilities;
using BookingApp.WPF.View.Guide;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class GuideMainWindowVM:ViewModelBase
    {
        public MyICommand HomePageCommand { get; set; }
        public NavigationService NavigationService { get; set; }
        private int userId;
        public GuideMainWindowVM(NavigationService navigationService,int userId)
        {
            this.userId = userId;
            NavigationService = navigationService;
            NavigationService.Navigate(new GuideHomePage(NavigationService,userId));
            HomePageCommand = new MyICommand(HomePageExecute);
        }
        private void HomePageExecute()
        {
            NavigationService.Navigate(new GuideHomePage(NavigationService,userId));  
        }
    }
}
