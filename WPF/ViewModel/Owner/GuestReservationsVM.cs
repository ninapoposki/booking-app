using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.WPF.ViewModel.Owner
{
    public class GuestReservationsVM : ViewModelBase
    {
        private AccommodationService accommodationService;
        public GuestReservationsVM() {
            accommodationService = new AccommodationService();
        }
    }
}
