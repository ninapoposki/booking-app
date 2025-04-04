﻿using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface IGuestGradeRepository
    {
        List<GuestGrade> GetAll();
        bool IsGuestGraded(int reservationId);
        int GetReservationId(AccommodationReservationDTO selectedAccommodationReservation);
        GuestGrade Add(GuestGrade grade);
        int NextId();
        void Delete(GuestGrade guestGrade);
        GuestGrade Update(GuestGrade guestGrade);
        void Subscribe(IObserver observer);
    }
}
