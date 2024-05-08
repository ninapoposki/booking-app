using BookingApp.Domain.Model;
using BookingApp.Observer;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingApp.DTO;
using BookingApp.Domain.IRepositories;

namespace BookingApp.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private const string FilePath = "../../../Resources/Data/locations.csv";

        private readonly Serializer<Location> serializer;

        private List<Location> locations;


        public LocationRepository()
        {
            serializer = new Serializer<Location>();
            locations = serializer.FromCSV(FilePath);
        }

        public List<Location> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }

        public List<string> GetAllCities(string country)
        {
            List<Location> locations = serializer.FromCSV(FilePath);

            List<Location> locationsInCountry = locations.Where(loc => loc.Country == country).ToList();
            List<string> cities = new List<string>();
            foreach (Location location in locationsInCountry)
            {
                cities.Add(location.City);
            }

            return cities;
        }

        public HashSet<string> GetAllCountries()
        {
            List<Location> locations = serializer.FromCSV(FilePath);

            HashSet<string> countries = new HashSet<string>();
            foreach (Location location in locations)
            {
                countries.Add(location.Country);
            }

            return countries;
        }

        public int GetLocationId(string city, string country){
            List<Location> locations = serializer.FromCSV(FilePath);
            Location location = locations.FirstOrDefault(loc => loc.City == city );
            return location != null ? location.Id : -1;
        }

        public Location GetById(int id)
        {
            locations = serializer.FromCSV(FilePath);
            return locations.Find(i => i.Id == id);
        }
        public HashSet<string> GetCities()
        {
            List<Location> locations = serializer.FromCSV(FilePath);

            HashSet<string> cities= new HashSet<string>();
            foreach (Location location in locations)
            {
                cities.Add(location.City);
            }

            return cities;
        }
        public List<string> GetAutocompleteCity(string start)
        {
            var allCities = GetCities();
            return allCities.Where(city => city.StartsWith(start, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<string> GetAutocompleteCountry(string start)
        {
            var allCountries = GetAllCountries();
            return allCountries.Where(country => country.StartsWith(start, StringComparison.OrdinalIgnoreCase)).ToList();
        }

    }
}

