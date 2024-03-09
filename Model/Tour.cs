using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class Tour : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Language Language { get; set; }  
        public Location Location { get; set; }
        public int Capacity { get; set; }
        public double Duration {  get; set; }
        public List<Image> Images { get; set; }

        public Tour() 
        { 
            Images = new List<Image>();
            Location = new Location();
            Language = new Language();
        }
        public Tour(int id, string name, string description, Language language, Location location, int capacity, double duration, List<Image> images)
        {
            Id = id;
            Name = name;
            Description = description;
            Language = language;
            Location = location;
            Capacity = capacity;
            Duration = duration;
            Images = new List<Image>();
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Description = values[2];
            string[] locationParametars = values[3].Split(",");
            Location.Id = Convert.ToInt32(locationParametars[0]);
            Location.City = locationParametars[1];
            Location.Country = locationParametars[2];   
            Language.Name = values[4];
            Capacity = Convert.ToInt32(values[5]);
            Duration = Convert.ToDouble(values[6]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
          {
                Id.ToString(),
                Name,
                Description,
                Location.ToString(),
                Language.Name.ToString(),
                Capacity.ToString(),
                Duration.ToString()

        };
            return csvValues;
        }
    }
}
