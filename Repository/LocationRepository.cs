using BookingApp.Model;
using BookingApp.Observer;
using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Repository
{
    public class LocationReporsitory
    {
        private const string FilePath = "../../../Resources/Data/locations.csv";

        private readonly Serializer<Location> serializer;

        private List<Location> locations;


        public LocationReporsitory()
        {
            serializer = new Serializer<Location>();
            locations = serializer.FromCSV(FilePath);
        }

        public List<Location> GetAll()
        {
            return serializer.FromCSV(FilePath);
        }
        
    }
}

