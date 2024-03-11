using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class Language : ISerializable
    {

        public int Id { get; set; }
        public string Name { get; set; }    


        public Language() { }

        public Language(string name)
        {
            Name=name;
        }
        public void FromCSV(string[] values)
        {
            Name = values[0];
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Name
            };

            return csvValues;
        }
       
    }
}
