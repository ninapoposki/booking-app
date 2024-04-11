using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class TourStartDateService
    {
        private ITourStartDateRepository tourStartDateRepository;

        public TourStartDateService()
        {
            tourStartDateRepository=Injector.Injector.CreateInstance<ITourStartDateRepository>();
        }

        public void Add(DateTime tourStartDate,int tourId)
        {
            TourStartDate tourDates = new TourStartDate(tourId, tourStartDate);
            tourStartDateRepository.Add(tourDates);
        }
    }
}
