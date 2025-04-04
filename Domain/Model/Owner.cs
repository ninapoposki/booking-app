﻿using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class Owner: ISerializable
    {

        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        
        public string Role { get; set; }



        public Owner()
        {
            User = new User();
        }

        public Owner(int id, String firstName, String lastName, String phoneNumber, int userId, string role)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.PhoneNumber = phoneNumber;
            this.UserId = userId;
            this.Role = role;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            FirstName = values[1];
            LastName = values[2];
            PhoneNumber = values[3];
            UserId = int.Parse(values[4]);
            Role = values[5];
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                FirstName,
                LastName,
                PhoneNumber,
                UserId.ToString(),
                Role
            };
            return csvValues;
        }

    }
}
