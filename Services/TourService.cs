using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Injector;
using BookingApp.Repository;
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
        private LanguageService languageService;
        private LocationService locationService;
        public TourService()
        {
            tourRepository = Injector.Injector.CreateInstance<ITourRepository>();
            locationService=new LocationService();
            languageService=new LanguageService();
        }
        public int GetCurrentId()
        {
            return tourRepository.GetCurrentId();
        }

        public Tour Add(Tour tour)
        {
            return tourRepository.Add(tour);
        }

        public TourDTO GetTour(int id)
        {
            Tour? todayTour = tourRepository.GetById(id);
            Location location = locationService.GetById(todayTour.LocationId);
            Language language = languageService.GetById(todayTour.LanguageId);
            return new TourDTO(todayTour, location, language);
        }

    }
}
