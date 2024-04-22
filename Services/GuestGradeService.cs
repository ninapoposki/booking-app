using BookingApp.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;

namespace BookingApp.Services
{
    public class GuestGradeService
    {
        private IGuestGradeRepository guestGradeRepository;
        private OwnerService ownerService;

        public GuestGradeService(IGuestGradeRepository guestGradeRepository,IOwnerRepository ownerRepository)
        {
            this.guestGradeRepository = guestGradeRepository;
            ownerService = new OwnerService(ownerRepository);
        }
        public void Add(GuestGrade guestGrade)
        {
            guestGradeRepository.Add(guestGrade);
        }
        public Owner GetByUserId(int userId) {
            return ownerService.GetByUserId(userId);
        }
        public bool IsGuestGraded(int reservationId) {
             return guestGradeRepository.IsGuestGraded(reservationId);
        } 
        public List<GuestGrade> GetAll()  {
            return guestGradeRepository.GetAll();
        }
        public int GetReservationId(AccommodationReservationDTO selectedAccommodationReservation)
        {
            return guestGradeRepository.GetReservationId(selectedAccommodationReservation);
        }
       
    }
}
