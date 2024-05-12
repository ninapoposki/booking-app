using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Injector;
using BookingApp.Repository;
using BookingApp.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BookingApp.Services
{
    public class TourService
    {
        private ITourRepository tourRepository; 
        private LocationService locationService;
        private LanguageService languageService;

        public TourService(ITourRepository tourRepository,ILanguageRepository languageRepository, ILocationRepository locationRepository)
        {
            this.tourRepository = tourRepository; 
            locationService = new LocationService(locationRepository);
           languageService = new LanguageService(languageRepository);
        }
        public int GetCurrentId()
        {
            return tourRepository.GetCurrentId();
        }
        public Tour Add(Tour tour)
        {
            return tourRepository.Add(tour);
        }
        //arijana 
        public TourDTO GetById(int id)
        {
            Tour tour = tourRepository.GetById(id);
            return new TourDTO(tour);
        }

        public bool IsCapacitySufficient(int tourId, int numberOfGuests)
        {
            Tour tour = tourRepository.GetById(tourId);
            if (tour == null)
            {
                return false;
            }
            return numberOfGuests <= tour.Capacity;
        }

        public int GetTourCapacity(int tourId)
        {
            Tour tour = tourRepository.GetById(tourId);
            if (tour != null) { return tour.Capacity; }
            else { return -1; }
        }

        public bool UpdateTourCapacity(int tourId, int numberOfGuests, out int remainingCapacity)
        {
            var tour = tourRepository.GetById(tourId);
            if (tour != null)
            {
                tour.Capacity -= numberOfGuests;
                tourRepository.Update(tour);
                remainingCapacity = tour.Capacity;
                return true;
            }
            remainingCapacity = 0;
            return false;
        }


        public TourDTO GetTour(int id)
        {
            Tour? todayTour = tourRepository.GetById(id);
            Location location = locationService.GetById(todayTour.LocationId);
            Language language = languageService.GetById(todayTour.LanguageId);
            return new TourDTO(todayTour, location, language);
        }
       public List<TourDTO> GetAll()
       {
            return tourRepository.GetAll().Select(tour => GetTour(tour.Id)).ToList();
       }
       public List<TourDTO> GetAllForUser(int userId)
       {
            return tourRepository.GetAll().Where(tour => tour.UserId == userId).Select(tour => GetTour(tour.Id)).ToList();
       }

     

    }
}
