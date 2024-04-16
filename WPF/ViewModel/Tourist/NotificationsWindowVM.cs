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
        private TourGuestDTO _selectedTourGuest;
        public TourGuestDTO SelectedTourGuest
        {
            get { return _selectedTourGuest; }
            set
            {
                _selectedTourGuest = value;
                OnPropertyChanged(nameof(SelectedTourGuest));
            }
        }


        public NotificationsWindowVM() { 
        
            TourGuestService = new TourGuestService();
            TourGuests= new ObservableCollection<TourGuestDTO>();
            SelectedTourGuest=new TourGuestDTO();
            Update();
        }

        public void Update()
        { 
            TourGuests.Clear();
            var guests = TourGuestService.GetAll().Where(guest => guest.CheckPointId != -1 && guest.HasArrived != true).Select(guest => new TourGuestDTO(guest)).ToList();
            foreach (var guest in guests)
            {
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
