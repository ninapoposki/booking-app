using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class Language : ISerializable
    {

        public int Id { get; set; }
        public string Name { get; set; }    


        public Language() { }

        public Language(int id,string name)
        {
            Id = id;
            Name=name;
        }
        public Language(string name)
        {
            Name = name;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                Name
            };

            return csvValues;
        }
       public override string ToString()
        {
            return Name;
        }
    }
}
