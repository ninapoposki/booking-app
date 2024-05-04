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
        public MyICommand HomePageCommand { get;  }
        public MyICommand MyToursCommand { get;  }
        public NavigationService NavigationService { get; set; }
        private int userId;
        public GuideMainWindowVM(NavigationService navigationService,int userId)
        {
            this.userId = userId;
            NavigationService = navigationService;
            NavigationService.Navigate(new GuideHomeUserControl(NavigationService,userId));
            HomePageCommand = new MyICommand(HomeExecute);
            MyToursCommand=new MyICommand(MyToursExecute);
        }

        private void HomeExecute()
        {
            NavigationService.Navigate(new GuideHomeUserControl(NavigationService,userId));  
        }
        private void MyToursExecute()
        {
            NavigationService.Navigate(new MyToursUserControl(NavigationService,userId));   
        }
    }
}
