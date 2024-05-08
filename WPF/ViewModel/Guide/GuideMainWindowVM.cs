using BookingApp.Utilities;
using BookingApp.WPF.View.Guide;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class GuideMainWindowVM:ViewModelBase
    {
        public BreadCrumbsVM BreadCrumbsVM { get; set; }
        public MyICommand HomePageCommand { get; private set; }
        public MyICommand MyToursCommand { get; private set; }
        public NavigationService NavigationService { get;  set; }
        private int userId;
        public GuideMainWindowVM(NavigationService navigationService,int userId)
        {
            BreadCrumbsVM = new BreadCrumbsVM();
            BreadCrumbsVM.AddBreadcrumb("Home", new MyICommand(() => HomeExecute()));
            this.userId = userId;
            NavigationService = navigationService;
            NavigationService.Navigate(new GuideHomeUserControl(NavigationService,userId,BreadCrumbsVM.Breadcrumbs));
            InitializeCommands();
        }
        private void InitializeCommands()
        {
            HomePageCommand = new MyICommand(HomeExecute);
            MyToursCommand = new MyICommand(MyToursExecute);
        }
        private void HomeExecute()
        {
            NavigationService.Navigate(new GuideHomeUserControl(NavigationService,userId,BreadCrumbsVM.Breadcrumbs));
            ResetBreadcrumbs(BreadCrumbsVM.Breadcrumbs);
        }
        private void MyToursExecute()
        {
            NavigationService.Navigate(new MyToursUserControl(NavigationService, userId,BreadCrumbsVM.Breadcrumbs));
            BreadCrumbsVM.AddBreadcrumb("My Tours",new MyICommand(() => MyToursExecute()));
            if(BreadCrumbsVM.Breadcrumbs.Count > 2) { BreadCrumbsVM.CutLast(); }
        }
        public void ResetBreadcrumbs(ObservableCollection<BreadcrumbItem> breadcrumbs)
        {
            breadcrumbs.Clear();
            breadcrumbs.Add(new BreadcrumbItem("Home", new MyICommand(() => HomeExecute()))); // Provide actual navigation command
            BreadCrumbsVM.UpdateLastItemProperty();
        }
    }
}
