using BookingApp.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Model;
using BookingApp.DTO;

namespace BookingApp.Services
{
    public class GuestGradeService
    {
        private IGuestGradeRepository guestGradeRepository;

        public GuestGradeService()
        {
            guestGradeRepository = Injector.Injector.CreateInstance<IGuestGradeRepository>();
        }

        public void Add(GuestGrade guestGrade)
        {
            guestGradeRepository.Add(guestGrade);
        }

        public bool IsGuestGraded(int reservationId)
        {
             return guestGradeRepository.IsGuestGraded(reservationId);
        } 
        public List<GuestGrade> GetAll()
        {
            return guestGradeRepository.GetAll();
        }

        public int GetReservationId(AccommodationReservationDTO selectedAccommodationReservation)
        {
            return guestGradeRepository.GetReservationId(selectedAccommodationReservation);
        }
       
    }
}
