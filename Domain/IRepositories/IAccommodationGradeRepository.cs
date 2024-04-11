﻿using BookingApp.Domain.Model;
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
        void Delete(AccommodationGrade accommodationGrade);
        AccommodationGrade Update(AccommodationGrade accommodationGrade);
        void Subscribe(IObserver observer);


    }
}
