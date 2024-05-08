using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using BookingApp.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class NotificationsWindowVM:ViewModelBase
    {
        public ObservableCollection<TourGuestDTO> TourGuests { get; set; }
        private TourGuestService tourGuestService;
        private TourGuestDTO selectedTourGuest;
        private CheckPointService checkPointService;
        public MyICommand<TourGuestDTO> MarkAsReadCommand { get; set; }
        public TourGuestDTO SelectedTourGuest
        {
            get { return selectedTourGuest; }
            set
            {
                selectedTourGuest = value;
                OnPropertyChanged(nameof(SelectedTourGuest));
            }
        }

        public NavigationService NavigationService { get; set; }
        public NotificationsWindowVM(NavigationService navigationService) {
            NavigationService = navigationService;
            tourGuestService = new TourGuestService(Injector.Injector.CreateInstance<ITourGuestRepository>());
            TourGuests = new ObservableCollection<TourGuestDTO>();
            SelectedTourGuest=new TourGuestDTO();
            checkPointService = new CheckPointService(Injector.Injector.CreateInstance<ICheckPointRepository>());
            MarkAsReadCommand = new MyICommand<TourGuestDTO>(MarkAsRead);
            Update();
        }

        public void Update()
        { 
            TourGuests.Clear();
            var guests = tourGuestService.GetAll().Where(guest => guest.CheckPointId != -1 && guest.HasArrived != true).Select(guest => new TourGuestDTO(guest)).ToList();
            foreach (var guest in guests)
            {
                guest.CheckPointName = checkPointService.GetName(guest.CheckPointId);
                TourGuests.Add(guest);
            }
        } 
        public void MarkAsRead(TourGuestDTO tourGuest)
        {
            if (tourGuest == null)
            {
                MessageBox.Show("Molim vas izaberite gosta.");
                return;
            }
            tourGuestService.MarkGuestAsArrived(tourGuest);
            Update();
        }


       
    }
}
