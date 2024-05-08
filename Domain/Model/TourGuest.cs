using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public enum Gender{ Male,Female}
    public class TourGuest : ISerializable
    {

        public int Id { get; set; }
        public string FullName { get; set; }    
        public int Age { get; set; }
        public Gender Gender{get; set;}     
        public int TourReservationId {  get; set; }
        public bool HasArrived { get; set; }
        public int CheckPointId {  get; set; }
        public TourGuest (int id, string fullName, int age, int tourReservationId, Gender gender)
        {
            Id = id;
            FullName = fullName;
            Age = age;
            TourReservationId = tourReservationId;
            CheckPointId = -1;
            HasArrived = false;
            Gender = gender;
        }
        public TourGuest() { }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            FullName = values[1];
            Age = Convert.ToInt32(values[2]);
            TourReservationId = Convert.ToInt32(values[3]);
            CheckPointId = Convert.ToInt32(values[4]);
            HasArrived = Convert.ToBoolean(values[5]);
            if (values[6] == "Male") { Gender=Gender.Male; }
            else { Gender=Gender.Female;}       
        }
        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                FullName,
                Age.ToString(),
                TourReservationId.ToString(),
                CheckPointId.ToString(),
                HasArrived.ToString(),
                Gender.ToString()
            };
            return csvValues;
        }
    }
}
