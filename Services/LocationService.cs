using BookingApp.Domain.IRepositories;
using BookingApp.Domain.Model;
using BookingApp.DTO;
using BookingApp.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public class LocationService
    {
        private ILocationRepository locationRepository;

        public LocationService()
        {
            locationRepository = Injector.Injector.CreateInstance<ILocationRepository>();
        }

        public void GetAll(List<LocationDTO> locations)
        {
            locations.Clear();
            foreach (Location location in locationRepository.GetAll()) locations.Add(new LocationDTO(location));
        }

        public string GetCountry(LocationDTO city)
        {
            foreach (Location location in locationRepository.GetAll())
            {
                if (city != null && city.Id == location.Id)
                {
                    return location.Country;
                }
            }
            return null;
        }
        public LocationDTO GetByIdDTO(int id)
        {

            Location location = locationRepository.GetById(id);
            return location != null ? new LocationDTO(location) : null;
        }
        public Location GetById(int id)
        {
            return locationRepository.GetById(id);
        }
    }
}
