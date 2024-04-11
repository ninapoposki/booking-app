using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Injector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class AccommodationGradeService
    {
        private IAccommodationGradeRepository accommodationGradeRepository;

        public AccommodationGradeService()
        {
            accommodationGradeRepository = Injector.Injector.CreateInstance<IAccommodationGradeRepository>();
        }

    }
}
