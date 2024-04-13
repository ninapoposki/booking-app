using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class TourReservationService
    {
        private ITourReservationRepository tourReservationRepository;
        private TourGuestService tourGuestService;
        public TourReservationService() 
        {
            tourReservationRepository=Injector.Injector.CreateInstance<ITourReservationRepository>();
            tourGuestService=new TourGuestService();
        }
        public bool DoReservationExists(int tourStartId)
        {
            return tourReservationRepository.GetByTourDateId(tourStartId).Count>0;
        }
        public List<TourGuestDTO> GetByStartDate(int id)
        { 
            List<TourGuestDTO> guests = new List<TourGuestDTO>();   
            foreach(TourReservation reservation in tourReservationRepository.GetByTourDateId(id)) 
            {
                guests=tourGuestService.GetGuests(reservation);
            }
            return guests;  
        }
        public List<TourReservation> GetReservationsByStartDate(int id)
        {
            return tourReservationRepository.GetAll().FindAll(t=>t.TourStartDateId==id);
        }
    }
}
