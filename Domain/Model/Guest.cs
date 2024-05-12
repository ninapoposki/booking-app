using BookingApp.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Domain.Model
{
    public class Guest:ISerializable
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public String FirstName { get; set; }
        public string LastName { get; set; }   
        public string PhoneNumber {  get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int Points { get; set; }
        public DateTime SuperGuestTime { get; set; }

        public Guest()
        {
            User=new User();
        }

        public Guest(int id, String firstName,String lastName,String phoneNumber,String email,int userId,string role,int points,DateTime superGuestTime)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
            this.UserId = userId;
            this.Role = role;
            this.Points = points;
            this.SuperGuestTime=superGuestTime;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            FirstName = values[1];
            LastName = values[2];
            PhoneNumber = values[3];
            Email = values[4];
            UserId = int.Parse(values[5]);
            Role = values[6];
            Points = int.Parse(values[7]);
            SuperGuestTime = DateTime.Parse(values[8]);
        }

        public string[] ToCSV()
        {
            string[] csvValues =
            {
                Id.ToString(),
                FirstName,
                LastName,
                PhoneNumber,
                Email,
                UserId.ToString(),
                Role,
                Points.ToString(),
                SuperGuestTime.ToString()
            };
            return csvValues;
        }
    }
}
