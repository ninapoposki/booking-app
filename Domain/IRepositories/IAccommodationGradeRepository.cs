using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Observer;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IAccommodationGradeRepository
    {
        List<AccommodationGrade> GetAll();
        AccommodationGrade Add(AccommodationGrade accommodationGrade);
        int NextId();

        List<AccommodationGrade> GetByOwnerId(int ownerId);
        List<double> GetAverageGrades(int ownerId);
        int GetCurrentId();
        bool IsReservationGraded(int reservationId);
        int GetReservationId(AccommodationReservation reservation);
      
    }
}
