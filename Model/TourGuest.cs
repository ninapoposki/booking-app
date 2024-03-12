﻿using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Model
{
    public class TourGuest : ISerializable
    {
        public int Id { get; set; }
        public string FullName { get; set; }    
        public int Age { get; set; }
        public int TourReservationId {  get; set; }

        public TourGuest (int  id, string fullName, int age,int tourReservationId)
        {
            Id = id;
            FullName = fullName;
            Age = age;
            TourReservationId=tourReservationId;
        }
        public TourGuest() { }


        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            FullName = values[1];
            Age = Convert.ToInt32(values[2]);
            TourReservationId = Convert.ToInt32(values[3]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {

                Id.ToString(),
                FullName,
                Age.ToString(),
                TourReservationId.ToString()


            };

            return csvValues;
        }
    }
}
