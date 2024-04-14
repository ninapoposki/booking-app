using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class Tour : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Language Language { get; set; } 
        
        public int LocationId {  get; set; }
        public int LanguageId{ get; set; }
        public Location Location { get; set; }
        public int Capacity { get; set; }
        public double Duration {  get; set; }
        public int UserId {  get; set; }
        public User User {  get; set; }
        public Tour() 
        { 
           
            Location = new Location();
            Language = new Language();
        }
        public Tour(int id, string name, string description, Language language, Location location, int capacity, double duration, int userId)
        {
            Id = id;
            Name = name;
            Description = description;
            Language = language;
            Location = location;
            Capacity = capacity;
            Duration = duration;
            UserId = userId;
        }

        public Tour(int id, string name, string description, int languageId,int locationId, int capacity, double duration,int userId)
        {
            Id = id;
            Name = name;
            Description = description;
            LanguageId = languageId;
            LocationId = locationId;
            Capacity = capacity;
            Duration = duration;
            UserId=userId;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Description = values[2];
            LanguageId = Convert.ToInt32(values[3]);
            LocationId = Convert.ToInt32(values[4]);    
            Capacity = Convert.ToInt32(values[5]);
            Duration = Convert.ToDouble(values[6]);
            UserId= Convert.ToInt32(values[7]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
          {
                Id.ToString(),
                Name,
                Description,
                LanguageId.ToString(),
                LocationId.ToString(),
                Capacity.ToString(),
                Duration.ToString(),
                UserId.ToString(),

        };
            return csvValues;
        }
    }
}
