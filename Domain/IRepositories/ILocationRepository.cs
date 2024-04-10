using BookingApp.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.IRepositories
{
    public interface ILocationRepository
    {
        List<Location> GetAll();
        List<string> GetAllCities(string country);
        HashSet<string> GetAllCountries();
        int GetLocationId(string city, string country);
        Location GetById(int id);

       
    }
}
