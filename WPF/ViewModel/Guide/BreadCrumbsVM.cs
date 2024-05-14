using BookingApp.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookingApp.WPF.ViewModel.Guide
{
    public class BreadcrumbItem:INotifyPropertyChanged
    {
        public string Display { get; set; }
        public MyICommand NavigateCommand { get; set; }
        private bool isLastItem;

        public bool IsLastItem
        {
            get { return isLastItem; }
            set
            {
                isLastItem = value;
                OnPropertyChanged("IsLastItem");
            }
        }
        public BreadcrumbItem(string display, MyICommand navigateCommand)
        {
            Display = display;
            NavigateCommand = navigateCommand;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class BreadCrumbsVM :INotifyPropertyChanged
    {
        public ObservableCollection<BreadcrumbItem> Breadcrumbs { get;  set; }

        public BreadCrumbsVM(ObservableCollection<BreadcrumbItem> breadcrumbs)
        {
            Breadcrumbs = breadcrumbs;
        }
        public BreadCrumbsVM()
        {
            Breadcrumbs = new ObservableCollection<BreadcrumbItem>();
        }
        public void AddBreadcrumb(string display, MyICommand navigateCommand)
        {
            var existingBreadcrumb = Breadcrumbs.FirstOrDefault(b => b.Display == display);
            if (existingBreadcrumb == null)
            {
                Breadcrumbs.Add(new BreadcrumbItem(display, navigateCommand));
                UpdateLastItemProperty();
            }
        }
        public void ResetBreadcrumbs()
        {
            Breadcrumbs.Clear();
            Breadcrumbs.Add(new BreadcrumbItem("Home", new MyICommand(() => {}))); // Provide actual navigation command
            UpdateLastItemProperty();
        }
        public void CutLast()
        {
            Breadcrumbs.RemoveAt(Breadcrumbs.Count - 1);
            UpdateLastItemProperty();
        } 
        public void UpdateLastItemProperty()
        {
            for (int i = 0; i < Breadcrumbs.Count; i++)
            {
                Breadcrumbs[i].IsLastItem = (i != Breadcrumbs.Count - 1);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
