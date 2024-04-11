using BookingApp.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class GuestGradeService
    {
        private IGuestGradeRepository guestGradeRepository;

        public GuestGradeService()
        {
            guestGradeRepository = Injector.Injector.CreateInstance<IGuestGradeRepository>();
        }
    }
}
