using BookingApp.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class TourGradeService
    {

        private ITourGradeRepository tourGradeRepository;

        public TourGradeService()
        {
            tourGradeRepository=Injector.Injector.CreateInstance<ITourGradeRepository>();
        }
    }
}
