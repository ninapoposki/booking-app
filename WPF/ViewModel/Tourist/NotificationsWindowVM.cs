using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Tourist
{
    public class NotificationsWindowVM:ViewModelBase
    {
        public ObservableCollection<TourGuestDTO> TourGuests { get; set; }
        private TourGuestService TourGuestService;
        private TourGuestDTO selectedTourGuest;
        private CheckPointService checkPointService;
        public TourGuestDTO SelectedTourGuest
        {
            get { return selectedTourGuest; }
            set
            {
                selectedTourGuest = value;
                OnPropertyChanged(nameof(SelectedTourGuest));
            }
        }


        public NotificationsWindowVM() { 
        
            TourGuestService = new TourGuestService();
            TourGuests= new ObservableCollection<TourGuestDTO>();
            SelectedTourGuest=new TourGuestDTO();
            checkPointService = new CheckPointService();
            Update();
        }

        public void Update()
        { 
            TourGuests.Clear();
            var guests = TourGuestService.GetAll().Where(guest => guest.CheckPointId != -1 && guest.HasArrived != true).Select(guest => new TourGuestDTO(guest)).ToList();
            foreach (var guest in guests)
            {
                guest.CheckPointName = checkPointService.GetName(guest.CheckPointId);
                TourGuests.Add(guest);
            }
        } 
        public void MarkAsRead()
        {
            if (SelectedTourGuest == null)
            {
                MessageBox.Show("Molim vas izaberite gosta.");
                return;
            }
            TourGuestService.MarkGuestAsArrived(SelectedTourGuest);
            Update();
        }


       
    }
}
