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
    public class TourService
    {
        private ITourRepository tourRepository;

        public TourService()
        {
            tourRepository = Injector.Injector.CreateInstance<ITourRepository>();
        }
        public int GetCurrentId()
        {
            return tourRepository.GetCurrentId();
        }

        public Tour Add(Tour tour)
        {
            return tourRepository.Add(tour);
        }
    }
}
