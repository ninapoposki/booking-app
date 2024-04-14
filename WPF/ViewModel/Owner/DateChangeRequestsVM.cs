using BookingApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookingApp.WPF.ViewModel.Owner
{
     public class DateChangeRequestsVM
     {
        public AccommodationReservationService accommodationReservationService;
        public DateChangeRequestsVM() { 

            accommodationReservationService = new AccommodationReservationService();
        }

         public void DeclineButtonClick()
         {

         }
        DateTime initialDate = new DateTime(2024, 4, 15);
        DateTime endDate = new DateTime(2024, 4, 15);
        public void AcceptButtonClick() { 
            accommodationReservationService.UpdateDate(1, initialDate, endDate);
        }
}

}
